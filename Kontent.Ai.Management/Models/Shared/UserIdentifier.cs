namespace Kontent.Ai.Management.Models.Shared;

/// <summary>
/// Identifier of a user. Construction is factory-only (<see cref="ById"/> / <see cref="ByEmail"/>) so exactly one identifier is ever set, mirroring <see cref="Reference"/>.
/// </summary>
public sealed record UserIdentifier
{
    private UserIdentifier() { }

    /// <summary>
    /// User ID; <c>null</c> unless this identifier was created by ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; private init; }

    /// <summary>
    /// User email; <c>null</c> unless this identifier was created by email.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; private init; }

    /// <summary>
    /// Creates the identifier by user ID.
    /// </summary>
    /// <param name="id">The user ID.</param>
    public static UserIdentifier ById(string id) => new() { Id = id };

    /// <summary>
    /// Creates the identifier by email.
    /// </summary>
    /// <param name="email">The user email.</param>
    public static UserIdentifier ByEmail(string email) => new() { Email = email };
}
