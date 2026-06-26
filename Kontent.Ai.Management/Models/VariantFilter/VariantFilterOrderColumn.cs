namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Column to order the items-with-variants filter results by.
/// </summary>
public enum VariantFilterOrderColumn
{
    /// <summary>
    /// Order by item name.
    /// </summary>
    [EnumMember(Value = "name")]
    Name,

    /// <summary>
    /// Order by the variant's due date.
    /// </summary>
    [EnumMember(Value = "due_date")]
    DueDate,

    /// <summary>
    /// Order by the variant's last-modified timestamp.
    /// </summary>
    [EnumMember(Value = "last_modified")]
    LastModified
}
