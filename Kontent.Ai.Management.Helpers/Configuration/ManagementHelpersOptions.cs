﻿namespace Kontent.Ai.Management.Helpers.Configuration;

/// <summary>
/// Keeps settings which are provided by customer or have default values, used in <see cref="EditLinkBuilder"/>.
/// </summary>
public class ManagementHelpersOptions
{
    /// <summary>
    /// Gets or sets the Admin URL. Optional, defaults to "https://app.kontent.ai/{0}".
    /// </summary>
    public string AdminUrl { get; set; } = "https://app.kontent.ai/{0}";

    /// <summary>
    /// Gets or sets the Project identifier.
    /// </summary>
    public string ProjectId { get; set; }
}
