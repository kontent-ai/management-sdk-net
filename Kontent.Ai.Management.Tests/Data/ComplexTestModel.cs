using Kontent.Ai.Management.Modules.ModelBuilders;
using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Tests.Data;

internal partial class ComplexTestModel
{
    [JsonProperty("title")]
    [KontentElementId("ba7c8840-bcbc-5e3b-b292-24d0a60f3977")]
    [KontentElementExternalId("85d5efc6-f47e-2fde-a6f5-0950fe89ecd1")]
    public TextElement Title { get; set; }

    [JsonProperty("rating")]
    [KontentElementId("773940f4-9e67-4a26-a93f-67e55fd7d837")]
    [KontentElementExternalId("773940f4-9e67-4a26-a93f-67e55fd7d837")]
    public NumberElement Rating { get; set; }

    [JsonProperty("options")]
    [KontentElementId("53a25074-b136-4a1f-a16d-3c130f696c66")]
    [KontentElementExternalId("53a25074-b136-4a1f-a16d-3c130f696c66")]
    public MultipleChoiceElement Options { get; set; }

    [JsonProperty("post_date")]
    [KontentElementId("0827e079-3754-5a1d-9381-8ff695a5bbf7")]
    [KontentElementExternalId("4ae5f7a9-fe1f-1e8c-bfec-d321455139c4")]
    public DateTimeElement PostDate { get; set; }

    [JsonProperty("body_copy")]
    [KontentElementId("55a88ab3-4009-5bf9-a590-f32162f09b92")]
    [KontentElementExternalId("108ed7c0-fc8c-c0ec-d0b5-5a8071408b54")]
    public RichTextElement BodyCopy { get; set; }

    [JsonProperty("selected_form")]
    [KontentElementId("47bf7d6d-285d-4ed0-9919-1d1a98b43acd")]
    [KontentElementExternalId("47bf7d6d-285d-4ed0-9919-1d1a98b43acd")]
    public CustomElement SelectedForm { get; set; }

    [JsonProperty("summary")]
    [KontentElementId("15517aa3-da8a-5551-a4d4-555461fd5226")]
    [KontentElementExternalId("90550cbe-7bff-40a9-2947-9c81489fe562")]
    public TextElement Summary { get; set; }

    [JsonProperty("teaser_image")]
    [KontentElementId("9c6a4fbc-3f73-585f-9521-8d57636adf56")]
    [KontentElementExternalId("62eb9881-e222-6b81-91d2-fdf052726414")]
    public AssetElement TeaserImage { get; set; }

    [JsonProperty("related_articles")]
    [KontentElementId("77108990-3c30-5ffb-8dcd-8eb85fc52cb1")]
    [KontentElementExternalId("ee7c3687-b469-6c56-3ac6-c8dfdc8b58b5")]
    public LinkedItemsElement RelatedArticles { get; set; }

    [JsonProperty("personas")]
    [KontentElementId("c1dc36b5-558d-55a2-8f31-787430a68e4d")]
    [KontentElementExternalId("0a16b642-ac3e-584d-a45a-ba354a30b2bd")]
    public TaxonomyElement Personas { get; set; }

    [JsonProperty("meta_keywords")]
    [KontentElementId("0ee20a72-0aaa-521f-8801-df3d9293b7dd")]
    [KontentElementExternalId("5efb2425-5987-a4a6-a2d3-b14712b56e73")]
    public TextElement MetaKeywords { get; set; }

    [JsonProperty("meta_description")]
    [KontentElementId("7df0048f-eaaf-50f8-85cf-fa0fc0d6d815")]
    [KontentElementExternalId("b9dc537c-2518-e4f5-8325-ce4fce26171e")]
    public TextElement MetaDescription { get; set; }

    [JsonProperty("url_pattern")]
    [KontentElementId("1f37e15b-27a0-5f48-b314-03b401c19cee")]
    [KontentElementExternalId("f2ff5e3f-a9ca-4604-58b0-34a2ad6a7cf1")]
    public UrlSlugElement UrlPattern { get; set; }

    [JsonProperty("cafe_subpage")]
    [KontentElementId("a29858ff-fa9f-5841-a682-d7fb6cc6effe")]
    [KontentElementExternalId("a29858ff-fa9f-5841-a682-d7fb6cc6effe")]
    public SubpagesElement Cafe { get; set; }
}
