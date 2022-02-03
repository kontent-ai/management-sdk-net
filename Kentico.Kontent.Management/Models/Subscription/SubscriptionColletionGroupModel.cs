using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Subscription
{
    /// <summary>
    /// Represents set of collections user is assigned to with a set of roles.
    /// </summary>
    public sealed class SubscriptionColletionGroupModel
    {
        /// <summary>
        /// Gets or sets references to internal identifiers of collections that the user is assigned to. 
        /// If the array is empty, the user can access any collection.
        /// </summary>
        [JsonProperty("collections")]
        public IEnumerable<Reference> Collections { get; set; }

        /// <summary>
        /// Gets or sets roles the user is assigned to within the collection.
        /// </summary>
        [JsonProperty("roles")]
        public IEnumerable<SubscriptionUserRoleModel> Roles { get; set; }
    }
}
