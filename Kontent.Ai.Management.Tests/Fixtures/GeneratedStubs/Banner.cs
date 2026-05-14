using Kontent.Ai.Management;
using Kontent.Ai.Management.Annotations;

namespace Kontent.Ai.Management.Tests.Fixtures.GeneratedStubs;

// Minimal stub used by [AllowedTypes] tests as an item that is NOT permitted in `Article.Related`.
[KontentContentType("banner", "cccccccc-cccc-cccc-cccc-cccccccccccc")]
internal sealed record Banner : IContentItem;
