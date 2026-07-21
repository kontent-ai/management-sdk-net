namespace Kontent.Ai.Management.Models.CustomApps.Patch;

/// <summary>
/// Intent-revealing factories for custom-app patch operations, ready to pass to <c>ModifyCustomAppAsync</c>.
/// Each factory bundles the verb, the property, and the correctly-typed value.
/// </summary>
public static class CustomAppPatch
{
    /// <summary>Replaces the custom app's display name.</summary>
    public static CustomAppReplacePatchModel ReplaceName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        return new() { PropertyName = CustomAppPropertyName.Name, Value = name };
    }

    /// <summary>Replaces the custom app's codename.</summary>
    public static CustomAppReplacePatchModel ReplaceCodename(string codename)
    {
        ArgumentNullException.ThrowIfNull(codename);
        return new() { PropertyName = CustomAppPropertyName.Codename, Value = codename };
    }

    /// <summary>Replaces the custom app's source URL.</summary>
    public static CustomAppReplacePatchModel ReplaceSourceUrl(string sourceUrl)
    {
        ArgumentNullException.ThrowIfNull(sourceUrl);
        return new() { PropertyName = CustomAppPropertyName.SourceUrl, Value = sourceUrl };
    }

    /// <summary>Replaces the custom app's config. Pass <c>null</c> to clear it.</summary>
    public static CustomAppReplacePatchModel ReplaceConfig(object? config) =>
        new() { PropertyName = CustomAppPropertyName.Config, Value = config };

    /// <summary>Replaces how the custom app is displayed in the UI.</summary>
    public static CustomAppReplacePatchModel ReplaceDisplayMode(CustomAppDisplayMode displayMode) =>
        new() { PropertyName = CustomAppPropertyName.DisplayMode, Value = displayMode };

    /// <summary>Replaces the whole set of roles allowed to use the custom app.</summary>
    public static CustomAppReplacePatchModel ReplaceAllowedRoles(params Reference[] roles)
    {
        ArgumentNullException.ThrowIfNull(roles);
        return new() { PropertyName = CustomAppPropertyName.AllowedRoles, Value = roles };
    }

    /// <summary>Adds a role to the set allowed to use the custom app.</summary>
    public static CustomAppAddIntoPatchModel AddAllowedRole(Reference role)
    {
        ArgumentNullException.ThrowIfNull(role);
        return new() { PropertyName = CustomAppPropertyName.AllowedRoles, Value = role };
    }

    /// <summary>Removes a role from the set allowed to use the custom app.</summary>
    public static CustomAppRemovePatchModel RemoveAllowedRole(Reference role)
    {
        ArgumentNullException.ThrowIfNull(role);
        return new() { PropertyName = CustomAppPropertyName.AllowedRoles, Value = role };
    }
}
