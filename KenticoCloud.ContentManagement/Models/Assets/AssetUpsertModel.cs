using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    public class AssetUpsertModel
    {
        public FileReferenceModel FileReference { get; set; }
        
        public IEnumerable<AssetDescriptionsModel> Descriptions { get; set; }

    }
}
