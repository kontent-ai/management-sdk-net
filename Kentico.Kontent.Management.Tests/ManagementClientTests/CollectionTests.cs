using System.Linq;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    partial class ManagementClientTests
    {
        [Fact]
        [Trait("Category", "Collections")]
        public async void ListCollections_ListsCollections()
        {
            var client = CreateManagementClient();

            var response = await client.ListCollectionsAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.Collections.Where(x => x.Codename == EXISTING_COLLECTION_CODENAME));
        }
    }
}
