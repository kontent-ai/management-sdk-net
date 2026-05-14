namespace Kontent.Ai.Management.Annotations;

/// <summary>
/// Exact number of items required in a collection element. The validator surfaces an error when a non-null
/// collection's count differs from <see cref="N"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class ExactElementsAttribute(int n) : Attribute
{
    /// <summary>Required count (exact).</summary>
    public int N { get; } = n;
}
