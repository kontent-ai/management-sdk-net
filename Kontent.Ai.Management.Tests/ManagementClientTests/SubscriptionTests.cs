using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Subscription;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using Xunit;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class SubscriptionTests
{
    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "Subscription", name));

    private static List<T> ConcatPages<T>(params string[] pages)
        => pages
            .SelectMany(p => JsonSerializer.Deserialize<List<T>>(JsonNode.Parse(p)!.AsObject().First().Value!.ToString(), SharedTestJsonOptions.Default)!)
            .ToList();

    [Fact]
    public async Task EnumerateSubscriptionProjectPagesAsync_PagesThroughAllProjects()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("ProjectsPage1.json");
        var page2 = Fixture("ProjectsPage2.json");
        var page3 = Fixture("ProjectsPage3.json");
        var url = $"{MockClientFactory.SubscriptionBaseUrl}/projects";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var projects = new List<SubscriptionProjectModel>();
        await foreach (var page in client.EnumerateSubscriptionProjectPagesAsync())
        {
            page.IsSuccess.Should().BeTrue();
            projects.AddRange(page.Value);
        }

        mock.VerifyNoOutstandingExpectation();
        projects.Should().BeEquivalentTo(ConcatPages<SubscriptionProjectModel>(page1, page2, page3));
    }

    [Fact]
    public async Task EnumerateSubscriptionUserPagesAsync_PagesThroughAllUsers()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("UsersPage1.json");
        var page2 = Fixture("UsersPage2.json");
        var page3 = Fixture("UsersPage3.json");
        var url = $"{MockClientFactory.SubscriptionBaseUrl}/users";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var users = new List<SubscriptionUserModel>();
        await foreach (var page in client.EnumerateSubscriptionUserPagesAsync())
        {
            page.IsSuccess.Should().BeTrue();
            users.AddRange(page.Value);
        }

        mock.VerifyNoOutstandingExpectation();
        users.Should().BeEquivalentTo(ConcatPages<SubscriptionUserModel>(page1, page2, page3));
    }

    [Fact]
    public async Task GetSubscriptionUserAsync_ById_GetsSubscriptionUser()
    {
        var (client, mock) = MockClientFactory.Create();
        var user = Fixture("User.json");
        var identifier = UserIdentifier.ById("some_id");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.SubscriptionBaseUrl}/users/{identifier.Id}")
            .Respond("application/json", user);

        var result = await client.GetSubscriptionUserAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<SubscriptionUserModel>(user, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetSubscriptionUserAsync_ByEmail_GetsSubscriptionUser()
    {
        var (client, mock) = MockClientFactory.Create();
        var user = Fixture("User.json");
        var identifier = UserIdentifier.ByEmail("some_email");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.SubscriptionBaseUrl}/users/email/{identifier.Email}")
            .Respond("application/json", user);

        var result = await client.GetSubscriptionUserAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<SubscriptionUserModel>(user, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetSubscriptionUserAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetSubscriptionUserAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ActivateSubscriptionUserAsync_ById_ActivatesUser()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = UserIdentifier.ById("some_id");
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.SubscriptionBaseUrl}/users/{identifier.Id}/activate")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.ActivateSubscriptionUserAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ActivateSubscriptionUserAsync_ByEmail_ActivatesUser()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = UserIdentifier.ByEmail("some_email");
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.SubscriptionBaseUrl}/users/email/{identifier.Email}/activate")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.ActivateSubscriptionUserAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ActivateSubscriptionUserAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ActivateSubscriptionUserAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeactivateSubscriptionUserAsync_ById_ActivatesUser()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = UserIdentifier.ById("some_id");
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.SubscriptionBaseUrl}/users/{identifier.Id}/deactivate")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.DeactivateSubscriptionUserAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeactivateSubscriptionUserAsync_ByEmail_ActivatesUser()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = UserIdentifier.ByEmail("some_email");
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.SubscriptionBaseUrl}/users/email/{identifier.Email}/deactivate")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.DeactivateSubscriptionUserAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeactivateSubscriptionUserAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.DeactivateSubscriptionUserAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }
}
