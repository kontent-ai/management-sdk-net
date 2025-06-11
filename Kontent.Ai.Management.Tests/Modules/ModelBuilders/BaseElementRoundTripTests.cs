using FluentAssertions;
using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Types.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.ModelBuilders;

public class BaseElementRoundTripTests
{
    public static IEnumerable<object[]> ReferenceElementTestCases()
    {
        yield return new object[] {
            new TaxonomyElement {
                Element = Reference.ByCodename("tax"),
                Value = [Reference.ByCodename("t1"), Reference.ByCodename("t2")]
            },
            ElementMetadataType.Taxonomy,
            new Action<BaseElement>(converted => {
                var e = (TaxonomyElement)converted;
                e.Element.Codename.Should().Be("tax");
                e.Value.Should().HaveCount(2);
                e.Value.First().Codename.Should().Be("t1");
                e.Value.Last().Codename.Should().Be("t2");
            })
        };
        
        yield return new object[] {
            new LinkedItemsElement {
                Element = Reference.ByCodename("link"),
                Value = [Reference.ByCodename("i1"), Reference.ByExternalId("ext2")]
            },
            ElementMetadataType.LinkedItems,
            new Action<BaseElement>(converted => {
                var e = (LinkedItemsElement)converted;
                e.Element.Codename.Should().Be("link");
                e.Value.Should().HaveCount(2);
                e.Value.First().Codename.Should().Be("i1");
                e.Value.Last().ExternalId.Should().Be("ext2");
            })
        };
        
        yield return new object[] {
            new MultipleChoiceElement {
                Element = Reference.ByCodename("mc"),
                Value = [Reference.ByCodename("o1"), Reference.ByCodename("o2")]
            },
            ElementMetadataType.MultipleChoice,
            new Action<BaseElement>(converted => {
                var e = (MultipleChoiceElement)converted;
                e.Element.Codename.Should().Be("mc");
                e.Value.Should().HaveCount(2);
                e.Value.First().Codename.Should().Be("o1");
                e.Value.Last().Codename.Should().Be("o2");
            })
        };
        
        yield return new object[] {
            new SubpagesElement {
                Element = Reference.ByCodename("sp"),
                Value = [Reference.ByCodename("p1"), Reference.ByCodename("p2")]
            },
            ElementMetadataType.Subpages,
            new Action<BaseElement>(converted => {
                var e = (SubpagesElement)converted;
                e.Element.Codename.Should().Be("sp");
                e.Value.Should().HaveCount(2);
                e.Value.First().Codename.Should().Be("p1");
                e.Value.Last().Codename.Should().Be("p2");
            })
        };
    }

    [Theory]
    [MemberData(nameof(ReferenceElementTestCases))]
    public void ReferenceElements_RoundTrip_PreservesValues(BaseElement original, ElementMetadataType type, Action<BaseElement> assertConverted)
    {
        // Act
        var dynamic = original.ToDynamic();
        var converted = BaseElement.FromDynamic(dynamic, type);

        // Assert
        assertConverted(converted);
    }

    public static IEnumerable<object[]> AssetElementTestCases()
    {
        yield return new object[] {
            new AssetElement {
                Element = Reference.ByCodename("asset"),
                Value = [
                    new AssetWithRenditionsReference(Reference.ByCodename("a1"), [Reference.ByCodename("r1")]),
                    new AssetWithRenditionsReference(Reference.ByCodename("a2"))
                ]
            },
            new Action<AssetElement>(converted => {
                var assets = converted.Value.ToArray();
                converted.Element.Codename.Should().Be("asset");
                assets[0].Codename.Should().Be("a1");
                assets[0].Renditions.Should().HaveCount(1);
                assets[0].Renditions.First().Codename.Should().Be("r1");
                assets[1].Codename.Should().Be("a2");
                assets[1].Renditions.Should().BeEmpty();
            })
        };

        yield return new object[] {
            new AssetElement {
                Element = Reference.ByCodename("asset"),
                Value = [
                    new AssetWithRenditionsReference(Reference.ById(Guid.NewGuid()), [Reference.ByCodename("rn")]),
                    new AssetWithRenditionsReference(Reference.ByExternalId("ext_a"), [Reference.ByExternalId("ext_r")])
                ]
            },
            new Action<AssetElement>(converted => {
                var assets = converted.Value.ToArray();
                assets[0].Id.Should().NotBeNull();
                assets[0].Renditions.First().Codename.Should().Be("rn");
                assets[1].ExternalId.Should().Be("ext_a");
                assets[1].Renditions.First().ExternalId.Should().Be("ext_r");
            })
        };

        yield return new object[] {
            new AssetElement {
                Element = Reference.ByCodename("asset"),
                Value = null
            },
            new Action<AssetElement>(converted => {
                converted.Value.Should().BeNull();
            })
        };
    }

    [Theory]
    [MemberData(nameof(AssetElementTestCases))]
    public void AssetElement_RoundTrip_PreservesValues(AssetElement original, Action<AssetElement> assertConverted)
    {
        // Act
        var dynamic = original.ToDynamic();
        var converted = (AssetElement)BaseElement.FromDynamic(dynamic, ElementMetadataType.Asset);

        // Assert
        assertConverted(converted);
    }
}
