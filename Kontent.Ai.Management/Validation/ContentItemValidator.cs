using Kontent.Ai.Management.Annotations;
using Kontent.Ai.Management.Models.Content;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Kontent.Ai.Management.Validation;

/// <summary>
/// Validates a generated content-type record instance against the constraints encoded in its
/// <see cref="Kontent.Ai.Management.Annotations"/> attributes and selected <see cref="System.ComponentModel.DataAnnotations"/>
/// attributes. Best-effort precheck — the MAPI remains the source of truth for what's accepted. Constraints that
/// need server-side context (<c>[MaxAssetSize]</c>, <c>[AllowedAssetFileTypes]</c>, <c>[AllowedTaxonomyGroup]</c>,
/// <c>[AllowedItemLinkTypes]</c>) are recorded on the property but not enforced.
/// </summary>
/// <remarks>
/// Rule construction is cached per <see cref="Type"/> so repeated validations of the same content-type record only
/// pay reflection cost once.
/// </remarks>
public static class ContentItemValidator
{
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<PropertyRule>> RuleCache = new();

    /// <summary>
    /// Validates <paramref name="item"/>. Returns a success result wrapping the same instance on a clean walk,
    /// or a failure result whose <see cref="IError.ValidationErrors"/> name each violation.
    /// </summary>
    public static IManagementResult<T> Validate<T>(T item) where T : IElementsModel
    {
        ArgumentNullException.ThrowIfNull(item);

        var rules = RuleCache.GetOrAdd(item.GetType(), BuildRules);
        var errors = new List<ValidationError>();

        foreach (var rule in rules)
        {
            rule.Check(item, errors);
        }

        return errors.Count == 0
            ? ManagementResult<T>.Success(item)
            : ManagementResult<T>.Failure(new Error
            {
                Message = "Content item validation failed.",
                ValidationErrors = errors,
            });
    }

    private static IReadOnlyList<PropertyRule> BuildRules(Type type)
    {
        var rules = new List<PropertyRule>();

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var elementAttr = prop.GetCustomAttribute<KontentElementAttribute>();
            if (elementAttr is null)
            {
                continue;
            }

            var checks = BuildChecks(prop, elementAttr.Codename);
            if (checks.Length > 0)
            {
                rules.Add(new PropertyRule(prop, checks));
            }
        }

        return rules;
    }

    private static Check[] BuildChecks(PropertyInfo prop, string codename)
    {
        var checks = new List<Check>();

        if (prop.GetCustomAttribute<StringLengthAttribute>() is { } stringLength)
        {
            var max = stringLength.MaximumLength;
            checks.Add((value, errors) =>
            {
                if (value is string s && s.Length > max)
                {
                    errors.Add(new ValidationError
                    {
                        Message = $"Value length {s.Length} exceeds the maximum of {max} characters.",
                        Path = codename,
                    });
                }
            });
        }

        if (prop.GetCustomAttribute<RegularExpressionAttribute>() is { } regexAttr)
        {
            var regex = new Regex(regexAttr.Pattern);
            var pattern = regexAttr.Pattern;
            checks.Add((value, errors) =>
            {
                if (value is string s && !regex.IsMatch(s))
                {
                    errors.Add(new ValidationError
                    {
                        Message = $"Value does not match the required pattern '{pattern}'.",
                        Path = codename,
                    });
                }
            });
        }

        if (prop.GetCustomAttribute<MinElementsAttribute>() is { } minElements)
        {
            var min = minElements.N;
            checks.Add((value, errors) =>
            {
                if (TryGetCount(value, out var count) && count < min)
                {
                    errors.Add(new ValidationError
                    {
                        Message = $"Collection must contain at least {min} item(s); got {count}.",
                        Path = codename,
                    });
                }
            });
        }

        if (prop.GetCustomAttribute<MaxElementsAttribute>() is { } maxElements)
        {
            var max = maxElements.N;
            checks.Add((value, errors) =>
            {
                if (TryGetCount(value, out var count) && count > max)
                {
                    errors.Add(new ValidationError
                    {
                        Message = $"Collection must contain at most {max} item(s); got {count}.",
                        Path = codename,
                    });
                }
            });
        }

        if (prop.GetCustomAttribute<ExactElementsAttribute>() is { } exactElements)
        {
            var n = exactElements.N;
            checks.Add((value, errors) =>
            {
                if (TryGetCount(value, out var count) && count != n)
                {
                    errors.Add(new ValidationError
                    {
                        Message = $"Collection must contain exactly {n} item(s); got {count}.",
                        Path = codename,
                    });
                }
            });
        }

        if (prop.GetCustomAttribute<AllowedTypesAttribute>() is { } allowedTypes)
        {
            var allowed = new HashSet<string>(allowedTypes.Codenames, StringComparer.Ordinal);
            // Enforceable only for rich-text components — the one place MAPI embeds typed content. On linked-items /
            // subpages the value is bare id/codename references with no type information, so the allow-list there is
            // server-enforced; the attribute is carried for schema/documentation, not checked here.
            checks.Add((value, errors) =>
            {
                if (value is RichTextElement { Components: { } components })
                {
                    foreach (var component in components)
                    {
                        CheckAllowedType(component.Content, allowed, codename, errors);
                    }
                }
            });
        }

        // [MaxAssetSize], [AllowedAssetFileTypes], [AllowedTaxonomyGroup], [AllowedItemLinkTypes] intentionally
        // produce no check — the data needed to enforce them isn't present on the in-memory record.

        return checks.ToArray();
    }

    private static void CheckAllowedType(IElementsModel item, HashSet<string> allowed, string codename, List<ValidationError> errors)
    {
        var typeAttr = item.GetType().GetCustomAttribute<KontentTypeAttribute>();
        if (typeAttr is null || !allowed.Contains(typeAttr.Codename))
        {
            var label = typeAttr?.Codename ?? item.GetType().Name;
            errors.Add(new ValidationError
            {
                Message = $"Content type '{label}' is not allowed; permitted: {string.Join(", ", allowed)}.",
                Path = codename,
            });
        }
    }

    private static bool TryGetCount(object? value, out int count)
    {
        switch (value)
        {
            case null:
                count = 0;
                return false;
            case ICollection collection:
                count = collection.Count;
                return true;
            default:
                count = 0;
                return false;
        }
    }

    private delegate void Check(object? value, List<ValidationError> errors);

    private sealed class PropertyRule(PropertyInfo property, Check[] checks)
    {
        public void Check(object instance, List<ValidationError> errors)
        {
            var value = property.GetValue(instance);
            foreach (var check in checks)
            {
                check(value, errors);
            }
        }
    }
}
