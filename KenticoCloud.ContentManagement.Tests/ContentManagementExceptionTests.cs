using System;
using System.Net;
using System.Net.Http;

using KenticoCloud.ContentManagement.Exceptions;

using Xunit;

namespace KenticoCloud.ContentManagement.Tests
{
    public class ContentManagementExceptionTests
    {
        [Fact]
        public void SimpleError_CorrectlyDeserialized()
        {
            var statusCode = HttpStatusCode.BadRequest;

            var response = new HttpResponseMessage(statusCode);
            var responseStr = "{ message: \"Some error\" }";

            var error = new ContentManagementException(response, responseStr);

            Assert.Equal(statusCode, error.StatusCode);
            Assert.Equal("Some error", error.Message);
        }


        [Fact]
        public void SimpleError_WithValidationErrors_CorrectlyDeserialized()
        {
            var statusCode = HttpStatusCode.BadRequest;

            var response = new HttpResponseMessage(statusCode);
            var responseStr = "{ message: \"Some error\", validation_errors: [ { message: \"First validation error\" }, { message: \"Second validation error\" } ] }";

            var error = new ContentManagementException(response, responseStr);

            Assert.Equal(statusCode, error.StatusCode);
            Assert.Equal($"Some error{Environment.NewLine}Validation errors:{Environment.NewLine}First validation error{Environment.NewLine}Second validation error", error.Message);
        }
    }
}
