using FluentAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Shared;
using System;
using System.Dynamic;
using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.ModelBuilders;

public class ElementReferenceTests
{
    [Fact]
    public void FromDynamic_GuidId_ById()
    {
        var id = Guid.NewGuid();
        dynamic source = new ExpandoObject();
        source.id = id;
        var idReference = Reference.FromDynamic(source);

        Assert.Equal(null, idReference.ExternalId);
        Assert.Equal(id, idReference.Id);
        Assert.Equal(null, idReference.Codename);
    }

    [Fact]
    public void FromDynamic_StringId_ById()
    {
        var id = Guid.NewGuid();
        dynamic source = new ExpandoObject();
        source.id = id.ToString();
        var idReference = Reference.FromDynamic(source);

        Assert.Equal(null, idReference.ExternalId);
        Assert.Equal(id, idReference.Id);
        Assert.Equal(null, idReference.Codename);
    }

    [Fact]
    public void FromDynamic_Codename_ByCodename()
    {
        var codename = "test";
        dynamic source = new ExpandoObject();
        source.codename = codename;
        var codenameReference = Reference.FromDynamic(source);

        Assert.Equal(null, codenameReference.ExternalId);
        Assert.Equal(null, codenameReference.Id);
        Assert.Equal(codename, codenameReference.Codename);
    }

    [Fact]
    public void FromDynamic_ExternalId_ByExternalId()
    {
        var externalId = "external";
        dynamic source = new ExpandoObject();
        source.external_id = externalId;
        var externalIdReference = Reference.FromDynamic(source);

        Assert.Equal(externalId, externalIdReference.ExternalId);
        Assert.Equal(null, externalIdReference.Id);
        Assert.Equal(null, externalIdReference.Codename);
    }

    [Fact]
    public void FromDynamic_NoIdentifier_ThrowException()
    {
        Action action = () =>
            Reference.FromDynamic(new { });

        action.Should()
            .Throw<DataMisalignedException>()
            .WithMessage("Dynamic element reference does not contain any identifier.");
    }

    [Fact]
    public void ToDynamic_ById_Id()
    {
        var id = Guid.NewGuid();
        var idReference = Reference.ById(id).ToDynamic();

        Assert.Equal(id, idReference.id);
        Assert.False(DynamicExtensions.HasProperty(idReference, "external_id"));
        Assert.False(DynamicExtensions.HasProperty(idReference, "codename"));
    }

    [Fact]
    public void ToDynamic_ByCodename_Codename()
    {
        var codename = "test";
        var codenameReference = Reference.ByCodename(codename).ToDynamic();

        Assert.Equal(codename, codenameReference.codename);
        Assert.False(DynamicExtensions.HasProperty(codenameReference, "external_id"));
        Assert.False(DynamicExtensions.HasProperty(codenameReference, "id"));
    }

    [Fact]
    public void ToDynamic_ByExternalId_ExternalId()
    {
        var externalId = "external";
        var externalIdReference = Reference.ByExternalId(externalId).ToDynamic();

        Assert.Equal(externalId, externalIdReference.external_id);
        Assert.False(DynamicExtensions.HasProperty(externalIdReference, "id"));
        Assert.False(DynamicExtensions.HasProperty(externalIdReference, "codename"));
    }

    [Fact]
    public void ToDynamic_NoIdentifier_ThrowException()
    {
        var emptyReference = (Reference)Activator.CreateInstance(typeof(Reference), true);
        Action action = () => emptyReference.ToDynamic();

        action.Should()
            .Throw<DataMisalignedException>()
            .WithMessage("Element reference does not contain any identifier.");
    }

    [Fact]
    public void AssetWithRenditionsReference_FromDynamic_WithInvalidData_ThrowsDataMisalignedException()
    {
        Action action = () => AssetWithRenditionsReference.FromDynamic(new { invalidProperty = "invalid" });

        action.Should()
            .Throw<DataMisalignedException>()
            .WithMessage("Object could not be converted to the strongly-typed AssetWithRenditionsReference. Please check if it has expected properties with expected type");
    }

    [Fact]
    public void AssetWithRenditionsReference_FromDynamic_WithNullData_ThrowsDataMisalignedException()
    {
        Action action = () => AssetWithRenditionsReference.FromDynamic(null);

        action.Should()
            .Throw<DataMisalignedException>()
            .WithMessage("Object could not be converted to the strongly-typed AssetWithRenditionsReference. Please check if it has expected properties with expected type");
    }
}
