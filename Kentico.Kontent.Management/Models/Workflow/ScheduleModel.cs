using Newtonsoft.Json;
using System;

namespace Kentico.Kontent.Management.Models.Workflow
{
    public class ScheduleModel
    {
        [JsonProperty("scheduled_to")]
        public DateTimeOffset ScheduleTo { get; set; }
    }
}
