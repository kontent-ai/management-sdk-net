using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

/// <summary>
/// Deserializes an abstract model to the concrete subtype selected by a <typeparamref name="TDiscriminator"/>
/// enum discriminator property, and serializes the runtime concrete type. The wire token is resolved against the
/// enum's <see cref="EnumMemberAttribute"/> values, so each converter only maps enum members to subtypes.
/// </summary>
/// <remarks>
/// <see cref="CanConvert"/> matches the abstract base type only. That is load-bearing: the nested
/// (de)serialization of the resolved concrete type must not re-enter this converter, otherwise it
/// recurses forever.
/// </remarks>
internal abstract class PolymorphicJsonConverter<TBase, TDiscriminator> : JsonConverter<TBase>
    where TBase : class
    where TDiscriminator : struct, Enum
{
    protected abstract string DiscriminatorPropertyName { get; }

    protected abstract Type ResolveType(TDiscriminator discriminator);

    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(TBase);

    public override TBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        if (!root.TryGetProperty(DiscriminatorPropertyName, out var discriminator) || discriminator.ValueKind != JsonValueKind.String)
        {
            throw new JsonException($"Object does not contain a string '{DiscriminatorPropertyName}' discriminator property.");
        }

        var token = discriminator.GetString()!;
        if (!EnumWire.TryFromValue<TDiscriminator>(token, out var key))
        {
            throw new JsonException($"Unknown '{DiscriminatorPropertyName}' discriminator value '{token}'.");
        }

        var concreteType = ResolveType(key);
        return (TBase)root.Deserialize(concreteType, options)!;
    }

    public override void Write(Utf8JsonWriter writer, TBase value, JsonSerializerOptions options)
        => JsonSerializer.Serialize(writer, value, value.GetType(), options);
}
