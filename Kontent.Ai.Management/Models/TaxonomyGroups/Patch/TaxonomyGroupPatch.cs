namespace Kontent.Ai.Management.Models.TaxonomyGroups.Patch;

/// <summary>
/// Intent-revealing factories for the taxonomy-group <c>replace</c> operations, ready to pass to
/// <c>ModifyTaxonomyGroupAsync</c>. Each factory names the property it replaces and takes the correctly-typed value;
/// the <c>target</c> is the taxonomy group itself or one of its terms. The <c>addInto</c>, <c>remove</c>,
/// and <c>move</c> operations already carry typed values — construct their models directly.
/// </summary>
public static class TaxonomyGroupPatch
{
    /// <summary>Replaces the display name of the group or term identified by <paramref name="target"/>.</summary>
    public static TaxonomyGroupReplacePatchModel ReplaceName(Reference target, string name)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(name);
        return new() { Reference = target, PropertyName = TaxonomyGroupPropertyName.Name, Value = name };
    }

    /// <summary>Replaces the codename of the group or term identified by <paramref name="target"/>.</summary>
    public static TaxonomyGroupReplacePatchModel ReplaceCodename(Reference target, string codename)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(codename);
        return new() { Reference = target, PropertyName = TaxonomyGroupPropertyName.Codename, Value = codename };
    }

    /// <summary>Replaces all child terms of the group or term identified by <paramref name="target"/>.</summary>
    public static TaxonomyGroupReplacePatchModel ReplaceTerms(Reference target, params TaxonomyTermCreateModel[] terms)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(terms);
        return new() { Reference = target, PropertyName = TaxonomyGroupPropertyName.Terms, Value = terms };
    }
}
