namespace Kontent.Ai.Management.Models.Spaces.Patch;

/// <summary>
/// Intent-revealing factories for the modify-space patch operations, ready to pass to <c>ModifySpaceAsync</c>.
/// The endpoint supports only <c>replace</c>, so each factory names the property it replaces and takes the
/// correctly-typed value.
/// </summary>
public static class SpacePatch
{
    /// <summary>Replaces the space's display name.</summary>
    public static SpaceReplacePatchModel Name(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        return new() { PropertyName = SpacePropertyName.Name, Value = name };
    }

    /// <summary>Replaces the space's codename.</summary>
    public static SpaceReplacePatchModel Codename(string codename)
    {
        ArgumentNullException.ThrowIfNull(codename);
        return new() { PropertyName = SpacePropertyName.Codename, Value = codename };
    }

    /// <summary>Replaces the space's root item. Pass <c>null</c> to unset it.</summary>
    public static SpaceReplacePatchModel RootItem(Reference? rootItem) =>
        new() { PropertyName = SpacePropertyName.RootItem, Value = rootItem };

    /// <summary>Replaces the collections belonging to the space.</summary>
    public static SpaceReplacePatchModel Collections(params Reference[] collections)
    {
        ArgumentNullException.ThrowIfNull(collections);
        return new() { PropertyName = SpacePropertyName.Collections, Value = collections };
    }
}
