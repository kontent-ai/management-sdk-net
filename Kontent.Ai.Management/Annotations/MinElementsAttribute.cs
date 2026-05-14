namespace Kontent.Ai.Management.Annotations;

/// <summary>
/// Minimum number of items required in a collection element (asset, linked items, subpages, taxonomy, multiple
/// choice). The validator surfaces an error when a non-null collection has fewer items than <see cref="N"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class MinElementsAttribute(int n) : Attribute
{
    /// <summary>Lower bound (inclusive).</summary>
    public int N { get; } = n;
}
