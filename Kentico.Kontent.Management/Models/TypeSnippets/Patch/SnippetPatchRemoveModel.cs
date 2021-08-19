using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.TypeSnippets.Patch
{
    public class SnippetPatchRemoveModel : SnippetOperationBaseModel
    {
        public override string Op => "remove";
    }
}
