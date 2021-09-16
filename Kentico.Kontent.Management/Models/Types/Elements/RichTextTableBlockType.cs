using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    /// <summary>
    /// Represents blocks types that can be used inside tables in your rich text element.
    /// </summary>
    public enum RichTextTableBlockType
    {
        /// <summary>
        /// Text
        /// </summary>
        [EnumMember(Value = "text")]
        Text,

        /// <summary>
        /// Images
        /// </summary>
        [EnumMember(Value = "images")]
        Images,
    }
}