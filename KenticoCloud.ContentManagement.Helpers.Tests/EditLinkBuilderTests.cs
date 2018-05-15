using System;

using KenticoCloud.ContentManagement.Helpers.Configuration;
using KenticoCloud.ContentManagement.Helpers.Models;

using Xunit;

namespace KenticoCloud.ContentManagement.Helpers.Tests
{
    public class EditLinkBuilderTests
    {
        private readonly string _itemId = "1cdaa8ef-cb2b-4f82-82e9-45467b2e01b9";
        private readonly string _language = "some-Language-Codename";
        private readonly string _projectId = "14dc0cf8-6cc1-4f20-8e2e-0b5edea89e43";

        private ContentManagementHelpersOptions DefaultOptions => new ContentManagementHelpersOptions
        {
            ProjectId = _projectId
        };

        [Fact]
        public void BuildEditItemUrl_ValidInput_ReturnsValidUrl()
        {
            var underTest = new EditLinkBuilder(DefaultOptions);
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
        public void BuildEditItemUrl_EmptyLanguage_ThrowsException()
        {
            var underTest = new EditLinkBuilder(DefaultOptions);
            Assert.Throws<ArgumentException>(() => underTest.BuildEditItemUrl(string.Empty, _itemId));
        }

        [Fact]
        public void BuildEditItemUrl_EmptyItemId_ThrowsException()
        {
            var underTest = new EditLinkBuilder(DefaultOptions);
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

        [Fact]
        public void BuildEditItemUrl_SingleElement_ReturnsValidUrl()
        {
            var elementIdentifier = new ElementIdentifier(_itemId, "single-element-Codename");
            
            var underTest = new EditLinkBuilder(DefaultOptions);
            var result = underTest.BuildEditItemUrl(_language, elementIdentifier);

            var expected = "https://app.kenticocloud.com/goto/edit-item/project/14dc0cf8-6cc1-4f20-8e2e-0b5edea89e43/variant-codename/some-Language-Codename/item/1cdaa8ef-cb2b-4f82-82e9-45467b2e01b9/element/single-element-Codename";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void BuildEditItemUrl_MultipleElements_ReturnsValidUrl()
        {
            var elements = new ElementIdentifier[]
            {
                new ElementIdentifier("76c06b74-bae9-4732-b629-1a59395e893d", "some-Element-Codename-1"),
                new ElementIdentifier("326c63aa-ae71-40b7-a6a8-56455b0b9751", "some-Element-Codename-2"),
                new ElementIdentifier("ffcd0436-8274-40ee-aaae-86fee1966fce", "some-Element-Codename-3"),
                new ElementIdentifier("d31d27cf-ddf6-4040-ab67-2f70edc0d46b", "some-Element-Codename-4"),
            };

            var underTest = new EditLinkBuilder(DefaultOptions);
            var result = underTest.BuildEditItemUrl(_language, elements);

            var expected = "https://app.kenticocloud.com/goto/edit-item/" +
                "project/14dc0cf8-6cc1-4f20-8e2e-0b5edea89e43/variant-codename/some-Language-Codename/" +
                "item/76c06b74-bae9-4732-b629-1a59395e893d/element/some-Element-Codename-1/" +
                "item/326c63aa-ae71-40b7-a6a8-56455b0b9751/element/some-Element-Codename-2/" +
                "item/ffcd0436-8274-40ee-aaae-86fee1966fce/element/some-Element-Codename-3/" +
                "item/d31d27cf-ddf6-4040-ab67-2f70edc0d46b/element/some-Element-Codename-4";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void BuildEditItemUrl_EmptyLanguageForElements_ThrowsException()
        {
            var underTest = new EditLinkBuilder(DefaultOptions);
            Assert.Throws<ArgumentException>(() => underTest.BuildEditItemUrl(string.Empty, 
                new ElementIdentifier("76c06b74-bae9-4732-b629-1a59395e893d", "single-element-Codename")));
        }

        [Fact]
        public void BuildEditItemUrl_NoElements_ThrowsException()
        {
            var underTest = new EditLinkBuilder(DefaultOptions);
            Assert.Throws<ArgumentException>(() => underTest.BuildEditItemUrl(_language));
        }

        [Fact]
        public void BuildEditItemUrl_EmptyElements_ThrowsException()
        {
            var elements = new ElementIdentifier[0];
            var underTest = new EditLinkBuilder(DefaultOptions);
            Assert.Throws<ArgumentException>(() => underTest.BuildEditItemUrl(_language, elements));
        }
    }
}
