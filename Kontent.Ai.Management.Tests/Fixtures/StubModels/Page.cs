using Kontent.Ai.Management.Annotations;

namespace Kontent.Ai.Management.Tests.Fixtures.StubModels;

// Minimal stub used by [AllowedTypes] tests as an item permitted in `Article.Related`.
[KontentType("page", "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")]
internal sealed record Page : IContentItem;
