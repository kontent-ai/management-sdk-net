using Kontent.Ai.Management.Models.Shared;
using System;
using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.UrlBuilder;

// A caller-supplied identifier (codename, external id, email, upload file name) becomes a URL path segment. Left raw,
// a value such as "../../webhooks" is normalized by System.Uri before the request is sent and retargets the call to a
// different Management API endpoint under the same API key. These tests pin that the offending characters are escaped
// or rejected at the URL-building layer so the request can never leave its intended segment.
public partial class EndpointUrlBuilderTests
{
    [Theory]
    [InlineData("../../webhooks", "..%2F..%2Fwebhooks")]
    [InlineData("a/b", "a%2Fb")]
    [InlineData("a\\b", "a%5Cb")]
    public void BuildItemUrl_Codename_EscapesPathSeparators(string codename, string escaped)
    {
        var actual = _builder.BuildItemUrl(Reference.ByCodename(codename));
        Assert.Equal($"{ENDPOINT}/projects/{ENVIRONMENT_ID}/items/codename/{escaped}", actual);
    }

    [Theory]
    [InlineData("../../webhooks", "..%2F..%2Fwebhooks")]
    [InlineData("a/b", "a%2Fb")]
    public void BuildItemUrl_ExternalId_EscapesPathSeparators(string externalId, string escaped)
    {
        var actual = _builder.BuildItemUrl(Reference.ByExternalId(externalId));
        Assert.Equal($"{ENDPOINT}/projects/{ENVIRONMENT_ID}/items/external-id/{escaped}", actual);
    }

    [Fact]
    public void BuildModifyUsersRoleUrl_Email_EscapesPathSeparators()
    {
        var actual = _builder.BuildModifyUsersRoleUrl(UserIdentifier.ByEmail("../../subscription"));
        Assert.Equal($"{ENDPOINT}/projects/{ENVIRONMENT_ID}/users/email/..%2F..%2Fsubscription/roles", actual);
    }

    [Fact]
    public void BuildUploadFileUrl_EscapesPathSeparators()
    {
        var actual = _builder.BuildUploadFileUrl("../../webhooks");
        Assert.Equal($"{ENDPOINT}/projects/{ENVIRONMENT_ID}/files/..%2F..%2Fwebhooks", actual);
    }

    [Theory]
    [InlineData(".")]
    [InlineData("..")]
    public void BuildItemUrl_Codename_RejectsDotSegments(string codename)
    {
        Assert.Throws<ArgumentException>(() => _builder.BuildItemUrl(Reference.ByCodename(codename)));
    }

    [Theory]
    [InlineData(".")]
    [InlineData("..")]
    public void BuildUploadFileUrl_RejectsDotSegments(string fileName)
    {
        Assert.Throws<ArgumentException>(() => _builder.BuildUploadFileUrl(fileName));
    }

    [Fact]
    public void BuildItemUrl_OrdinaryCodename_IsUnchanged()
    {
        var actual = _builder.BuildItemUrl(Reference.ByCodename("which_brewing_fits_you"));
        Assert.Equal($"{ENDPOINT}/projects/{ENVIRONMENT_ID}/items/codename/which_brewing_fits_you", actual);
    }
}
