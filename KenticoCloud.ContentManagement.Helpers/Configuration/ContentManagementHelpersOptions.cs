namespace KenticoCloud.ContentManagement.Helpers.Configuration
{
    /// <summary>
    /// Keeps settings which are provided by customer or have default values, used in <see cref="EditLinkBuilder"/>.
    /// </summary>
    public class ContentManagementHelpersOptions
    {
        /// <summary>
        /// Gets or sets the Admin URL. Optional, defaults to "https://app.kenticocloud.com/{0}".
        /// </summary>
        public string AdminUrl { get; set; } = "https://app.kenticocloud.com/{0}";

        /// <summary>
        /// Gets or sets the Project identifier.
        /// </summary>
        public string ProjectId { get; set; }
    }
}
