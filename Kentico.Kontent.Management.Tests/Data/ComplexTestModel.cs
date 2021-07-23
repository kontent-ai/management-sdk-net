using System;
using System.Collections.Generic;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.StronglyTyped;
using Kentico.Kontent.Management.Modules.ModelBuilders;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Tests.Data
{

    public class ComplexTestModel
    {
        [JsonProperty("title")]
        [KontentElementId("ba7c8840-bcbc-5e3b-b292-24d0a60f3977")]
        public string Title { get; set; }

        [JsonProperty("post_date")]
        [KontentElementId("0827e079-3754-5a1d-9381-8ff695a5bbf7")]
        public DateTime? PostDate { get; set; }

        [JsonProperty("body_copy")]
        [KontentElementId("55a88ab3-4009-5bf9-a590-f32162f09b92")]
        public string BodyCopy { get; set; }

        [JsonProperty("summary")]
        [KontentElementId("15517aa3-da8a-5551-a4d4-555461fd5226")]
        public string Summary { get; set; }

        [JsonProperty("teaser_image")]
        [KontentElementId("9c6a4fbc-3f73-585f-9521-8d57636adf56")]
        public AssetIdentifier[] TeaserImage { get; set; }

        [JsonProperty("related_articles")]
        [KontentElementId("77108990-3c30-5ffb-8dcd-8eb85fc52cb1")]
        public IEnumerable<ContentItemIdentifier> RelatedArticles { get; set; }

        [JsonProperty("personas")]
        [KontentElementId("c1dc36b5-558d-55a2-8f31-787430a68e4d")]
        public List<TaxonomyTermIdentifier> Personas { get; set; }

        [JsonProperty("meta_keywords")]
        [KontentElementId("0ee20a72-0aaa-521f-8801-df3d9293b7dd")]
        public string MetaKeywords { get; set; }

        [JsonProperty("meta_description")]
        [KontentElementId("7df0048f-eaaf-50f8-85cf-fa0fc0d6d815")]
        public string MetaDescription { get; set; }

        [JsonProperty("url_pattern")]
        [KontentElementId("1f37e15b-27a0-5f48-b314-03b401c19cee")]
        public UrlSlug UrlPattern { get; set; }
    }
}
