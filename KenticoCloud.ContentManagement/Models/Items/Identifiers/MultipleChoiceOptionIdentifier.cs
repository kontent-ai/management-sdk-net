using System;
using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Items
{
    /// <summary>
    /// Represents identifier of the multiplechoice option.
    /// </summary>
    public sealed class MultipleChoiceOptionIdentifier
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
        public static MultipleChoiceOptionIdentifier ById(Guid id)
        {
            return new MultipleChoiceOptionIdentifier() { Id = id };
        }

        /// <summary>
        /// Creates identifier by codename.
        /// </summary>
        /// <param name="codename">The codename of the identifier.</param>
        public static MultipleChoiceOptionIdentifier ByCodename(string codename)
        {
            return new MultipleChoiceOptionIdentifier() { Codename = codename };
        }
    }
}
