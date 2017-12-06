using System;
using System.Collections.Generic;
using KenticoCloud.ContentManagement.Models.Assets;
using KenticoCloud.ContentManagement.Models.Items;
using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Tests.Data
{
    internal class ComplexTestModel
    {
        [JsonProperty("text_element")]
        public string TextElement { get; set; }

        [JsonProperty("number_element")]
        public decimal? Number { get; set; }

        [JsonProperty("datetime_element")]
        public DateTime? DateTimeElement { get; set; }

        [JsonProperty("urlslug_element")]
        public string UrlSlugElement { get; set; }

        [JsonProperty("richtext_element")]
        public string RichTextElement { get; set; }

        [JsonProperty("asset_element")]
        public AssetIdentifier[] AssetElement { get; set; }

        [JsonProperty("modular_content_element")]
        public IEnumerable<ContentItemIdentifier> ModularContentElement { get; set; }

        [JsonProperty("multiplechoice_element")]
        public HashSet<MultipleChoiceOptionIdentifier> MultipleChoiceElementCheck { get; set; }

        [JsonProperty("taxonomygroup1")]
        public List<TaxonomyTermIdentifier> TaxonomyElement { get; set; }

        [JsonIgnore]
        public LinkedList<MultipleChoiceOptionIdentifier> MultipleChoiceElementCheckRadio { get; set; }
    }
}
