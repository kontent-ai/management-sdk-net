namespace KenticoCloud.ContentManagement.Helpers
{
    /// <summary>
    /// Creates URL for redirection to editing of content items.
    /// </summary>
    public interface IEditLinkBuilder
    {
        /// <summary>
        /// Gets URL to edit page of specified content item.
        /// </summary>
        /// <param name="language">Language codename.</param>
        /// <param name="itemId">Identifier of content item.</param>
        /// <returns>URL to edit page of specified item.</returns>
        string BuildEditItemUrl(string language, string itemId);
    }
}