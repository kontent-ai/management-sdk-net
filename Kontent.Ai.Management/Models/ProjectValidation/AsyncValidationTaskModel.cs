using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.ProjectValidation;

/// <summary>
/// Async validation task.
/// </summary>
public sealed class AsyncValidationTaskModel
{
    /// <summary>
    /// Gets or sets the id of the task.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the status of the task.
    /// </summary>
    [JsonProperty("status")]
    public AsyncValidationTaskStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the validation result of the task.
    /// </summary>
    [JsonProperty("validation_result")]
    public AsyncValidationTaskResult ValidationResult { get; set; }
}
