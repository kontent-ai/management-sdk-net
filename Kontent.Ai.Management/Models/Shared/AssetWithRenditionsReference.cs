using System;
using System.Collections.Generic;
using System.Linq;
using Kontent.Ai.Management.Modules.ActionInvoker;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Shared;

/// <summary>
/// Represents identifier of asset with renditions.
/// </summary>
[JsonConverter(typeof(AssetWithRenditionsReferenceConverter))]
public class AssetWithRenditionsReference
{
    private readonly IList<Reference> _renditions;

    private readonly Reference _assetReference;

    /// <summary>
    /// Gets the id of the asset identifier.
    /// </summary>
    [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Guid? Id => _assetReference.Id;

    /// <summary>
    /// Gets the codename of the asset identifier.
    /// </summary>
    [JsonProperty("codename", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Codename => _assetReference.Codename;

    /// <summary>
    /// Gets the external id of the asset identifier.
    /// </summary>
    [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string ExternalId => _assetReference.ExternalId;

    /// <summary>
    /// Gets identifiers of linked renditions.
    /// </summary>
    [JsonProperty("renditions", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public IEnumerable<Reference> Renditions => _renditions;

    /// <summary>
    /// Creates instance from asset reference and a single rendition reference.
    /// </summary>
    /// <param name="assetReference">Asset reference.</param>
    /// <param name="renditionReference">Rendition reference.</param>
    public AssetWithRenditionsReference(Reference assetReference, Reference renditionReference)
        : this(assetReference, new[] { renditionReference })
    {
    }

    /// <summary>
    /// Creates instance from asset reference and a multiple rendition references (if present).
    /// </summary>
    /// <param name="assetReference">Asset reference.</param>
    /// <param name="renditionReferences">Rendition references.</param>
    public AssetWithRenditionsReference(Reference assetReference, IEnumerable<Reference> renditionReferences = null)
    {
        _assetReference = assetReference;
        _renditions = renditionReferences?.ToList() ?? new List<Reference>();
    }
}
