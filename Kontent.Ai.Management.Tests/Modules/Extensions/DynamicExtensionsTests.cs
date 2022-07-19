using Kontent.Ai.Management.Extensions;
using System.Dynamic;
using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.Extensions;

public class DynamicExtensionsTests
{
    [Fact]
    public void HasProperty_ExpandoObjectWithProperty_True()
    {
        dynamic source = new ExpandoObject();
        source.test = "test";

        Assert.True(DynamicExtensions.HasProperty(source, "test"));
    }

    [Fact]
    public void HasProperty_ExpandoObjectWithoutProperty_False()
    {
        dynamic source = new ExpandoObject();

        Assert.False(DynamicExtensions.HasProperty(source, "test"));
    }

    [Fact]
    public void HasProperty_ObjectWithProperty_True()
    {
        dynamic source = new {
            test = "test"
        };

        Assert.True(DynamicExtensions.HasProperty(source, "test"));
    }

    [Fact]
    public void HasProperty_ObjectWithoutProperty_False()
    {
        dynamic source = new { };

        Assert.False(DynamicExtensions.HasProperty(source, "test"));
    }
}
