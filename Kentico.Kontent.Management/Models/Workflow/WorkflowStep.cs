using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Workflow
{
    public class WorkflowStep
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("codename")]
        public string Codename { get; set; }

        [JsonProperty("transitions_to")]
        public IEnumerable<Guid> TransitionsTo { get; set; }
    }
}
