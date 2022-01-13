﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kentico.Kontent.Management.Models.Roles
{
    /// <summary>
    /// Represents project's roles
    /// </summary>
    public class ProjectRolesModel
    {
        /// <summary>
        /// Gets or sets the list of project roles
        /// </summary>
        [JsonProperty("roles")]
        public IEnumerable<ProjectRoleModel> Roles { get; set; }
    }
}
