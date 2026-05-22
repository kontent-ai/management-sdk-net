using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

/// <summary>
/// Deserializes an abstract model to the concrete subtype selected by a string discriminator
/// property, and serializes the runtime concrete type.
/// </summary>
/// <remarks>
/// <see cref="CanConvert"/> matches the abstract base type only. That is load-bearing: the nested
/// (de)serialization of the resolved concrete type must not re-enter this converter, otherwise it
/// recurses forever. It is the System.Text.Json equivalent of the legacy Newtonsoft
/// <c>BaseSpecifiedConcreteClassConverter</c> contract resolver.
/// </remarks>
internal abstract class PolymorphicJsonConverter<TBase> : JsonConverter<TBase> where TBase : class
{
    protected abstract string DiscriminatorPropertyName { get; }

    protected abstract Type ResolveType(string discriminator);

    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(TBase);

    public override TBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        if (!root.TryGetProperty(DiscriminatorPropertyName, out var discriminator) || discriminator.ValueKind != JsonValueKind.String)
        {
            throw new JsonException($"Object does not contain a string '{DiscriminatorPropertyName}' discriminator property.");
        }

        var concreteType = ResolveType(discriminator.GetString()!);
        return (TBase)root.Deserialize(concreteType, options)!;
    }

    public override void Write(Utf8JsonWriter writer, TBase value, JsonSerializerOptions options)
        => JsonSerializer.Serialize(writer, value, value.GetType(), options);
}
