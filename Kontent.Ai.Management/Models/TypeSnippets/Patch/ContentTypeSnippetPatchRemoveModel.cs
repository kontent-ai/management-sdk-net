﻿namespace Kontent.Ai.Management.Models.TypeSnippets.Patch;

/// <summary>
/// Represents the remove operation.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-a-content-type-snippet
/// </summary>
public class ContentTypeSnippetPatchRemoveModel : ContentTypeSnippetOperationBaseModel
{
    /// <summary>
    /// Represents the remove operation.
    /// </summary>
    public override string Op => "remove";
}
