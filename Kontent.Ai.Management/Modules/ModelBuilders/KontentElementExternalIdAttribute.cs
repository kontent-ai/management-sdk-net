using System;

namespace Kontent.Ai.Management.Modules.ModelBuilders;

/// <summary>
/// Represents the element external id of an element.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class KontentElementExternalIdAttribute : Attribute
{
    private readonly string elementExternalId;

    /// <summary>
    /// Constructor for the attribute.
    /// </summary>
    /// <param name="elementExternalId">The external id of the element.</param>
    public KontentElementExternalIdAttribute(string elementExternalId)
    {
        this.elementExternalId = elementExternalId;
    }

    /// <summary>
    /// Gets the id of the element.
    /// </summary>
    public virtual string ElementExternalId => elementExternalId;
}
