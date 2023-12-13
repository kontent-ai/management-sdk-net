using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.Webhooks;

/// <summary>
/// Webhook health status.
/// </summary>
public enum WebhookHealthStatus
{
    /// <summary>
    /// Appears for newly created webhooks before any notification is sent.
    /// </summary>
    [EnumMember(Value = "unknown")]
    Unknown = 0,
    
    /// <summary>
    /// Appears for webhooks that have successfully delivered notifications.
    /// </summary>
    [EnumMember(Value = "working")]
    Working = 1,
    
    /// <summary>
    /// Appears for webhooks that have not been successful in delivering notifications. 
    /// </summary>
    [EnumMember(Value = "failing")]
    Failing = 2,
    
    /// <summary>
    /// Appears for webhooks where notification delivery has repeatedly failed for 7 days. 
    /// </summary>
    [EnumMember(Value = "dead")]
    Dead = 3
}