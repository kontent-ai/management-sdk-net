using Kontent.Ai.Management.Models.EnvironmentValidation;
using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

internal sealed class AsyncValidationTaskIssueJsonConverter : PolymorphicJsonConverter<AsyncValidationTaskIssueModel, AsyncValidationTaskIssueType>
{
    protected override string DiscriminatorPropertyName => "issue_type";

    protected override Type ResolveType(AsyncValidationTaskIssueType discriminator) => discriminator switch
    {
        AsyncValidationTaskIssueType.VariantIssue => typeof(AsyncValidationTaskVariantIssueModel),
        AsyncValidationTaskIssueType.TypeIssue => typeof(AsyncValidationTaskTypeIssueModel),
        _ => throw new JsonException($"No async validation task issue subtype for '{discriminator}'."),
    };
}
