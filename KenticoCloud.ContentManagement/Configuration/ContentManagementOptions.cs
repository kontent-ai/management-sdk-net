using KenticoCloud.ContentManagement.Modules.ModelBuilders;

namespace KenticoCloud.ContentManagement
{
    /// <summary>
    /// Keeps settings which are provided by customer or have default values, used in <see cref="ContentManagementClient"/>.
    /// </summary>
    public class ContentManagementOptions
    {
        /// <summary>
        /// Gets or sets the Production endpoint address. Optional, defaults to "https://manage.kenticocloud.com/{0}".
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

        /// <summary>
        /// Gets or sets the Model provider for strongly typed models
        /// </summary>
        public IModelProvider ModelProvider { get; set; }

        /// <summary>
        /// Gets or sets whether HTTP requests will use a retry logic.
        /// </summary>
        public bool EnableResilienceLogic { get; set; } = true;

        /// <summary>
        /// Gets or sets the maximum retry attempts.
        /// </summary>
        public int MaxRetryAttempts { get; set; } = 5;
    }
}
