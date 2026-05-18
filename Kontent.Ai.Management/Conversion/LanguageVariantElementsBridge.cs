using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kontent.Ai.Management.Conversion;

/// <summary>
/// Transitional adapter between the Newtonsoft-deserialized <c>elements</c> envelope array carried by
/// <see cref="Models.LanguageVariants.LanguageVariantModel"/> / <see cref="Models.LanguageVariants.LanguageVariantUpsertModel"/>
/// (<c>IEnumerable&lt;dynamic&gt;</c> of <see cref="JToken"/>) and the <see cref="System.Text.Json"/>-based
/// <see cref="ContentItemEnvelopeConverter"/>.
/// </summary>
/// <remarks>
/// This whole type exists only because the Refit transport still serializes with Newtonsoft
/// (<c>ManagementApiContentSerializer</c>) while the generated-record conversion is System.Text.Json. It round-trips
/// the elements array through a JSON string — the one, deliberately localized, Newtonsoft touchpoint on the
/// strongly-typed variant path. <b>Delete this in its entirety when the global serializer flips to STJ</b> (the
/// model-regeneration wave): at that point the converter consumes the wire payload directly and the bridge has no
/// reason to exist.
/// </remarks>
internal static class LanguageVariantElementsBridge
{
    public static string ElementsToJson(IEnumerable<object> elements)
        => JsonConvert.SerializeObject(elements);

    public static IEnumerable<object> JsonToElements(string envelopesJson)
        => JArray.Parse(envelopesJson);
}
