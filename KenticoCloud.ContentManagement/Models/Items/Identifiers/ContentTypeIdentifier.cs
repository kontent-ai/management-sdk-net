using Newtonsoft.Json;
using System;

namespace KenticoCloud.ContentManagement.Models.Items
{
    /// <summary>
    /// Represents identifier of the content type.
    /// </summary>
    public sealed class ContentTypeIdentifier
    {
        /// <summary>
        /// Gets id of the identifier.
        /// </summary>
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? Id { get; private set; }

        /// <summary>
        /// Gets codename of the identifier.
        /// </summary>
        [JsonProperty("codename", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Codename { get; private set; }
        
        /// <summary>
        /// Creates identifier by id.
        /// </summary>
        /// <param name="id">The id of the identifier.</param>
        public static ContentTypeIdentifier ById(Guid id)
        {
            return new ContentTypeIdentifier() { Id = id };
        }

        /// <summary>
        /// Creates identifier by codename.
        /// </summary>
        /// <param name="codename">The codename of the identifier.</param>
        public static ContentTypeIdentifier ByCodename(string codename)
        {
            return new ContentTypeIdentifier() { Codename = codename };
        }
    }
}
