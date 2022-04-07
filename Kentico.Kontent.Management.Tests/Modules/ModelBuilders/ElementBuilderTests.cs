using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Modules.ModelBuilders;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Modules.ModelBuilders;

public class ElementBuilderTests
{
    [Fact]
    public void GetElementsAsDynamic_NoReferenceProvided_RaiseException()
    {
        Action action = () =>
           ElementBuilder.GetElementsAsDynamic(new BaseElement[]
           {
                new  TextElement
                {
                    Value = "Test"
                }
           });

        action.Should()
            .Throw<AggregateException>()
            .WithInnerException<ArgumentNullException>();
    }

    [Fact]
    public void GetElementsAsDynamic_NoIdentificationProvided_ThrowsException()
    {
        Action action = () =>
            ElementBuilder.GetElementsAsDynamic(new BaseElement[]
            {
                new  TextElement
                {
                    Element = Reference.ByExternalId(null),
                    Value = "Test"
                }
            });

        action.Should()
            .Throw<AggregateException>()
            .WithInnerException<ArgumentException>();
    }

    [Fact]
    public void GetElementsAsDynamic_IdIdentification_ReturnCorrectResult()
    {
        var id = Guid.Parse("ebc7f389-8ad3-4366-b2ca-e452c3f2a807");
        var elementValue = "Test";

        var elements = ElementBuilder.GetElementsAsDynamic(new BaseElement[]
        {
            new  TextElement
            {
                Element = Reference.ById(id),
                Value = elementValue
            }
        });

        using (new AssertionScope())
        {
            elements.Should().HaveCount(1);
            ((object)elements.First()?.element?.id).Should().Be(id);
            ((object)elements.First()?.value).Should().Be(elementValue);
        }
    }

    [Fact]
    public void GetElementsAsDynamic_CodenameIdentification_ReturnCorrectResult()
    {
        var codename = "codename";
        var elementValue = "Test";

        var elements = ElementBuilder.GetElementsAsDynamic(new BaseElement[]
        {
            new  TextElement
            {
                Element = Reference.ByCodename(codename),
                Value = elementValue
            }
        });

        using (new AssertionScope())
        {
            elements.Should().HaveCount(1);
            ((object)elements.First()?.element?.codename).Should().Be(codename);
            ((object)elements.First()?.value).Should().Be(elementValue);
        }
    }
}
