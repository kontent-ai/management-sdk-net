using FluentAssertions;
using Kontent.Ai.Management.Models.Roles;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Xunit;

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

        var response = await client.ListEnvironmentRolesAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<EnvironmentRolesModel>(ProjectRoles));
    }

    [Fact]
    public async Task GetEnvironmentRoleAsync_ById_GetsEnvironmentRole()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/roles/{identifier.Id}")
            .Respond("application/json", ProjectRole);

        var response = await client.GetEnvironmentRoleAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<EnvironmentRoleModel>(ProjectRole));
    }

    [Fact]
    public async Task GetEnvironmentRoleAsync_ByCodename_GetsEnvironmentRole()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("codename");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/roles/codename/{identifier.Codename}")
            .Respond("application/json", ProjectRole);

        var response = await client.GetEnvironmentRoleAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<EnvironmentRoleModel>(ProjectRole));
    }

    [Theory]
    [MemberData(nameof(InvalidIdentifiers))]
    public async Task GetEnvironmentRoleAsync_InvalidIdentifier_Throws(Reference? identifier)
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetEnvironmentRoleAsync(identifier!)).Should().ThrowAsync<Exception>();
    }
}
