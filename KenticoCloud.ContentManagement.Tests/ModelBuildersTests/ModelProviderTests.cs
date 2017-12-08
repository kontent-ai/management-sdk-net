using System;
using System.Collections.Generic;
using System.Linq;
using KenticoCloud.ContentManagement.Models.Assets;
using KenticoCloud.ContentManagement.Models.Items;
using KenticoCloud.ContentManagement.Modules.ModelBuilders;
using KenticoCloud.ContentManagement.Tests.Data;
using Newtonsoft.Json.Linq;
using Xunit;

namespace KenticoCloud.ContentManagement.Tests.ModelBuildersTests
{
    public class ModelProviderTests
    {
        private readonly IModelProvider _modelProvider;

        public ModelProviderTests()
        {
            _modelProvider = new ModelProvider();
        }

        [Fact]
        public void GetContentItemVariantModel_ReturnsExpected()
        {
            var expected = GetTestModel();
            expected.MultipleChoiceElementCheckRadio = null;
            var model = new ContentItemVariantModel
            {
                Elements = new Dictionary<string, object>
                {
                    {"text_element", expected.TextElement},
                    {"datetime_element", expected.DateTimeElement.Value},
                    {"number_element", expected.Number},
                    {"urlslug_element", expected.UrlSlugElement},
                    {"richtext_element", expected.RichTextElement},
                    {"asset_element", JArray.FromObject(expected.AssetElement)},
                    {"modular_content_element", JArray.FromObject(expected.ModularContentElement)},
                    {"multiplechoice_element", JArray.FromObject(expected.MultipleChoiceElementCheck)},
                    {"taxonomygroup1", JArray.FromObject(expected.TaxonomyElement)},
                    {"multiplechoice_2", JArray.FromObject(new LinkedList<MultipleChoiceOptionIdentifier>(new[] {Guid.NewGuid(), Guid.NewGuid()}.Select(MultipleChoiceOptionIdentifier.ById)))}
                }
            };
            var actual = _modelProvider.GetContentItemVariantModel<ComplexTestModel>(model).Elements;

            AssertElements(expected, actual);
        }

        [Fact]
        public void GetContentItemVariantUpsertModel_ReturnsExpected()
        {
            var model = GetTestModel();
            var expected = new Dictionary<string, object>
            {
                {"text_element", model.TextElement},
                {"datetime_element", model.DateTimeElement.Value},
                {"number_element", model.Number},
                {"urlslug_element", model.UrlSlugElement},
                {"richtext_element", model.RichTextElement},
                {"asset_element", model.AssetElement},
                {"modular_content_element", model.ModularContentElement},
                {"multiplechoice_element", model.MultipleChoiceElementCheck},
                {"taxonomygroup1", model.TaxonomyElement}
            };
            var actual = _modelProvider.GetContentItemVariantUpsertModel(model).Elements;

            AssertElements(expected, actual);
        }

        private static void AssertElements(ComplexTestModel expected, ComplexTestModel actual)
        {
            AssertIdentifiers(expected.AssetElement.Select(x=>x.Id.Value), actual.AssetElement.Select(x => x.Id.Value));
            Assert.Equal(expected.DateTimeElement, actual.DateTimeElement);
            Assert.Equal(expected.UrlSlugElement, actual.UrlSlugElement);
            AssertIdentifiers(expected.ModularContentElement?.Select(x => x.Id.Value), actual.ModularContentElement?.Select(x => x.Id.Value));
            AssertIdentifiers(expected.MultipleChoiceElementCheck?.Select(x => x.Id.Value), actual.MultipleChoiceElementCheck?.Select(x => x.Id.Value));
            AssertIdentifiers(expected.MultipleChoiceElementCheckRadio?.Select(x => x.Id.Value), actual.MultipleChoiceElementCheckRadio?.Select(x => x.Id.Value));
            Assert.Equal(expected.Number, actual.Number);
            Assert.Equal(expected.RichTextElement, actual.RichTextElement);
            AssertIdentifiers(expected.TaxonomyElement?.Select(x => x.Id.Value), actual.TaxonomyElement?.Select(x => x.Id.Value));
            Assert.Equal(expected.TextElement, actual.TextElement);
        }

        private static ComplexTestModel GetTestModel()
        {
            return new ComplexTestModel
            {
                TextElement = "text",
                Number = 179.5649165M,
                DateTimeElement = DateTime.Now,
                UrlSlugElement = "urlslug",
                RichTextElement = "RichText",
                AssetElement = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(AssetIdentifier.ById).ToArray(),
                ModularContentElement = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(ContentItemIdentifier.ById).ToArray(),
                MultipleChoiceElementCheck = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(MultipleChoiceOptionIdentifier.ById).ToHashSet(),
                TaxonomyElement = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(TaxonomyTermIdentifier.ById).ToList(),
                MultipleChoiceElementCheckRadio = new LinkedList<MultipleChoiceOptionIdentifier>(new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(MultipleChoiceOptionIdentifier.ById)),
            };
        }

        private static void AssertElements(IDictionary<string, object> expected, IDictionary<string, object> actual)
        {
            Assert.Equal(expected.Count, actual.Count);
            foreach (var element in expected)
            {
                Assert.True(actual.ContainsKey(element.Key));
                Assert.Equal(element.Value, actual[element.Key]);
            }
        }

        private static void AssertIdentifiers(IEnumerable<Guid> expected, IEnumerable<Guid> actual)
        {
            if (expected == null && actual == null) return;
            if (expected == null || actual == null)
            {
                Assert.True(false, "Null check");
            }
            Assert.Equal(expected, actual);
        }
    }
}