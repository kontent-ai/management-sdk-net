using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

/// <summary>
/// Deserializes <see cref="UserIdentifier"/> via its factory methods (<c>ById</c> / <c>ByEmail</c>)
/// — System.Text.Json cannot otherwise construct it (private parameterless constructor + private
/// setters, same factory-only invariant as <see cref="Reference"/>).
/// </summary>
internal sealed class UserIdentifierJsonConverter : JsonConverter<UserIdentifier>
{
    public override UserIdentifier Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        if (root.TryGetProperty("id", out var id) && id.ValueKind == JsonValueKind.String)
        {
            return UserIdentifier.ById(id.GetString()!);
        }

        if (root.TryGetProperty("email", out var email) && email.ValueKind == JsonValueKind.String)
        {
            return UserIdentifier.ByEmail(email.GetString()!);
        }

        throw new JsonException("UserIdentifier object must contain 'id' or 'email'.");
    }

    public override void Write(Utf8JsonWriter writer, UserIdentifier value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (value.Id is { } id)
        {
            writer.WriteString("id", id);
        }
        else if (value.Email is { } email)
        {
            writer.WriteString("email", email);
        }
        writer.WriteEndObject();
    }
}
