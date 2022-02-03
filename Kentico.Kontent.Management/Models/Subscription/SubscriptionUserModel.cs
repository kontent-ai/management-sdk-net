using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Subscription
{
    /// <summary>
    /// Represents subscription user object.
    /// </summary>
    public sealed class SubscriptionUserModel
    {
        /// <summary>
        /// Gets or sets the id of the user.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the user's name.
        /// </summary>
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's name.
        /// </summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's name.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's name.
        /// </summary>
        [JsonProperty("has_pending_invitation")]
        public bool HasPendingInvitation { get; set; }

        /// <summary>
        /// Gets or sets the user's name.
        /// </summary>
        [JsonProperty("projects")]
        public IEnumerable<SubscriptionUserProjectModel> Projects { get; set; }
    }
}
