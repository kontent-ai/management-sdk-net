using Kontent.Ai.Management.Conversion;
using System.Text;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.Conversion;

// String-based conveniences over the converter's writer/element primitives — fixture-driven tests read better
// with JSON strings, but production only ever needs the stream/element paths.
internal static class EnvelopeConverterTestExtensions
{
    public static string WriteEnvelopes<T>(this ContentItemEnvelopeConverter converter, T item) where T : IElementsModel
    {
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream))
        {
            converter.WriteEnvelopes(writer, item);
        }
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    public static T ReadEnvelopes<T>(this ContentItemEnvelopeConverter converter, string envelopesJson) where T : IElementsModel
    {
        using var doc = JsonDocument.Parse(envelopesJson);
        return (T)converter.ReadEnvelopes(doc.RootElement, typeof(T));
    }
}
