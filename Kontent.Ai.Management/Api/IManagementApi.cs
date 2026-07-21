namespace Kontent.Ai.Management.Api;

/// <summary>
/// Refit definition of the Kontent.ai Management API v2.
/// </summary>
/// <remarks>
/// Declared as a partial interface split into one <c>IManagementApi.{Domain}.cs</c> file per domain (assets, content
/// items, spaces, …), mirroring the public <c>IManagementClient</c> surface. Refit produces the implementation at
/// runtime from the attributes on its members; the SDK's public client wrapper is the only intended consumer.
/// </remarks>
internal partial interface IManagementApi
{
}
