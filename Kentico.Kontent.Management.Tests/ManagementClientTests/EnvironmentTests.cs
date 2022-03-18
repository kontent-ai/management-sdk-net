using FluentAssertions;
using Kentico.Kontent.Management.Models.Environments;
using Kentico.Kontent.Management.Models.Environments.Patch;
using Kentico.Kontent.Management.Tests.Base;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    public class EnvironmentTests : IClassFixture<FileSystemFixture>
    {
        private readonly FileSystemFixture _fileSystemFixture;

        public EnvironmentTests(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
            _fileSystemFixture.SetSubFolder("Environment");
        }

        [Fact]
        public async void CloneEnvironment_RequestModelIsNull_ThrowsException()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("ClonedEnvironment.json");

            await client.Invoking(x => x.CloneEnvironmentAsync(null)).Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async void CloneEnvironment_ReturnsNewEnvironment()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("ClonedEnvironment.json");
            var expectedResponse = _fileSystemFixture.GetExpectedResponse<EnvironmentClonedModel>("ClonedEnvironment.json");

            var cloneEnvironmentModel = new EnvironmentCloneModel
            {
                Name = "New environment",
                RolesToActivate = new[] { Guid.Parse("2ea66788-d3b8-5ff5-b37e-258502e4fd5d") }
            };

            var response = await client.CloneEnvironmentAsync(cloneEnvironmentModel);
            response.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async void GetEnvironmentCloningState_ReturnsCloningState()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("EnvironmentCloningState.json");
            var expectedResponse = _fileSystemFixture.GetExpectedResponse<EnvironmentCloningStateModel>("EnvironmentCloningState.json");

            var response = await client.GetEnvironmentCloningStateAsync();
            response.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async void MarkEnvironmentAsProduction_RequestModelIsNull_ThrowsException()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("ClonedEnvironment.json");

            await client.Invoking(x => x.MarkEnvironmentAsProductionAsync(null)).Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async void MarkEnvironmentAsProduction_MarkEnvironmentAsProduction()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            var markAsProductionModel = new MarkAsProductionModel
            {
                EnableWebhooks = true
            };

            Func<Task> markEnvironmentAsProduction = async () => await client.MarkEnvironmentAsProductionAsync(markAsProductionModel);

            await markEnvironmentAsProduction.Should().NotThrowAsync();
        }

        [Fact]
        public async void DeleteEnvironment_DeletesEnvironment()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            Func<Task> deleteEnvironment = async () => await client.DeleteEnvironmentAsync();

            await deleteEnvironment.Should().NotThrowAsync();
        }

        [Fact]
        public async void ModifyEnvironment_RequestModelIsNull_ThrowsException()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("ClonedEnvironment.json");

            await client.Invoking(x => x.ModifyEnvironmentAsync(null)).Should().ThrowExactlyAsync<ArgumentNullException>();
        }
        [Fact]
        public async void ModifyEnvironment_Rename_RenamesEnvironment()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Environment.json");

            var expectedResponse = _fileSystemFixture.GetExpectedResponse<EnvironmentModel>("Environment.json");

            var changes = new[] { new EnvironmentRenamePatchModel
            {
                Value = "New name"
            }};

            var response = await client.ModifyEnvironmentAsync(changes);

            response.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
