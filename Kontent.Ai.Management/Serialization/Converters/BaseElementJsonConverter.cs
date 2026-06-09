using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

/// <summary>
/// Serializes a <see cref="BaseElement"/> by its runtime type. Element collections are statically typed as
/// <c>IReadOnlyList&lt;BaseElement&gt;</c>; without this, System.Text.Json would serialize each item against the
/// declared base type and drop the concrete subtype's value properties.
/// </summary>
/// <remarks>
/// <see cref="CanConvert"/> matches the abstract base only (the default for <see cref="JsonConverter{T}"/>, restated for
/// intent): the nested write of the resolved concrete type must not re-enter this converter, or it recurses forever.
/// Reading yields a <see cref="DynamicElement"/> — the wire carries no kind discriminator, so a concrete subtype can't be
/// chosen without the content-type schema; the upsert models this converter serves are request-only, so this path is a
/// faithful fallback rather than a hot path.
/// </remarks>
internal sealed class BaseElementJsonConverter : JsonConverter<BaseElement>
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(BaseElement);

    public override BaseElement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<DynamicElement>(ref reader, options)!;

    public override void Write(Utf8JsonWriter writer, BaseElement value, JsonSerializerOptions options)
        => JsonSerializer.Serialize(writer, value, value.GetType(), options);
}
