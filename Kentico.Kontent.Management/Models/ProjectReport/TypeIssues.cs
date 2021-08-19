using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.ProjectReport
{
    public class TypeIssues
    {
		/// <summary>
		/// Gets or sets information about content type
		/// </summary>
		[JsonProperty("type")]
		public Metadata Type { get; set; }

		/// <summary>
		/// Gets or sets information about project language
		/// </summary>
		[JsonProperty("issues")]
		public IEnumerable<ElementIssue> Issues { get; set; }
	}
}