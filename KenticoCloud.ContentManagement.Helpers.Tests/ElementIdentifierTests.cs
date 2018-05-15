using System;

using KenticoCloud.ContentManagement.Helpers.Models;

using Xunit;

namespace KenticoCloud.ContentManagement.Helpers.Tests
{
    public class ElementIdentifierTests
    {
        private readonly string _itemId = "1cdaa8ef-cb2b-4f82-82e9-45467b2e01b9";
        private readonly string _elementId = "2e6bf402-30d3-4fee-bcd0-801711a3b8a1";

        [Fact]
        public void ElementIdentifier_EmptyItemId_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new ElementIdentifier(string.Empty, _elementId));
        }

        [Fact]
        public void ElementIdentifier_EmptyElementId_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new ElementIdentifier(_itemId, string.Empty));
        }

        [Fact]
        public void ElementIdentifier_AllDataProvided_GetsInstance()
        {
            var result = new ElementIdentifier(_itemId, _elementId);
            Assert.Equal(_itemId, result.ItemId);
            Assert.Equal(_elementId, result.ElementCodename);
        }
    }
}
