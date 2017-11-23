namespace KenticoCloud.ContentManagement
{
    public class ContentManagementOptions
    {
        /// <summary>
        /// Gets or sets the Production endpoint address.
        /// </summary>
        public string Endpoint { get; set; } = "https://manage.kenticocloud.com/{0}";

        /// <summary>
        /// Gets or sets the Project identifier.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the Preview API key.
        /// </summary>
        public string ApiKey { get; set; }
    }
}
