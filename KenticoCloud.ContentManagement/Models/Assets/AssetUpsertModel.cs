using System.Collections.Generic;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    public sealed class AssetUpsertModel
    {
        public FileReferenceModel FileReference { get; set; }
        
        public IEnumerable<AssetDescriptionsModel> Descriptions { get; set; }

    }
}
