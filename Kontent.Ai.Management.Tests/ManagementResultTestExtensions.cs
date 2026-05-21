namespace Kontent.Ai.Management.Tests;

/// <summary>Test-only conveniences over <see cref="IManagementResult"/>.</summary>
internal static class ManagementResultTestExtensions
{
    /// <summary>The validation errors a result carries, or an empty list when it carries no error.</summary>
    public static IReadOnlyList<ValidationError> ValidationErrors(this IManagementResult result)
        => result.Error?.ValidationErrors ?? [];
}
