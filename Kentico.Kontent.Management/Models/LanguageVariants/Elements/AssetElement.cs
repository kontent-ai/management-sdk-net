using System;
using System.Collections.Generic;
using System.Linq;
using Kentico.Kontent.Management.Models.Assets;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements
{
    /// <summary>
    /// Represents strongly typed assets element.
    /// </summary>
    public class AssetElement : BaseElement
    {
        /// <summary>
        /// Gets or sets value of asset element.
        /// </summary>
        [JsonProperty("value")]
        public IEnumerable<AssetIdentifier> Value { get; set; }

        /// <summary>
        /// Creates new instance of AssertElement 
        /// <paramref name="data"/>Value of asset element
        /// </summary>
        public AssetElement(dynamic data = null) : base((object)data)
        {
            if (data != null)
            {
                // TODO - Verify if the internal type <ObjectIdentifier> is ok - maybe AssetIdentifier would fit in DynamicObjectJsonCoverter better
                Value = (data.value as IEnumerable<dynamic>)?.Select<dynamic, AssetIdentifier>(identifier => AssetIdentifier.ById(Guid.Parse(identifier.id)));
            }
        }

        /// <summary>
        /// Transforms the asset element to dynamic object.
        /// </summary>
        public override dynamic ToDynamic(string elementId)
        {
            return new
            {
                element = new { id = elementId },
                value = Value,
            };
        }
    }
}
