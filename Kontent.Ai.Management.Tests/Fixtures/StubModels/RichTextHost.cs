using Kontent.Ai.Management.Annotations;
using Kontent.Ai.Management.Models.Content;

namespace Kontent.Ai.Management.Tests.Fixtures.StubModels;

// Stub exercising the rich-text [AllowedTypes] component walk: a RichTextElement property whose
// embedded components' content types are constrained to `page` / `article`.
[KontentType("rich_text_host", "dddddddd-dddd-dddd-dddd-dddddddddddd")]
internal sealed record RichTextHost : IContentItem
{
    [KontentElement("body", "eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee")]
    [AllowedTypes("page", "article")]
    public RichTextElement? Body { get; init; }
}
