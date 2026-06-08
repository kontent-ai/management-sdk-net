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
    public static IManagementResult<T> Validate<T>(T item) where T : IContentItem
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
            // Compile once per property; closure captures the compiled regex.
            var regex = new Regex(regexAttr.Pattern, RegexOptions.Compiled);
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
            checks.Add((value, errors) =>
            {
                switch (value)
                {
                    // Rich-text element: every embedded component's content type must be in the allow-list.
                    case RichTextElement { Components: { } components }:
                        foreach (var component in components)
                        {
                            CheckAllowedType(component.Content, allowed, codename, errors);
                        }
                        break;

                    // Linked-items / subpages style collection. Conventionally IReadOnlyList<IContentItem>?,
                    // but stay robust if the attribute is misapplied to a non-IContentItem collection.
                    case IEnumerable enumerable and not string:
                        foreach (var entry in enumerable)
                        {
                            if (entry is IContentItem item)
                            {
                                CheckAllowedType(item, allowed, codename, errors);
                            }
                        }
                        break;
                }
            });
        }

        // [MaxAssetSize], [AllowedAssetFileTypes], [AllowedTaxonomyGroup], [AllowedItemLinkTypes] intentionally
        // produce no check — the data needed to enforce them isn't present on the in-memory record.

        return checks.ToArray();
    }

    private static void CheckAllowedType(IContentItem item, HashSet<string> allowed, string codename, List<ValidationError> errors)
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
            case IEnumerable enumerable:
                var c = 0;
                foreach (var _ in enumerable)
                {
                    c++;
                }
                count = c;
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
