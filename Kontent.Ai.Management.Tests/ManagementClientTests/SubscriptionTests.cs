using FluentAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class SubscriptionTests
{
    private readonly Scenario _scenario;

    public SubscriptionTests()
    {
        _scenario = new Scenario(folder: "Subscription");
    }

    [Fact]
    public async void ListSubscriptionProjectsAsync_WithContinuation_ListsSubscriptionProjects()
    {
        var client = _scenario
            .WithResponses("ProjectsPage1.json", "ProjectsPage2.json", "ProjectsPage3.json")
            .CreateManagementClient();

        var response = await client.ListSubscriptionProjectsAsync().GetAllAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .ListingResponse(response)
            .Url($"{Endpoint}/subscriptions/{SUBSCRIPTION_ID}/projects")
            .Validate();
    }


    [Fact]
    public async Task ListSubscriptionUsersAsync_WithContinuation_ListsSubscriptionUsers()
    {
        var client = _scenario
            .WithResponses("UsersPage1.json", "UsersPage2.json", "UsersPage3.json")
            .CreateManagementClient();

        var response = await client.ListSubscriptionUsersAsync().GetAllAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .ListingResponse(response)
            .Url($"{Endpoint}/subscriptions/{SUBSCRIPTION_ID}/users")
            .Validate();
    }

    [Fact]
    public async Task GetSubscriptionUserAsync_ById_GetsSubscriptionUser()
    {
        var client = _scenario
            .WithResponses("User.json")
            .CreateManagementClient();

        var identifier = UserIdentifier.ById("some_id");

        var response = await client.GetSubscriptionUserAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/subscriptions/{SUBSCRIPTION_ID}/users/{identifier.Id}")
            .Validate();
    }


    [Fact]
    public async Task GetSubscriptionUserAsync_ByEmail_GetsSubscriptionUser()
    {
        var client = _scenario
            .WithResponses("User.json")
            .CreateManagementClient();

        var identifier = UserIdentifier.ByEmail("some_email");

        var response = await client.GetSubscriptionUserAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/subscriptions/{SUBSCRIPTION_ID}/users/email/{identifier.Email}")
            .Validate();
    }

    [Fact]
    public async void GetSubscriptionUserAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.GetSubscriptionUserAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ActivateSubscriptionUserAsync_ById_ActivatesUser()
    {
        var client = _scenario
            .WithResponses("User.json")
            .CreateManagementClient();

        var identifier = UserIdentifier.ById("some_id");

        await client.ActivateSubscriptionUserAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .Url($"{Endpoint}/subscriptions/{SUBSCRIPTION_ID}/users/{identifier.Id}/activate")
            .Validate();
    }

    [Fact]
    public async void ActivateSubscriptionUserAsync_ByEmail_ActivatesUser()
    {
        var client = _scenario
            .WithResponses("User.json")
            .CreateManagementClient();

        var identifier = UserIdentifier.ByEmail("some_email");

        await client.ActivateSubscriptionUserAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .Url($"{Endpoint}/subscriptions/{SUBSCRIPTION_ID}/users/email/{identifier.Email}/activate")
            .Validate();
    }

    [Fact]
    public async void ActivateSubscriptionUserAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.ActivateSubscriptionUserAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void DeactivateSubscriptionUserAsync_ById_ActivatesUser()
    {
        var client = _scenario
            .WithResponses("User.json")
            .CreateManagementClient();

        var identifier = UserIdentifier.ById("some_id");

        await client.DeactivateSubscriptionUserAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .Url($"{Endpoint}/subscriptions/{SUBSCRIPTION_ID}/users/{identifier.Id}/deactivate")
            .Validate();
    }

    [Fact]
    public async void DeactivateSubscriptionUserAsync_ByEmail_ActivatesUser()
    {
        var client = _scenario
            .WithResponses("User.json")
            .CreateManagementClient();

        var identifier = UserIdentifier.ByEmail("some_email");

        await client.DeactivateSubscriptionUserAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .Url($"{Endpoint}/subscriptions/{SUBSCRIPTION_ID}/users/email/{identifier.Email}/deactivate")
            .Validate();
    }

    [Fact]
    public async void DeactivateSubscriptionUserAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DeactivateSubscriptionUserAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }
}
