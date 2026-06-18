using System.Diagnostics.CodeAnalysis;

namespace Kontent.Ai.Management;

/// <summary>
/// Opt-in conveniences over <see cref="IManagementResult"/> for callers who would rather throw on failure or use the
/// Try pattern than branch on <see cref="IManagementResult.IsSuccess"/> at every call site.
/// </summary>
public static class ManagementResultExtensions
{
    /// <summary>
    /// Returns the result on success; throws <see cref="ManagementException"/> on failure. Handy for scripts and
    /// simple apps that prefer exceptions over result branching.
    /// </summary>
    public static IManagementResult EnsureSuccess(this IManagementResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        return result.IsSuccess ? result : throw new ManagementException(result);
    }

    /// <summary>
    /// Returns the result value on success; throws <see cref="ManagementException"/> on failure.
    /// </summary>
    public static T EnsureSuccess<T>(this IManagementResult<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);
        return result.IsSuccess ? result.Value : throw new ManagementException(result);
    }

    /// <summary>
    /// Gets the result value when the operation succeeded. Returns <c>true</c> and sets <paramref name="value"/> on
    /// success; returns <c>false</c> otherwise.
    /// </summary>
    public static bool TryGetValue<T>(this IManagementResult<T> result, [MaybeNullWhen(false)] out T value)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.IsSuccess)
        {
            value = result.Value;
            return true;
        }

        value = default;
        return false;
    }
}
