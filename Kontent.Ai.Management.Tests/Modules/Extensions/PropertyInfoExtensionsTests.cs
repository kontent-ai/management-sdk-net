using Kontent.Ai.Management.Modules.ModelBuilders;
using Kontent.Ai.Management.Modules.Extensions;
using System;
using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.Extensions;

public class PropertyInfoExtensionsTests
{
    internal const string ELEMENT_ID_GUID = "632afb85-9b1a-46aa-9717-5991ae859e13";
    internal const string ELEMENT_EXTERNAL_ID = "external_id";

    internal class PropertyInfoExtensionsTestsSampleClass
    {
        public string Property1 { get; set; }

        [KontentElementId(ELEMENT_ID_GUID)]
        [KontentElementExternalId(ELEMENT_EXTERNAL_ID)]
        public string Property2 { get; set; }
    }

    [Fact]
    public void GetKontentElementId_ThrowsIfNoAttribute()
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

    [Fact]
    public void GetKontentElementExternalId_ThrowsIfNoAttribute()
    {
        var property = typeof(PropertyInfoExtensionsTestsSampleClass).GetProperty("Property1");

        Assert.Throws<InvalidOperationException>(() => property.GetKontentElementExternalId());
    }

    [Fact]
    public void GetKontentElementExternalId_ReturnsAttributeValue()
    {
        var property = typeof(PropertyInfoExtensionsTestsSampleClass).GetProperty("Property2");

        Assert.Equal(ELEMENT_EXTERNAL_ID, property.GetKontentElementExternalId());
    }
}
