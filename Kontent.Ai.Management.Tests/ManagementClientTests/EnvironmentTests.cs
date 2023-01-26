using FluentAssertions;
using Kontent.Ai.Management.Models.Environments;
using Kontent.Ai.Management.Models.Environments.Patch;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Net.Http;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class EnvironmentTests : IClassFixture<FileSystemFixture>
{
    private readonly Scenario _scenario;

    public EnvironmentTests()
    {
        _scenario = new Scenario(folder: "Environment");
    }

    [Fact]
    public async void CloneEnvironmentAsync_ReturnsNewEnvironment()
    {
        var client = _scenario
            .WithResponses("ClonedEnvironment.json")
            .CreateManagementClient();

        var clone = new EnvironmentCloneModel
        {
            Name= "name",
            RolesToActivate = new[] { Guid.NewGuid() }
        };

        var response = await client.CloneEnvironmentAsync(clone);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .RequestPayload(clone)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/clone-environment")
            .Validate();
    }

    [Fact]
    public async void CloneEnvironmentAsync_RequestModelIsNull_ThrowsException()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.CloneEnvironmentAsync(null)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }


    [Fact]
    public async void GetEnvironmentCloningStateAsync_ReturnsCloningState()
    {
        var client = _scenario
            .WithResponses("ClonedEnvironment.json")
            .CreateManagementClient();

        var response = await client.GetEnvironmentCloningStateAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/environment-cloning-state")
            .Validate();
    }

    [Fact] 
    public async void MarkEnvironmentAsProduction_MarkEnvironmentAsProduction()
    {
        var client = _scenario.CreateManagementClient();

        var markAsProduction = new MarkAsProductionModel
        {
            EnableWebhooks = true
        };

        await client.MarkEnvironmentAsProductionAsync(markAsProduction);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(markAsProduction)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/mark-environment-as-production")
            .Validate();
    }


    [Fact]
    public async void MarkEnvironmentAsProductionAsync_RequestModelIsNull_ThrowsException()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.MarkEnvironmentAsProductionAsync(null)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async void DeleteEnvironmentAsync_DeletesEnvironment()
    {
        var client = _scenario.CreateManagementClient();

        await client.DeleteEnvironmentAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Delete)
            .Url($"{Endpoint}/projects/{PROJECT_ID}")
            .Validate();
    }

    [Fact]
    public async void ModifyEnvironmentAsync_Rename_RenamesEnvironment()
    {
        var client = _scenario.CreateManagementClient();

        var changes = new[] {
            new EnvironmentRenamePatchModel
            {
                Value= "newName"
            }
        };

        await client.ModifyEnvironmentAsync(changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Url($"{Endpoint}/projects/{PROJECT_ID}")
            .Validate();
    }

    [Fact]
    public async void ModifyEnvironmentAsync_RequestModelIsNull_ThrowsException()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.ModifyEnvironmentAsync(null)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}
