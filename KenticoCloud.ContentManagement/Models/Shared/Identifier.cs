using System;

using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Identifiers
{
    /// <summary>
    /// Represents base class for identifiers.
    /// </summary>
    public abstract class Identifier<T>
        where T : Identifier<T>, new()
    {
        /// <summary>
        /// Gets id of the identifier.
        /// </summary>
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? Id { get; private set; }

        /// <summary>
        /// Creates identifier by id.
        /// </summary>
        /// <param name="id">The id of the identifier.</param>
        public static T ById(Guid id)
        {
            return new T() { Id = id };
        }

        protected Identifier() { }
    }
}
