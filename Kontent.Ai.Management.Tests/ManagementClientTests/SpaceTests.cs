using FluentAssertions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Spaces;
using Kontent.Ai.Management.Models.Spaces.Patch;
using Kontent.Ai.Management.Tests.Base;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class SpaceTests
{
    private static string SpacesUrl => $"{MockClientFactory.BaseUrl}/spaces";

    private static string Space => Fixture("Space.json");
    private static string Spaces => Fixture("Spaces.json");
    private static string ModifySpaceReplace => Fixture("ModifySpace_Replace_ModifiesSpace.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "Space", name));

    [Fact]
    public async Task CreateSpace_CreatesSpace()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonConvert.DeserializeObject<SpaceModel>(Space)!;
        var createModel = new SpaceCreateModel
        {
            Codename = expected.Codename,
            Name = expected.Name,
            WebSpotlightRootItem = expected.WebSpotlightRootItem,
            Collections = expected.Collections
        };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Post, SpacesUrl)
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", Space);

        var response = await client.CreateSpaceAsync(createModel);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<SpaceModel>(Space));
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<SpaceCreateModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<SpaceCreateModel>(JsonConvert.SerializeObject(createModel)));
    }

    [Fact]
    public async Task CreateSpace_ModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.CreateSpaceAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ListSpaces_ListsSpaces()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, SpacesUrl)
            .Respond("application/json", Spaces);

        var response = await client.ListSpacesAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<IEnumerable<SpaceModel>>(Spaces));
    }

    [Fact]
    public async Task GetSpace_ById_GetsSpace()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Get, $"{SpacesUrl}/{identifier.Id}")
            .Respond("application/json", Space);

        var response = await client.GetSpaceAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<SpaceModel>(Space));
    }

    [Fact]
    public async Task GetSpace_ByCodename_GetsSpace()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("space_1");
        mock.Expect(HttpMethod.Get, $"{SpacesUrl}/codename/{identifier.Codename}")
            .Respond("application/json", Space);

        var response = await client.GetSpaceAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<SpaceModel>(Space));
    }

    [Fact]
    public async Task GetSpace_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetSpaceAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifySpace_Replace_ModifiesSpace()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        var changes = new SpaceOperationReplaceModel[]
        {
            new() { PropertyName = PropertyName.Name, Value = "New space name" },
            new() { PropertyName = PropertyName.Codename, Value = "new_space_codename" },
            new() { PropertyName = PropertyName.WebSpotlightRootItem, Value = identifier },
            new() { PropertyName = PropertyName.Collections, Value = new[] {
                    Reference.ByCodename("collection_codename"),
                    Reference.ById(Guid.NewGuid()) }
            }
        };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Patch, $"{SpacesUrl}/{identifier.Id}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ModifySpaceReplace);

        var response = await client.ModifySpaceAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<SpaceModel>(ModifySpaceReplace));
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<SpaceOperationReplaceModel[]>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<SpaceOperationReplaceModel[]>(JsonConvert.SerializeObject(changes)));
    }

    [Fact]
    public async Task ModifySpace_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var changes = new SpaceOperationReplaceModel[]
        {
            new() { PropertyName = PropertyName.Name, Value = "New space name" }
        };

        await client.Invoking(x => x.ModifySpaceAsync(null!, changes)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifySpace_ChangesAreNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());

        await client.Invoking(x => x.ModifySpaceAsync(identifier, null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteSpace_ById_DeletesSpace()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Delete, $"{SpacesUrl}/{identifier.Id}")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.DeleteSpaceAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeleteSpace_ByCodename_DeletesSpace()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("space_1");
        mock.Expect(HttpMethod.Delete, $"{SpacesUrl}/codename/{identifier.Codename}")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.DeleteSpaceAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeleteSpace_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.DeleteSpaceAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }
}
