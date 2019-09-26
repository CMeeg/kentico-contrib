using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KenticoContrib;
using NUnit.Framework;

namespace Meeg.Kentico.ContentComponents.Tests
{
    [TestFixture]
    public class TreeNodeExtensionsTests : ContentComponentUnitTests
    {
        [Test]
        public void GetContentComponent_NonComponentField_ReturnsNull()
        {
            var page = TreeNode.New<Page>();
            page.DocumentName = "Fake page";

            // `page.Metadata` uses the extension method we are testing
            Metadata actual = page.Metadata;

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void GetContentComponent_ComponentField_ReturnsComponent()
        {
            var serializer = new ContentComponentSerializer();

            var expected = TestData.MetadataComponent;

            var page = TreeNode.New<Page>();
            page.DocumentName = "Fake page";
            page.PageMetadata = serializer.Serialize(expected);

            // `page.Metadata` uses the extension method we are testing
            Metadata actual = page.Metadata;

            Assert.Multiple(() =>
            {
                Assert.That(actual.DocumentPageTitle, Is.EqualTo(expected.DocumentPageTitle));
                Assert.That(actual.DocumentPageDescription, Is.EqualTo(expected.DocumentPageDescription));
                Assert.That(actual.MetadataOpenGraphTitle, Is.EqualTo(expected.MetadataOpenGraphTitle));
                Assert.That(actual.MetadataOpenGraphDescription, Is.EqualTo(expected.MetadataOpenGraphDescription));
            });
        }
    }
}
