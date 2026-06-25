using Kontent.Ai.Management.Models.AssetFolders.Patch;
using Kontent.Ai.Management.Models.Collections.Patch;
using Kontent.Ai.Management.Models.ContentModel.Patch;
using Kontent.Ai.Management.Models.CustomApps.Patch;
using Kontent.Ai.Management.Models.Environments.Patch;
using Kontent.Ai.Management.Models.TaxonomyGroups.Patch;
using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

/// <summary>
/// Serializes PATCH operation models by their runtime concrete type. System.Text.Json serializes the declared
/// type, which would drop every derived property, so writing the concrete type has to be made explicit here.
/// </summary>
/// <remarks>
/// <see cref="CanConvert"/> matches the abstract bases only. The nested serialization of the runtime
/// concrete type therefore does not re-enter this factory and falls through to default reflection
/// serialization — same recursion guard as <see cref="PolymorphicJsonConverter{TBase, TDiscriminator}"/>. These
/// models are request-only; deserialization is never exercised.
/// </remarks>
internal sealed class PatchOperationJsonConverterFactory : JsonConverterFactory
{
    private static readonly HashSet<Type> OperationBases =
    [
        typeof(AssetFolderOperationBaseModel),
        typeof(CollectionOperationBaseModel),
        typeof(CustomAppOperationBaseModel),
        typeof(EnvironmentOperationBaseModel),
        typeof(TaxonomyGroupOperationBaseModel),
        typeof(ContentModelOperationBaseModel),
    ];

    public override bool CanConvert(Type typeToConvert) => OperationBases.Contains(typeToConvert);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        => (JsonConverter)Activator.CreateInstance(typeof(PatchOperationWriteConverter<>).MakeGenericType(typeToConvert))!;

    private sealed class PatchOperationWriteConverter<T> : JsonConverter<T> where T : class
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => throw new NotSupportedException($"{typeof(T).Name} is a request-only PATCH operation model and is never deserialized.");

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            => JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
