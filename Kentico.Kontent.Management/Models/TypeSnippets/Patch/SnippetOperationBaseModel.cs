using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.TypeSnippets.Patch
{
    public abstract class SnippetOperationBaseModel
    {
        [JsonProperty("op", Required = Required.Always)]
        public abstract string Op { get; }

        [JsonProperty("path", Required = Required.Always)]
        public string Path { get; set; }
    }
}
