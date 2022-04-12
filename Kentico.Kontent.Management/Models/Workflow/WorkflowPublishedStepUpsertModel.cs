using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Workflow
{
    /// <summary>
    /// Represents the Published workflow step upsert model.
    /// </summary>
    public class WorkflowPublishedStepUpsertModel
    {
        /// <summary>
        /// Gets or sets the roles which can create new version from published variant.
        /// </summary>
        [JsonProperty("create_new_version_role_ids", Required = Required.Always)]
        public IReadOnlyCollection<Guid> RoleCreateNewVersionIds  { get; set; }

        /// <summary>
        /// Gets or sets the roles which can unpublish the item's variant.
        /// </summary>
        [JsonProperty("unpublish_role_ids", Required = Required.Always)]
        public IReadOnlyCollection<Guid> RolesUnpublishArchivedCancelSchedulingIds { get; set; }
    }
}
