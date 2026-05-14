namespace Kontent.Ai.Management;

/// <summary>
/// Marker for generated content-type records. Records emitted by <c>model-generator-net</c>'s management mode
/// implement this so the SDK validator, STJ envelope converter, and strongly-typed API methods can recognize them.
/// </summary>
public interface IContentItem
{
}
