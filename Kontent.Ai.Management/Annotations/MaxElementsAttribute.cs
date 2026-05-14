namespace Kontent.Ai.Management.Annotations;

/// <summary>
/// Maximum number of items allowed in a collection element (asset, linked items, subpages, taxonomy, multiple
/// choice). The validator surfaces an error when a non-null collection has more items than <see cref="N"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class MaxElementsAttribute(int n) : Attribute
{
    /// <summary>Upper bound (inclusive).</summary>
    public int N { get; } = n;
}
