using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Modules.ModelBuilders;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Unit.Modules.ModelBuilders
{
    public class ElementBuilderTests
    {
        [Fact]
        public void GetElementsAsDynamic_NoReferenceProvided_RaiseException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ElementBuilder.GetElementsAsDynamic(new BaseElement[]
                {
                    new  TextElement
                    {
                        Value = "Test"
                    }
                }));
        }

        [Fact]
        public void GetElementsAsDynamic_NoIdentificationProvided_RaiseException()
        {
            Assert.Throws<ArgumentException>(() =>
                ElementBuilder.GetElementsAsDynamic(new BaseElement[]
                {
                    new  TextElement
                    {
                        Element = new Reference(),
                        Value = "Test"
                    }
                }));
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
                Assert.Equal(id, elements.First()?.element?.id);
                Assert.Equal(elementValue, elements.First()?.value);
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
                Assert.Equal(codename, elements.First()?.element?.codename);
                Assert.Equal(elementValue, elements.First()?.value);
            }
        }
    }
}