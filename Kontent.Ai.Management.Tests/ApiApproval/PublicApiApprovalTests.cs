using System.Reflection;
using System.Text;
using VerifyXunit;
using Xunit;

namespace Kontent.Ai.Management.Tests.ApiApproval;

public class PublicApiApprovalTests
{
    [Fact]
    public Task ManagementPublicApi_ShouldNotChangeUnexpectedly()
    {
        var assembly = typeof(Kontent.Ai.Management.IManagementClient).Assembly;
        var publicApi = GetPublicApiSurface(assembly);
        return Verifier.Verify(publicApi);
    }

    private static string GetPublicApiSurface(Assembly assembly)
    {
        var sb = new StringBuilder();

        var publicTypes = assembly.GetExportedTypes()
            .OrderBy(t => t.Namespace)
            .ThenBy(t => t.Name);

        foreach (var type in publicTypes)
        {
            sb.AppendLine($"// {type.Namespace}");
            sb.AppendLine(GetTypeSignature(type));

            foreach (var member in GetPublicMembers(type))
            {
                sb.AppendLine($"    {member}");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    private static string GetTypeSignature(Type type)
    {
        var kind = GetKind(type);

        var modifiers = type.IsSealed && !type.IsValueType ? "sealed " : "";
        var generic = type.IsGenericType
            ? $"<{string.Join(", ", type.GetGenericArguments().Select(a => a.Name))}>"
            : string.Empty;
        var baseTypes = GetBaseTypes(type);

        return $"public {modifiers}{kind} {type.Name}{generic}{baseTypes}";
    }

    private static string GetKind(Type type)
    {
        if (type.IsInterface)
        {
            return "interface";
        }

        if (type.IsEnum)
        {
            return "enum";
        }

        if (type.IsValueType)
        {
            return "struct";
        }

        return "class";
    }

    private static string GetBaseTypes(Type type)
    {
        var bases = new List<string>();

        if (type.BaseType is not null && type.BaseType != typeof(object) && type.BaseType != typeof(ValueType))
        {
            bases.Add(type.BaseType.Name);
        }

        bases.AddRange(type.GetInterfaces()
            .Where(i => !type.BaseType?.GetInterfaces().Contains(i) ?? true)
            .Select(i => i.Name));

        return bases.Count > 0 ? " : " + string.Join(", ", bases) : string.Empty;
    }

    private static IEnumerable<string> GetPublicMembers(Type type)
    {
        if (type.IsEnum)
        {
            foreach (var name in Enum.GetNames(type).OrderBy(x => x))
            {
                yield return name;
            }

            yield break;
        }

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .OrderBy(p => p.Name))
        {
            var getter = prop.GetMethod?.IsPublic == true ? "get; " : "";
            var setter = prop.SetMethod?.IsPublic == true ? "set; " : "";
            var init = prop.SetMethod?.ReturnParameter.GetRequiredCustomModifiers()
                .Any(m => m.Name == "IsExternalInit") == true ? "init; " : "";
            yield return $"{FormatTypeName(prop.PropertyType)} {prop.Name} {{ {getter}{setter}{init}}}";
        }

        foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName)
            .OrderBy(m => m.Name)
            .ThenBy(m => m.GetParameters().Length))
        {
            var staticPrefix = method.IsStatic ? "static " : "";
            var generic = method.IsGenericMethod
                ? $"<{string.Join(", ", method.GetGenericArguments().Select(a => a.Name))}>"
                : string.Empty;
            var parameters = string.Join(", ", method.GetParameters().Select(FormatParameter));
            yield return $"{staticPrefix}{FormatTypeName(method.ReturnType)} {method.Name}{generic}({parameters})";
        }
    }

    private static string FormatParameter(ParameterInfo parameter)
        => $"{FormatTypeName(parameter.ParameterType)} {parameter.Name}";

    private static string FormatTypeName(Type type)
    {
        if (type.IsGenericType)
        {
            var typeName = type.Name[..type.Name.IndexOf('`')];
            var args = string.Join(", ", type.GetGenericArguments().Select(FormatTypeName));
            return $"{typeName}<{args}>";
        }

        if (type.IsArray)
        {
            return $"{FormatTypeName(type.GetElementType()!)}[]";
        }

        return type.Name;
    }
}
