using Kentico.Kontent.Management.Modules.Extensions;
using Kentico.Kontent.Management.Modules.ModelBuilders;
using System;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Modules.Extensions;

public class PropertyInfoExtensionsTests
{
    internal const string ELEMENT_ID_GUID = "632afb85-9b1a-46aa-9717-5991ae859e13";
    internal class PropertyInfoExtensionsTestsSampleClass
    {
        public string Property1 { get; set; }

        [KontentElementId(ELEMENT_ID_GUID)]
        public string Property2 { get; set; }
    }

    [Fact]
    public void GetKontentElementId_ThorwsIfNoAttribute()
    {
        var property = typeof(PropertyInfoExtensionsTestsSampleClass).GetProperty("Property1");

        Assert.Throws<InvalidOperationException>(() => property.GetKontentElementId());
    }

    [Fact]
    public void GetKontentElementId_ReturnsAttributeValue()
    {
        var property = typeof(PropertyInfoExtensionsTestsSampleClass).GetProperty("Property2");

        Assert.Equal(Guid.Parse(ELEMENT_ID_GUID), property.GetKontentElementId());
    }
}
