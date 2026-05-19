using System.Text.Json;
using AwesomeAssertions;
using Kontent.Ai.Management.Serialization.Converters;
using Xunit;

namespace Kontent.Ai.Management.Tests.Serialization;

public class DecimalJsonConverterTests
{
    private static readonly JsonSerializerOptions Options = new() { Converters = { new DecimalJsonConverter() } };

    [Fact]
    public void StjDefault_PreservesDecimalScale()
    {
        // Documents why the converter is not dead code: System.Text.Json keeps a scaled zero's
        // scale, so 0.0m would otherwise reach the wire as "0.0".
        JsonSerializer.Serialize(0.0m).Should().Be("0.0");
        JsonSerializer.Serialize(0m).Should().Be("0");
    }

    [Fact]
    public void Converter_ZeroOfAnyScale_SerializesAsZero()
    {
        JsonSerializer.Serialize(0m, Options).Should().Be("0");
        JsonSerializer.Serialize(0.0m, Options).Should().Be("0");
        JsonSerializer.Serialize(0.00m, Options).Should().Be("0");
        JsonSerializer.Serialize(decimal.Zero, Options).Should().Be("0");
    }

    [Fact]
    public void Converter_NonZero_PreservesValue()
    {
        JsonSerializer.Serialize(12.50m, Options).Should().Be("12.50");
        JsonSerializer.Serialize(123m, Options).Should().Be("123");
        JsonSerializer.Serialize(-1.5m, Options).Should().Be("-1.5");
    }

    [Fact]
    public void Converter_Roundtrips()
        => JsonSerializer.Deserialize<decimal>(JsonSerializer.Serialize(42.42m, Options), Options).Should().Be(42.42m);
}
