using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Subscription
{
    /// <summary>
    /// Represents the projects to which the user has been invited.
    /// </summary>
    public sealed class SubscriptionUserProjectModel
    {
        /// <summary>
        /// Gets or sets the project's internal ID.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the project's name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the project's environments.
        /// </summary>
        [JsonProperty("environments")]
        public IEnumerable<SubscriptionUserProjectEnvironmentModel> Environments { get; set; }
    }
}
