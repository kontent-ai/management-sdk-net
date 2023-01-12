﻿using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.PreviewConfiguration;

/// <summary>
/// Represents content type preview URL patterns response model.
/// </summary>
public class TypePreviewUrlPatternResponseModel
{
    /// <summary>
    /// Gets or sets the content type reference.
    /// </summary>
    [JsonProperty("content_type")]
    public Reference ContentType { get; set; }

    /// <summary>
    /// Gets or sets content type's url patterns.
    /// </summary>
    [JsonProperty("url_patterns")]
    public IReadOnlyCollection<PreviewUrlPatternResponseModel> UrlPatterns { get; set; }
}