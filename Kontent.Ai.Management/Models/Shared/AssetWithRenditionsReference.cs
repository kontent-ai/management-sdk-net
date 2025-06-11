using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Modules.ActionInvoker;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kontent.Ai.Management.Models.Shared;

/// <summary>
/// Represents identifier of asset with renditions.
/// </summary>
[JsonConverter(typeof(AssetWithRenditionsReferenceConverter))]
public sealed class AssetWithRenditionsReference
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
        : this(assetReference, [renditionReference])
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
        _renditions = renditionReferences?.ToList() ?? [];
    }

    /// <summary>
    /// Transforms the dynamic object to the <see cref="AssetWithRenditionsReference"/>
    /// </summary>
    public static AssetWithRenditionsReference FromDynamic(dynamic source)
    {
        try
        {
            var assetReference = Reference.FromDynamic(source);
            
            IEnumerable<Reference> renditions = null;
            if (DynamicExtensions.HasProperty(source, "renditions") && source.renditions != null)
            {
                renditions = (source.renditions as IEnumerable<dynamic>)?.Select(Reference.FromDynamic);
            }

            return new AssetWithRenditionsReference(assetReference, renditions);
        }
        catch (Exception exception)
        {
            throw new DataMisalignedException(
                "Object could not be converted to the strongly-typed AssetWithRenditionsReference. Please check if it has expected properties with expected type",
                exception);
        }
    }

    /// <summary>
    /// Transforms the <see cref="AssetWithRenditionsReference"/> to the dynamic object.
    /// </summary>
    public dynamic ToDynamic()
    {
        if (Id != null)
        {
            return new
            {
                id = Id,
                renditions = (_renditions ?? []).Select(r => r.ToDynamic())
            };
        }
        
        if (Codename != null)
        {
            return new
            {
                codename = Codename,
                renditions = (_renditions ?? []).Select(r => r.ToDynamic())
            };
        }
        
        if (ExternalId != null)
        {
            return new
            {
                external_id = ExternalId,
                renditions = (_renditions ?? []).Select(r => r.ToDynamic())
            };
        }

        return _assetReference.ToDynamic();
    }
}