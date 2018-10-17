using KenticoCloud.ContentManagement.Models.ProjectReport.Metadata;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace KenticoCloud.ContentManagement.Models.ProjectReport
{
	/// <summary>
	/// Represents information necessary to identify 
	/// the language variant and lists the content elements
	/// </summary>
	public sealed class VariantIssue
	{
		/// <summary>
		/// Gets or sets information about content item
		/// </summary>
		[JsonProperty("item")]
		public ContentItemMetadata Item { get; set; }

		/// <summary>
		/// Gets or sets information about project language
		/// </summary>
		[JsonProperty("language")]
		public LanguageMetadata Language { get; set; }

		/// <summary>
		/// Gets or sets information about issues
		/// found in specific content elements
		/// </summary>
		[JsonProperty("issues")]
		public List<ContentElementIssue> Issues { get; set; }
	}
}
