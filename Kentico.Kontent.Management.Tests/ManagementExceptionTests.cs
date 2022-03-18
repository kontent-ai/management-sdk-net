using System;
using System.Net;
using System.Net.Http;

using Kentico.Kontent.Management.Exceptions;

using Xunit;

namespace Kentico.Kontent.Management.Tests;

public class ManagementExceptionTests
{
    [Fact]
    public void SimpleError_CorrectlyDeserialized()
    {
        var statusCode = HttpStatusCode.BadRequest;

        var response = new HttpResponseMessage(statusCode);
        var responseStr = "{ message: \"Some error\" }";

        var error = new ManagementException(response, responseStr);

        Assert.Equal(statusCode, error.StatusCode);
        Assert.Equal("Some error", error.Message);
    }


    [Fact]
    public void SimpleError_WithValidationErrors_CorrectlyDeserialized()
    {
        var statusCode = HttpStatusCode.BadRequest;

        var response = new HttpResponseMessage(statusCode);
        var responseStr = "{ message: \"Some error\", validation_errors: [ { message: \"First validation error\" }, { message: \"Second validation error\" } ] }";

        var error = new ManagementException(response, responseStr);

        Assert.Equal(statusCode, error.StatusCode);
        Assert.Equal($"Some error{Environment.NewLine}Validation errors:{Environment.NewLine}First validation error{Environment.NewLine}Second validation error", error.Message);
    }
}
