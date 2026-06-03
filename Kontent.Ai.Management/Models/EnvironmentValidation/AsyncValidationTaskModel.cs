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
    public required Guid Id { get; init; }

    /// <summary>
    /// Gets the status of the task.
    /// </summary>
    [JsonPropertyName("status")]
    public required AsyncValidationTaskStatus Status { get; init; }

    /// <summary>
    /// Gets the validation result of the task.
    /// </summary>
    [JsonPropertyName("validation_result")]
    public required AsyncValidationTaskResult ValidationResult { get; init; }
}
