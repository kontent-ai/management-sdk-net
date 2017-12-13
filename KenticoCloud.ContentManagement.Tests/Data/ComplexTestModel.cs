using System;
using System.Collections.Generic;
using KenticoCloud.ContentManagement.Models.Assets;
using KenticoCloud.ContentManagement.Models.Items;
using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Tests.Data
{
    internal class ComplexTestModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("post_date")]
        public DateTime? PostDate { get; set; }

        [JsonProperty("body_copy")]
        public string BodyCopy { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("teaser_image")]
        public AssetIdentifier[] TeaserImage { get; set; }

        [JsonProperty("related_articles")]
        public IEnumerable<ContentItemIdentifier> RelatedArticles { get; set; }

        [JsonProperty("personas")]
        public List<TaxonomyTermIdentifier> Personas { get; set; }

        [JsonProperty("meta_keywords")]
        public string MetaKeywords { get; set; }

        [JsonProperty("meta_description")]
        public string MetaDescription { get; set; }

        [JsonProperty("url_pattern")]
        public string UrlPattern { get; set; }
    }
}
