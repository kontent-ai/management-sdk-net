using System;

namespace Kontent.Ai.Management.Models.EnvironmentValidation;

/// <summary>
/// Async validation task.
/// </summary>
public sealed record AsyncValidationTaskModel
{
    /// <summary>
    /// Gets the id of the task.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the status of the task.
    /// </summary>
    [JsonPropertyName("status")]
    public AsyncValidationTaskStatus Status { get; init; }

    /// <summary>
    /// Gets the validation result of the task.
    /// </summary>
    [JsonPropertyName("validation_result")]
    public AsyncValidationTaskResult ValidationResult { get; init; }
}
