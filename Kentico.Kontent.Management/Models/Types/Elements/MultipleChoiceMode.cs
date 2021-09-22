using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    /// <summary>
    /// Defines whether the multiple choice element acts as a single choice (shown as radio buttons in the UI) or multiple choice (shown as checkboxes in the UI).
    /// </summary>
    public enum  MultipleChoiceMode
    {
        /// <summary>
        /// Multiple choice (shown as checkboxes in the UI).
        /// </summary>
        [EnumMember(Value = "multiple")]
        Multiple,

        /// <summary>
        /// Single choice (shown as radio buttons in the UI) 
        /// </summary>
        [EnumMember(Value = "single")]
        Single
    }
}
