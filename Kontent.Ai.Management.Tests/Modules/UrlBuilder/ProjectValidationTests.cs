using System;
using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildAsyncValidationUrl_ReturnsCorrectUrl()
    {
        var actualUrl = _builder.BuildAsyncValidationUrl();
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/validate-async";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildAsyncValidationTaskUrl_ReturnsCorrectUrl()
    {
        var taskId = Guid.NewGuid();

        var actualUrl = _builder.BuildAsyncValidationTaskUrl(taskId);
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/validate-async/tasks/{taskId}";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildAsyncValidationTaskIssuesUrl_ReturnsCorrectUrl()
    {
        var taskId = Guid.NewGuid();

        var actualUrl = _builder.BuildAsyncValidationTaskIssuesUrl(taskId);
        var expectedUrl = $"{ENDPOINT}/projects/{PROJECT_ID}/validate-async/tasks/{taskId}/issues";

        Assert.Equal(expectedUrl, actualUrl);
    }
}
