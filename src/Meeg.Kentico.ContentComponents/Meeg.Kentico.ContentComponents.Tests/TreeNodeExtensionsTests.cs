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

            Metadata actual = page.GetContentComponent<Page, Metadata>(node => node.DocumentName);

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

            Metadata actual = page.GetContentComponent<Page, Metadata>(node => node.PageMetadata);

            Assert.Multiple(() =>
            {
                Assert.That(actual.PageTitle, Is.EqualTo(expected.PageTitle));
                Assert.That(actual.PageDescription, Is.EqualTo(expected.PageDescription));
                Assert.That(actual.MetadataOpenGraphTitle, Is.EqualTo(expected.MetadataOpenGraphTitle));
                Assert.That(actual.MetadataOpenGraphDescription, Is.EqualTo(expected.MetadataOpenGraphDescription));
            });
        }
    }
}
