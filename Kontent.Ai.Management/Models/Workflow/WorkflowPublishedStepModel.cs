using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// Represents the Published workflow step response model. If a variant is in this step, it indicated that it has been
/// published and is (soon to be) available as Published content in Delivery APIs. Such variants are read-only.
/// </summary>
/// <remarks>
/// All <c>Id</c>, <c>Name</c>, and <c>Codename</c> properties are predefined by the system and cannot be changed.
/// </remarks>
public sealed record WorkflowPublishedStepModel
{
    /// <summary>
    /// Gets the workflow step's internal ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the workflow step's name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Gets the workflow step's codename.
    /// </summary>
    [JsonPropertyName("codename")]
    public string Codename { get; init; }

    /// <summary>
    /// Gets the roles which can unpublish the item's variant.
    /// </summary>
    [JsonPropertyName("unpublish_role_ids")]
    public IReadOnlyCollection<Guid> UnpublishRoleIds { get; init; }

    /// <summary>
    /// Gets the roles which can create new version from published variant.
    /// </summary>
    [JsonPropertyName("create_new_version_role_ids")]
    public IReadOnlyCollection<Guid> CreateNewVersionRoleIds { get; init; }
}