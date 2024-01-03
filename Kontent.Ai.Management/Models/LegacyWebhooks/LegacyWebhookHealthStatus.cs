using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.LegacyWebhooks;

/// <summary>
/// Webhook health status.
/// </summary>
public enum LegacyWebhookHealthStatus
{
    /// <summary>
    /// Appears for newly created webhooks before any notification has been sent.
    /// </summary>
    [EnumMember(Value = "unknown")]
    Unknown = 0,
    
    /// <summary>
    /// Appears for webhooks that have properly delivered notifications.
    /// </summary>
    [EnumMember(Value = "working")]
    Working = 1,
    
    /// <summary>
    /// Appears for webhooks that have not been delivered properly.
    /// </summary>
    [EnumMember(Value = "failing")]
    Failing = 2,
    
    /// <summary>
    /// Appears for webhooks where delivery has repeatedly failed, and no notifications have been accepted for 7 days.
    /// </summary>
    [EnumMember(Value = "dead")]
    Dead = 3
}