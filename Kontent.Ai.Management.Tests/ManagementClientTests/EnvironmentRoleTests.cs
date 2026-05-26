using AwesomeAssertions;
using Kontent.Ai.Management.Models.Roles;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class EnvironmentRoleTests
{
    private static string ProjectRole => Fixture("ProjectRole.json");
    private static string ProjectRoles => Fixture("ProjectRoles.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "ProjectRole", name));

    public static TheoryData<Reference?> InvalidIdentifiers =>
    [
        Reference.ByExternalId("externalId"),
        null,
    ];

    [Fact]
    public async Task ListEnvironmentRolesAsync_ListsEnvironmentRoles()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/roles")
            .Respond("application/json", ProjectRoles);

        var result = await client.ListEnvironmentRolesAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<EnvironmentRolesModel>(ProjectRoles, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetEnvironmentRoleAsync_ById_GetsEnvironmentRole()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/roles/{identifier.Id}")
            .Respond("application/json", ProjectRole);

        var result = await client.GetEnvironmentRoleAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<EnvironmentRoleModel>(ProjectRole, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetEnvironmentRoleAsync_ByCodename_GetsEnvironmentRole()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("codename");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/roles/codename/{identifier.Codename}")
            .Respond("application/json", ProjectRole);

        var result = await client.GetEnvironmentRoleAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<EnvironmentRoleModel>(ProjectRole, SharedTestJsonOptions.Default));
    }

    [Theory]
    [MemberData(nameof(InvalidIdentifiers))]
    public async Task GetEnvironmentRoleAsync_InvalidIdentifier_Throws(Reference? identifier)
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetEnvironmentRoleAsync(identifier!)).Should().ThrowAsync<Exception>();
    }
}
