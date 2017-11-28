using System.Collections.Generic;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    public class AssetUpsertModel
    {
        public FileReferenceModel FileReference { get; set; }
        
        public IEnumerable<AssetDescriptionsModel> Descriptions { get; set; }

    }
}
