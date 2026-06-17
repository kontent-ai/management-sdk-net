using AwesomeAssertions;

namespace Kontent.Ai.Management.Tests.Shared;

public class ReferenceTests
{
    [Fact]
    public void ById_SetsOnlyId()
    {
        var id = Guid.NewGuid();
        var reference = Reference.ById(id);

        reference.Id.Should().Be(id);
        reference.Codename.Should().BeNull();
        reference.ExternalId.Should().BeNull();
    }

    [Fact]
    public void ByCodename_SetsOnlyCodename()
    {
        var reference = Reference.ByCodename("on_roasts");

        reference.Codename.Should().Be("on_roasts");
        reference.Id.Should().BeNull();
        reference.ExternalId.Should().BeNull();
    }

    [Fact]
    public void ByExternalId_SetsOnlyExternalId()
    {
        var reference = Reference.ByExternalId("ext-123");

        reference.ExternalId.Should().Be("ext-123");
        reference.Id.Should().BeNull();
        reference.Codename.Should().BeNull();
    }

    [Fact]
    public void ByDefaultId_IsZeroGuidById()
    {
        var reference = Reference.ByDefaultId();

        reference.Id.Should().Be(Guid.Empty);
        reference.Codename.Should().BeNull();
        reference.ExternalId.Should().BeNull();
    }

    [Fact]
    public void ByDefaultCodename_IsDefaultCodename()
    {
        var reference = Reference.ByDefaultCodename();

        reference.Codename.Should().Be("default");
        reference.Id.Should().BeNull();
        reference.ExternalId.Should().BeNull();
    }

    [Fact]
    public void RecordEquality_HoldsByValue()
    {
        Reference.ById(Guid.Empty).Should().Be(Reference.ByDefaultId());
        Reference.ByCodename("default").Should().Be(Reference.ByDefaultCodename());
        Reference.ByCodename("a").Should().NotBe(Reference.ByCodename("b"));
        // Same identifier value in different slots must not be equal.
        Reference.ByCodename("default").Should().NotBe(Reference.ByExternalId("default"));
    }
}
