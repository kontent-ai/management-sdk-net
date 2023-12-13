using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers;

/// <summary>
/// Represents the delivery slot.
/// </summary>
public enum DeliverySlot
{
    /// <summary>
    /// Published data.
    /// </summary>
    [EnumMember(Value = "published")]
    Published,
    
    /// <summary>
    /// Preview data.
    /// </summary>
    [EnumMember(Value = "preview")]
    Preview
}