using System.Reflection;
using Kontent.Ai.Management.Modules.ActionInvoker;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kontent.Ai.Management.Api;

/// <summary>
/// Refit content serializer that reuses the legacy <see cref="ActionInvoker"/>'s Newtonsoft settings: distinct converter
/// sets for the request body (decimal normalization, string enums, omit nulls) and the response body (dynamic-object
/// materialization). Transitional — it is replaced by a System.Text.Json serializer once the models are regenerated.
/// </summary>
internal sealed class ManagementApiContentSerializer : IHttpContentSerializer
{
    private static readonly NewtonsoftJsonContentSerializer RequestSerializer = new(new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        Converters = { new DecimalObjectConverter(), new StringEnumConverter() },
    });

    private static readonly NewtonsoftJsonContentSerializer ResponseSerializer = new(new JsonSerializerSettings
    {
        Converters = { new DynamicObjectJsonConverter() },
    });

    public HttpContent ToHttpContent<T>(T item) => RequestSerializer.ToHttpContent(item);

    public Task<T?> FromHttpContentAsync<T>(HttpContent content, CancellationToken cancellationToken = default)
        => ResponseSerializer.FromHttpContentAsync<T>(content, cancellationToken);

    public string? GetFieldNameForProperty(PropertyInfo propertyInfo) => RequestSerializer.GetFieldNameForProperty(propertyInfo);
}
