using Kontent.Ai.Management.Annotations;

namespace Kontent.Ai.Management.Tests.Fixtures.StubModels;

// Minimal stub used by the rich-text component tests as an embedded component content type.
[KontentType("page", "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")]
internal sealed record Page : IElementsModel;
