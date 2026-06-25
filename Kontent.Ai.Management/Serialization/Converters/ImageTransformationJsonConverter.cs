using Kontent.Ai.Management.Models.AssetRenditions;
using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

internal sealed class ImageTransformationJsonConverter : PolymorphicJsonConverter<ImageTransformation, ImageTransformationMode>
{
    protected override string DiscriminatorPropertyName => "mode";

    protected override Type ResolveType(ImageTransformationMode discriminator) => discriminator switch
    {
        ImageTransformationMode.Rect => typeof(RectangleResizeTransformation),
        _ => throw new JsonException($"No image transformation subtype for '{discriminator}'."),
    };
}
