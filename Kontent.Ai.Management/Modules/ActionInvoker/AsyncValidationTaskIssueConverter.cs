using Kontent.Ai.Management.Models.EnvironmentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;

namespace Kontent.Ai.Management.Modules.ActionInvoker;

internal class AsyncValidationTaskIssueConverter : JsonConverter
{
    private static readonly JsonSerializerSettings _specifiedSubclassConversion = new()
    {
        ContractResolver = new BaseSpecifiedConcreteClassConverter()
    };

    public override bool CanConvert(Type objectType) => (objectType == typeof(AsyncValidationTaskIssueModel));

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);
        var type = jObject["issue_type"]?.ToObject<AsyncValidationTaskIssueType>()
            ?? throw new ArgumentException("Object does not contain 'issue_type' property or it is null.", nameof(reader));

        return type switch
        {
            AsyncValidationTaskIssueType.VariantIssue => JsonConvert.DeserializeObject<AsyncValidationTaskVariantIssueModel>(
                jObject.ToString(), _specifiedSubclassConversion),
            AsyncValidationTaskIssueType.TypeIssue => JsonConvert.DeserializeObject<AsyncValidationTaskTypeIssueModel>(
                jObject.ToString(), _specifiedSubclassConversion),
            _ => throw new InvalidEnumArgumentException(nameof(type), Convert.ToInt32(type), typeof(AsyncValidationTaskIssueModel))
        };
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        => throw new NotImplementedException(); // won't be called because CanWrite returns false
}
