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

        [JsonProperty("pages")]
        public decimal? Pages { get; set; }

        [JsonProperty("post_date")]
        public DateTime? PostDate { get; set; }

        [JsonProperty("url_pattern")]
        public string UrlPattern { get; set; }

        [JsonProperty("body_copy")]
        public string BodyCopy { get; set; }

        [JsonProperty("teaser_image")]
        public AssetIdentifier[] TeaserImage { get; set; }

        [JsonProperty("related_articles")]
        public IEnumerable<ContentItemIdentifier> RelatedArticles { get; set; }

        [JsonProperty("categories")]
        public HashSet<MultipleChoiceOptionIdentifier> Categories { get; set; }

        [JsonProperty("personas")]
        public List<TaxonomyTermIdentifier> Personas { get; set; }

        [JsonIgnore]
        public LinkedList<MultipleChoiceOptionIdentifier> IgnoredMultipleChoice { get; set; }
    }
}
