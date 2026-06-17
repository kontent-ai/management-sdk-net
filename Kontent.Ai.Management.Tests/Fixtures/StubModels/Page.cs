using Kontent.Ai.Management.Annotations;

namespace Kontent.Ai.Management.Tests.Fixtures.StubModels;

// Minimal stub used by the rich-text [AllowedTypes] tests as a component content type that IS permitted.
[KontentType("page", "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")]
internal sealed record Page : IElementsModel;
