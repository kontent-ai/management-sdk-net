using Kontent.Ai.Management.Models.AssetRenditions;
using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

internal sealed class ImageTransformationJsonConverter : PolymorphicJsonConverter<ImageTransformation>
{
    protected override string DiscriminatorPropertyName => "mode";

    protected override Type ResolveType(string discriminator) => discriminator switch
    {
        "rect" => typeof(RectangleResizeTransformation),
        _ => throw new JsonException($"Unknown image transformation mode '{discriminator}'."),
    };
}
