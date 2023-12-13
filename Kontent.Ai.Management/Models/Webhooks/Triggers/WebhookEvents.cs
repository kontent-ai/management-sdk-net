using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers;

/// <summary>
/// Specifies whether all available events can trigger the webhook or only the specified ones.
/// </summary>
public enum WebhookEvents
{
    /// <summary>
    /// All available events trigger the webhook. 
    /// </summary>
    [EnumMember(Value = "all")]
    All,
    
    /// <summary>
    /// Only a specific subset of events triggers the webhook.
    /// </summary>
    [EnumMember(Value = "specific")]
    Specific
}