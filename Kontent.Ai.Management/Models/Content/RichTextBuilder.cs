using Kontent.Ai.Management.Annotations;
using System.Net;
using System.Reflection;

namespace Kontent.Ai.Management.Models.Content;

/// <summary>
/// Fluent authoring helper for <see cref="RichTextElement"/>. Designed for migration scripts and similar code-authored
/// content: lets the consumer compose the rich-text HTML body with string interpolation while the builder records
/// embedded components and emits the matching <c>&lt;object data-id="…"&gt;</c> placeholders — same GUID on both sides,
/// hidden from the consumer.
/// </summary>
/// <remarks>
/// <para>
/// Usage:
/// <code>
/// var rt = new RichTextBuilder();
/// var element = rt.Build($"""
///     &lt;p&gt;See {rt.ItemLink(Reference.ByCodename("intro"), "the intro")}.&lt;/p&gt;
///     {rt.Component(new Callout { Type = [CalloutType.Warning], Content = … })}
///     """);
/// </code>
/// Interpolation evaluates left-to-right, so <see cref="Component"/> calls record their components before <see cref="Build(string)"/> closes.
/// </para>
/// <para>
/// <see cref="Build(string)"/> snapshots the recorded components and resets the builder, so the same instance can be
/// reused for the next element. Builders nested inside a component's own rich-text body are independent — each owns
/// its own component list.
/// </para>
/// <para>
/// The HTML body is passed through verbatim. The builder does not sanitize or validate the markup; it's intended for
/// trusted, code-authored content. Helper return values escape attribute values and link text via HTML encoding.
/// </para>
/// </remarks>
public sealed class RichTextBuilder
{
    private readonly List<Component> _components = [];

    /// <summary>
    /// Records <paramref name="item"/> as an inline component and returns the matching
    /// <c>&lt;object data-type="component" data-id="…"&gt;</c> placeholder. The GUID is generated internally
    /// and shared between the placeholder and the recorded <see cref="Component.Id"/>; consumers do not see it.
    /// </summary>
    public string Component(IElementsModel item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (item.GetType().GetCustomAttribute<KontentTypeAttribute>() is null)
        {
            throw new ArgumentException(
                $"Type '{item.GetType().FullName}' lacks [KontentType] and cannot be embedded as a rich-text component.",
                nameof(item));
        }

        var id = Guid.NewGuid();
        _components.Add(new Models.Content.Component { Id = id, Content = item });
        return $"""<object type="application/kenticocloud" data-type="component" data-id="{id}"></object>""";
    }

    /// <summary>
    /// Returns an <c>&lt;object data-type="item" data-…="…"&gt;</c> placeholder referencing an existing content item.
    /// The attribute (<c>data-id</c> / <c>data-codename</c> / <c>data-external-id</c>) is picked from
    /// <paramref name="reference"/> in id-first priority.
    /// </summary>
    public string LinkedItem(Reference reference)
    {
        ArgumentNullException.ThrowIfNull(reference);
        var (suffix, value) = PickReferenceIdentifier(reference);
        return $"""<object type="application/kenticocloud" data-type="item" data-{suffix}="{value}"></object>""";
    }

    /// <summary>
    /// Returns an <c>&lt;a data-item-…="…"&gt;</c> inline hyperlink referencing an existing content item.
    /// <paramref name="linkText"/> is HTML-encoded; the identifier attribute follows the same id-first priority as <see cref="LinkedItem"/>.
    /// </summary>
    public string ItemLink(Reference reference, string linkText)
    {
        ArgumentNullException.ThrowIfNull(reference);
        ArgumentNullException.ThrowIfNull(linkText);
        var (suffix, value) = PickReferenceIdentifier(reference);
        return $"""<a data-item-{suffix}="{value}">{WebUtility.HtmlEncode(linkText)}</a>""";
    }

    /// <summary>
    /// Returns a <c>&lt;figure data-asset-…="…"&gt;</c> placeholder referencing an asset. The identifier attribute
    /// follows the same id-first priority as <see cref="LinkedItem"/>.
    /// </summary>
    public string Asset(AssetReference asset)
    {
        ArgumentNullException.ThrowIfNull(asset);
        var (suffix, value) = PickAssetIdentifier(asset);
        return $"""<figure data-asset-{suffix}="{value}"></figure>""";
    }

    /// <summary>
    /// Closes the builder. Returns a <see cref="RichTextElement"/> whose <see cref="RichTextElement.Value"/> is
    /// <paramref name="html"/> verbatim and <see cref="RichTextElement.Components"/> is the recorded components list
    /// (<c>null</c> if none). The builder's internal component list is cleared so the same instance can be reused.
    /// </summary>
    public RichTextElement Build(string html)
    {
        ArgumentNullException.ThrowIfNull(html);
        var components = _components.Count == 0 ? null : _components.ToArray();
        _components.Clear();
        return new RichTextElement { Value = html, Components = components };
    }

    private static (string Suffix, string Value) PickReferenceIdentifier(Reference reference)
    {
        if (reference.Id is Guid id) return ("id", id.ToString());
        if (!string.IsNullOrEmpty(reference.Codename)) return ("codename", WebUtility.HtmlEncode(reference.Codename));
        if (!string.IsNullOrEmpty(reference.ExternalId)) return ("external-id", WebUtility.HtmlEncode(reference.ExternalId));

        throw new ArgumentException(
            "Reference must carry at least one of Id, Codename, or ExternalId to be emitted as inline rich-text markup.",
            nameof(reference));
    }

    private static (string Suffix, string Value) PickAssetIdentifier(AssetReference asset)
    {
        if (asset.Id is Guid id) return ("id", id.ToString());
        if (!string.IsNullOrEmpty(asset.Codename)) return ("codename", WebUtility.HtmlEncode(asset.Codename));
        if (!string.IsNullOrEmpty(asset.ExternalId)) return ("external-id", WebUtility.HtmlEncode(asset.ExternalId));

        throw new ArgumentException(
            "AssetReference must carry at least one of Id, Codename, or ExternalId to be emitted as inline rich-text markup.",
            nameof(asset));
    }
}
