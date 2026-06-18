using AwesomeAssertions;
using Kontent.Ai.Management.Models.Assets;
using System.Text;

namespace Kontent.Ai.Management.Tests.Assets;

// Guards upload retry-safety: the resilience pipeline re-sends the same content on retry, so the body must survive
// being read more than once. A one-shot StreamContent would yield an empty second read; FileUploadContent re-reads.
public class FileUploadContentTests
{
    private static readonly byte[] Payload = Encoding.UTF8.GetBytes("Hello world from CM API .NET SDK");

    private static async Task<byte[]> SerializeAsync(HttpContent content)
    {
        using var buffer = new MemoryStream();
        await content.CopyToAsync(buffer);
        return buffer.ToArray();
    }

    [Fact]
    public async Task ByteArraySource_ReReadsFullBodyOnEveryAttempt()
    {
        var content = new FileUploadContent(new FileContentSource(Payload, "hello.txt", "text/plain"));

        var first = await SerializeAsync(content);
        var second = await SerializeAsync(content); // simulates a pipeline retry re-sending the request

        first.Should().Equal(Payload);
        second.Should().Equal(Payload);
    }

    [Fact]
    public async Task SeekableStreamSource_RewindsAndReReadsOnEveryAttempt()
    {
        var content = new FileUploadContent(new FileContentSource(new MemoryStream(Payload), "hello.txt", "text/plain"));

        var first = await SerializeAsync(content);
        var second = await SerializeAsync(content);

        first.Should().Equal(Payload);
        second.Should().Equal(Payload);
    }

    [Fact]
    public void SeekableSource_ReportsContentLength()
    {
        var content = new FileUploadContent(new FileContentSource(Payload, "hello.txt", "text/plain"));

        content.Length.Should().Be(Payload.Length);
        content.Headers.ContentLength.Should().Be(Payload.Length);
    }

    [Fact]
    public async Task NonSeekableSource_HasNoLengthAndStreamsOnce()
    {
        var content = new FileUploadContent(new FileContentSource(new NonSeekableStream(Payload), "hello.txt", "text/plain"));

        content.Length.Should().BeNull();
        content.Headers.ContentLength.Should().BeNull();
        (await SerializeAsync(content)).Should().Equal(Payload);
    }

    private sealed class NonSeekableStream(byte[] data) : MemoryStream(data)
    {
        public override bool CanSeek => false;
        public override long Length => throw new NotSupportedException();
    }
}
