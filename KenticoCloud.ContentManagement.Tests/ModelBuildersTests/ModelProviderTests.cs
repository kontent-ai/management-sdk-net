using System;
using System.Collections.Generic;
using System.Linq;
using KenticoCloud.ContentManagement.Models.Assets;
using KenticoCloud.ContentManagement.Models.Items;
using KenticoCloud.ContentManagement.Modules.ModelBuilders;
using KenticoCloud.ContentManagement.Tests.Data;
using Newtonsoft.Json;
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
            expected.IgnoredMultipleChoice = null;
            var model = new ContentItemVariantModel
            {
                Elements = new Dictionary<string, object>
                {
                    {"title", expected.Title},
                    {"post_date", expected.PostDate.Value},
                    {"pages", expected.Pages},
                    {"url_pattern", expected.UrlPattern},
                    {"body_copy", expected.BodyCopy},
                    {"teaser_image", JArray.FromObject(expected.TeaserImage)},
                    {"related_articles", JArray.FromObject(expected.RelatedArticles)},
                    {"categories", JArray.FromObject(expected.Categories)},
                    {"personas", JArray.FromObject(expected.Personas)},
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
                {"title", model.Title},
                {"post_date", model.PostDate.Value},
                {"pages", model.Pages},
                {"url_pattern", model.UrlPattern},
                {"body_copy", model.BodyCopy},
                {"teaser_image", JArray.FromObject(model.TeaserImage)},
                {"related_articles", JArray.FromObject(model.RelatedArticles)},
                {"categories", JArray.FromObject(model.Categories)},
                {"personas", JArray.FromObject(model.Personas)},
            };
            var actual = _modelProvider.GetContentItemVariantUpsertModel(model).Elements;

            AssertElements(expected, actual);
        }

        private static void AssertElements(ComplexTestModel expected, ComplexTestModel actual)
        {
            AssertIdentifiers(expected.TeaserImage.Select(x=>x.Id.Value), actual.TeaserImage.Select(x => x.Id.Value));
            Assert.Equal(expected.PostDate, actual.PostDate);
            Assert.Equal(expected.UrlPattern, actual.UrlPattern);
            AssertIdentifiers(expected.RelatedArticles?.Select(x => x.Id.Value), actual.RelatedArticles?.Select(x => x.Id.Value));
            AssertIdentifiers(expected.Categories?.Select(x => x.Id.Value), actual.Categories?.Select(x => x.Id.Value));
            AssertIdentifiers(expected.IgnoredMultipleChoice?.Select(x => x.Id.Value), actual.IgnoredMultipleChoice?.Select(x => x.Id.Value));
            Assert.Equal(expected.Pages, actual.Pages);
            Assert.Equal(expected.BodyCopy, actual.BodyCopy);
            AssertIdentifiers(expected.Personas?.Select(x => x.Id.Value), actual.Personas?.Select(x => x.Id.Value));
            Assert.Equal(expected.Title, actual.Title);
        }

        private static ComplexTestModel GetTestModel()
        {
            return new ComplexTestModel
            {
                Title = "text",
                Pages = 179.5649165M,
                PostDate = DateTime.Now,
                UrlPattern = "urlslug",
                BodyCopy = "RichText",
                TeaserImage = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(AssetIdentifier.ById).ToArray(),
                RelatedArticles = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(ContentItemIdentifier.ById).ToArray(),
                Categories = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(MultipleChoiceOptionIdentifier.ById).ToHashSet(),
                Personas = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(TaxonomyTermIdentifier.ById).ToList(),
                IgnoredMultipleChoice = new LinkedList<MultipleChoiceOptionIdentifier>(new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(MultipleChoiceOptionIdentifier.ById)),
            };
        }

        private static void AssertElements(IDictionary<string, object> expected, IDictionary<string, object> actual)
        {
            Assert.True(JToken.DeepEquals(JToken.FromObject(expected), JToken.FromObject(actual)));
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