namespace Kontent.Ai.Management.Annotations;

/// <summary>
/// Binds a taxonomy element to a specific taxonomy group, identified by either its codename or id. The MAPI
/// rejects terms from outside the named group at upsert time; the validator records the constraint but does
/// not have enough information to enforce it locally.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class AllowedTaxonomyGroupAttribute(string codenameOrId) : Attribute
{
    /// <summary>Taxonomy group codename or identifier (GUID).</summary>
    public string CodenameOrId { get; } = codenameOrId ?? throw new ArgumentNullException(nameof(codenameOrId));
}
