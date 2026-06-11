using Kontent.Ai.Management.Annotations;

namespace Kontent.Ai.Management.Tests.Fixtures.StubModels;

// Minimal stub used by [AllowedTypes] tests as an item that is NOT permitted in `Article.Related`.
[KontentType("banner", "cccccccc-cccc-cccc-cccc-cccccccccccc")]
internal sealed record Banner : IContentItem;
