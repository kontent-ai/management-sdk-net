using System;

namespace Kontent.Ai.Management.Modules.ModelBuilders;

/// <summary>
/// Represents the element id of an element.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class KontentElementIdAttribute : Attribute
{
    private readonly string elementId;

    /// <summary>
    /// Constructor for the attribute.
    /// </summary>
    /// <param name="elementId">The id of the element.</param>
    public KontentElementIdAttribute(string elementId)
    {
        this.elementId = elementId;
    }

    /// <summary>
    /// Gets the id of the element.
    /// </summary>
    public virtual string ElementId => elementId;
}
