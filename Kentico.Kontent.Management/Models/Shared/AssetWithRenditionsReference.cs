using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Shared;

public sealed class AssetWithRenditionsReference
{
    private readonly IList<Reference> _renditions;

    private readonly Reference _assetReference;

    [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Guid? Id => _assetReference.Id;

    [JsonProperty("codename", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Codename => _assetReference.Codename;

    [JsonProperty("external_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string ExternalId => _assetReference.ExternalId;

    [JsonProperty("renditions", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public IEnumerable<Reference> Renditions => _renditions;

    public AssetWithRenditionsReference(Reference assetReference, Reference renditionReference)
        : this(assetReference, new[] { renditionReference })
    {
    }

    public AssetWithRenditionsReference(Reference assetReference, IEnumerable<Reference> renditionReferences = null)
    {
        _assetReference = assetReference;
        _renditions = renditionReferences?.ToList() ?? new List<Reference>();
    }
}