using System.Collections.Generic;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    public class GetAssetsResponseModel
    {
        public IEnumerable<AssetModel> Assets { get; set; }
    }
}
