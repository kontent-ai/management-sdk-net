using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Users
{
    /// <summary>
    /// Represents user's invitation model.
    /// </summary>
    public class UserInviteModel
    {
        /// <summary>
        /// Gets or sets the email of user that is to be invited.
        /// </summary>
        [JsonProperty("email")]
        public string email { get; set; }

        /// <summary>
        /// Gets or sets the language's display name.
        /// </summary>
        [JsonProperty("collection_groups")]
        public IEnumerable<UserCollectionGroup> CollectionGroup { get; set; }
    }
}
