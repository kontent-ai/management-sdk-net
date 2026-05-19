using AwesomeAssertions;
using Kontent.Ai.Management.Models.Environments;
using Kontent.Ai.Management.Models.Environments.Patch;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using Xunit;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class EnvironmentTests
{
    private static string ClonedEnvironment => Fixture("ClonedEnvironment.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "Environment", name));

    [Fact]
    public async Task CloneEnvironmentAsync_ReturnsNewEnvironment()
    {
        var (client, mock) = MockClientFactory.Create();
        var clone = new EnvironmentCloneModel
        {
            Name = "name",
            RolesToActivate = new[] { Guid.NewGuid() }
        };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/clone-environment")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ClonedEnvironment);

        var response = await client.CloneEnvironmentAsync(clone);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonSerializer.Deserialize<EnvironmentClonedModel>(ClonedEnvironment, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<EnvironmentCloneModel>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<EnvironmentCloneModel>(JsonSerializer.Serialize(clone, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task CloneEnvironmentAsync_RequestModelIsNull_ThrowsException()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.CloneEnvironmentAsync(null!)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetEnvironmentCloningStateAsync_ReturnsCloningState()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/environment-cloning-state")
            .Respond("application/json", ClonedEnvironment);

        var response = await client.GetEnvironmentCloningStateAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonSerializer.Deserialize<EnvironmentCloningStateModel>(ClonedEnvironment, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task MarkEnvironmentAsProduction_MarkEnvironmentAsProduction()
    {
        var (client, mock) = MockClientFactory.Create();
        var markAsProduction = new MarkAsProductionModel
        {
            EnableWebhooks = true
        };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/mark-environment-as-production")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond(System.Net.HttpStatusCode.OK);

        await client.MarkEnvironmentAsProductionAsync(markAsProduction);

        mock.VerifyNoOutstandingExpectation();
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<MarkAsProductionModel>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<MarkAsProductionModel>(JsonSerializer.Serialize(markAsProduction, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task MarkEnvironmentAsProductionAsync_RequestModelIsNull_ThrowsException()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.MarkEnvironmentAsProductionAsync(null!)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteEnvironmentAsync_DeletesEnvironment()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.DeleteEnvironmentAsync();

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task ModifyEnvironmentAsync_Rename_RenamesEnvironment()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = new[] {
            new EnvironmentRenamePatchModel
            {
                Value = "newName"
            }
        };

        string? capturedBody = null;
        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond(System.Net.HttpStatusCode.OK);

        await client.ModifyEnvironmentAsync(changes);

        mock.VerifyNoOutstandingExpectation();
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<EnvironmentRenamePatchModel[]>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<EnvironmentRenamePatchModel[]>(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default), opt => opt.WithStrictOrdering());
    }

    [Fact]
    public async Task ModifyEnvironmentAsync_RequestModelIsNull_ThrowsException()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ModifyEnvironmentAsync(null!)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}
