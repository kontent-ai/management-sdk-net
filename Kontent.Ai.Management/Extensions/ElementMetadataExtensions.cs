using Kontent.Ai.Management.Models.Types.Elements;

namespace Kontent.Ai.Management.Extensions;

/// <summary>
/// Extensions methods for the ElementMetadataBase class.
/// </summary>
public static class ElementMetadataExtension
{
    /// <summary>
    /// Casts the base element metadata to a concrete element type.
    /// </summary>
    /// <param name="source">The element metadata to cast.</param>
    /// <typeparam name="T">The concrete element metadata type to cast to.</typeparam>
    /// <returns>The element typed as <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidOperationException">The element is not of type <typeparamref name="T"/>.</exception>
    public static T ToElement<T>(this ElementMetadataBase source) where T : ElementMetadataBase
    {
        ArgumentNullException.ThrowIfNull(source);

        return source as T
            ?? throw new InvalidOperationException($"Element of type {source.Type} cannot be cast to {typeof(T).Name}.");
    }
}
