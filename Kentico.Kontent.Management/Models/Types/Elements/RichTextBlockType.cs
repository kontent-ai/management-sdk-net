using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    /// <summary>
    /// Specifies which blocks are allowed inside your rich text element.
    /// You can allow text, tables, images, and components and items. To allow all blocks, leave the array empty.
    /// </summary>
    public enum RichTextBlockType
    {
        /// <summary>
        /// Text block.
        /// </summary>
        [EnumMember(Value = "text")]
        Text,

        /// <summary>
        /// Tables block.
        /// </summary>
        [EnumMember(Value = "tables")]
        Tables,

        /// <summary>
        /// Images block.
        /// </summary>
        [EnumMember(Value = "images")]
        Images,

        /// <summary>
        /// Components and items block.
        /// </summary>
        [EnumMember(Value = "components-and-items")]
        ComponentsAndItems
    }
}