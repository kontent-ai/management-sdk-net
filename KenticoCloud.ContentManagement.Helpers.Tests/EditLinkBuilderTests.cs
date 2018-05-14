using System;

using KenticoCloud.ContentManagement.Helpers.Configuration;

using Xunit;

namespace KenticoCloud.ContentManagement.Helpers.Tests
{
    public class EditLinkBuilderTests
    {
        private readonly string _itemId = "1cdaa8ef-cb2b-4f82-82e9-45467b2e01b9";
        private readonly string _language = "some-Language-Codename";

        [Fact]
        public void BuildEditItemUrl_ValidInput_ReturnsValidUrl()
        {
            var options = new ContentManagementHelpersOptions
            {
                ProjectId = "14dc0cf8-6cc1-4f20-8e2e-0b5edea89e43"
            };

            var underTest = new EditLinkBuilder(options);
            var result = underTest.BuildEditItemUrl(_language, _itemId);

            var expected = "https://app.kenticocloud.com/goto/edit-item/project/14dc0cf8-6cc1-4f20-8e2e-0b5edea89e43/variant-codename/some-Language-Codename/item/1cdaa8ef-cb2b-4f82-82e9-45467b2e01b9";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void BuildEditItemUrl_AdminUrlOtherThanDefault_ReturnsCorrectUrl()
        {
            var options = new ContentManagementHelpersOptions
            {
                AdminUrl = "https://someOther.url/{0}",
                ProjectId = "34998683-4dd6-441c-b4cb-57493cafcaf0",
            };

            var underTest = new EditLinkBuilder(options);
            var result = underTest.BuildEditItemUrl(_language, _itemId);

            var expected = "https://someOther.url/goto/edit-item/project/34998683-4dd6-441c-b4cb-57493cafcaf0/variant-codename/some-Language-Codename/item/1cdaa8ef-cb2b-4f82-82e9-45467b2e01b9";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void BuildEditItemUrl_EmptyVariantCodename_ThrowsException()
        {
            var options = new ContentManagementHelpersOptions
            {
                ProjectId = "14dc0cf8-6cc1-4f20-8e2e-0b5edea89e43"
            };

            var underTest = new EditLinkBuilder(options);
            Assert.Throws<ArgumentException>(() => underTest.BuildEditItemUrl(string.Empty, _itemId));
        }

        [Fact]
        public void BuildEditItemUrl_EmptyItemId_ThrowsException()
        {
            var options = new ContentManagementHelpersOptions
            {
                ProjectId = "14dc0cf8-6cc1-4f20-8e2e-0b5edea89e43"
            };

            var underTest = new EditLinkBuilder(options);
            Assert.Throws<ArgumentException>(() => underTest.BuildEditItemUrl(_language, string.Empty));
        }

        [Fact]
        public void EditLinkBuilder_MissingEditAppEndpoint_ThrowsException()
        {
            var noEditAppEndpointOptions = new ContentManagementHelpersOptions
            {
                AdminUrl = string.Empty
            };

            Assert.Throws<ArgumentException>(() => new EditLinkBuilder(noEditAppEndpointOptions));
        }

        [Fact]
        public void EditLinkBuilder_MissingProjectId_ThrowsException()
        {
            var noProjectIdOptions = new ContentManagementHelpersOptions
            {
                ProjectId = string.Empty
            };

            Assert.Throws<ArgumentException>(() => new EditLinkBuilder(noProjectIdOptions));
        }

        [Fact]
        public void EditLinkBuilder_ProjectIdBadFormat_ThrowsException()
        {
            var badFormatProjectIdOptions = new ContentManagementHelpersOptions
            {
                ProjectId = "aa-bb-cc"
            };

            Assert.Throws<ArgumentException>(() => new EditLinkBuilder(badFormatProjectIdOptions));
        }   
    }
}
