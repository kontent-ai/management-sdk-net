using Kontent.Ai.Management.Annotations;

namespace Kontent.Ai.Management.Tests.Fixtures.StubModels;

// Minimal stub used by the rich-text [AllowedTypes] tests as a component content type that is NOT permitted.
[KontentType("banner", "cccccccc-cccc-cccc-cccc-cccccccccccc")]
internal sealed record Banner : IElementsModel;
