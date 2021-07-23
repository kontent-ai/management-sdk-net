using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public enum ElementMetadataType
    {
        Undefined,
        [EnumMember(Value = "text")]
        Text = 1,
        [EnumMember(Value = "rich_text")]
        RichText = 2,
        [EnumMember(Value = "number")]
        Number = 3,
        [EnumMember(Value = "multiple_choice")]
        MultipleChoice = 4,
        [EnumMember(Value = "date_time")]
        DateTime = 5,
        [EnumMember(Value = "asset")]
        Asset = 6,
        [EnumMember(Value = "modular_content")]
        ModularContent = 7,
        [EnumMember(Value = "guidelines")]
        Guidelines = 8,
        [EnumMember(Value = "taxonomy")]
        Taxonomy = 9,
        [EnumMember(Value = "url_slug")]
        UrlSlug = 10,
        [EnumMember(Value = "snippet")]
        Snippet = 11,
        [EnumMember(Value = "custom")]
        Custom = 12
    }
}
