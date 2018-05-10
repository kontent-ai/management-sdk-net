using System;

using KenticoCloud.ContentManagement.Helpers.Configuration;

using Xunit;

namespace KenticoCloud.ContentManagement.Helpers.Tests
{
    public class EditLinkBuilderTests
    {
        private EditLinkBuilder _underTest;

        public EditLinkBuilderTests()
        {
            _underTest = new EditLinkBuilder(new ContentManagementHelpersOptions {
                ProjectId = "14dc0cf8-6cc1-4f20-8e2e-0b5edea89e43"
            });
        }

        [Fact]
        public void GetEditItemUrl_ValidInput_ReturnsValidUrl()
        {
            var itemId = Guid.Parse("1cdaa8ef-cb2b-4f82-82e9-45467b2e01b9");
            var variantCodename = "some-Variant-Codename";

            var result = _underTest.GetEditItemUrl(variantCodename, itemId);

            var expected = "https://app.kenticocloud.com/goto/edit-item/item/1cdaa8ef-cb2b-4f82-82e9-45467b2e01b9/variant-codename/some-Variant-Codename/project/14dc0cf8-6cc1-4f20-8e2e-0b5edea89e43";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetEditItemUrl_EmptyVariantCodename_ThrowsException()
        {
            var itemId = Guid.Parse("1cdaa8ef-cb2b-4f82-82e9-45467b2e01b9");

            Assert.Throws<ArgumentException>(() => _underTest.GetEditItemUrl(string.Empty, itemId));
        }

        [Fact]
        public void EditLinkBuilder_MissingEditAppEndpoint_ThrowsException()
        {
            var noEditAppEndpointOptions = new ContentManagementHelpersOptions
            {
                EditAppEndpoint = string.Empty
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
