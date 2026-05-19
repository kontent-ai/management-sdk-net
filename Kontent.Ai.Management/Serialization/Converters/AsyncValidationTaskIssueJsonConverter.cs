using System.Text.Json;
using Kontent.Ai.Management.Models.EnvironmentValidation;

namespace Kontent.Ai.Management.Serialization.Converters;

internal sealed class AsyncValidationTaskIssueJsonConverter : PolymorphicJsonConverter<AsyncValidationTaskIssueModel>
{
    protected override string DiscriminatorPropertyName => "issue_type";

    protected override Type ResolveType(string discriminator) => discriminator switch
    {
        "variant_issue" => typeof(AsyncValidationTaskVariantIssueModel),
        "type_issue" => typeof(AsyncValidationTaskTypeIssueModel),
        _ => throw new JsonException($"Unknown async validation task issue type '{discriminator}'."),
    };
}
