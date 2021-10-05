using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Items
{
    /// <summary>
    /// Represents content item update model.
    /// </summary>
    public sealed class ContentItemUpdateModel
    {
        /// <summary>
        /// Gets or sets the name of the content item.
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the codename of the content item.
        /// </summary>
        [JsonProperty("codename")]
        public string Codename { get; set; }

        /// <summary>
        /// Gets or sets the collection of the content item.
        /// </summary>
        [JsonProperty("collection")]
        public Reference Collection { get; set; }

        /// <summary>
        /// A default constructor.
        /// </summary>
        public ContentItemUpdateModel()
        {
        }

        /// <summary>
        /// Instantiates the <see cref="ContentItemUpdateModel"/> using an instance of the <see cref="ContentItemModel"/>.
        /// </summary>
        /// <param name="contentItem"></param>
        public ContentItemUpdateModel(ContentItemModel contentItem)
        {
            Name = contentItem.Name;
        }
    }
}
