using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.ProjectReport;

	/// <summary>
	/// Represents information necessary to identify 
	/// the language variant and lists the content elements
	/// </summary>
	public sealed class VariantIssue
	{
		/// <summary>
		/// Gets or sets information about the content item
		/// </summary>
		[JsonProperty("item")]
		public Metadata Item { get; set; }

		/// <summary>
		/// Gets or sets information about project language
		/// </summary>
		[JsonProperty("language")]
		public Metadata Language { get; set; }

		/// <summary>
		/// Gets or sets information about issues
		/// found in specific content elements
		/// </summary>
		[JsonProperty("issues")]
		public List<ElementIssue> Issues { get; set; }
	}
