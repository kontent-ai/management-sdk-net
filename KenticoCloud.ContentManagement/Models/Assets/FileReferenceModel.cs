using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    public sealed class FileReferenceModel
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty("type", Required = Required.Always)]
        public FileReferenceTypeEnum Type { get; set; }
    }
}
