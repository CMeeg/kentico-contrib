using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KenticoContrib;
using NUnit.Framework;

namespace Meeg.Kentico.ContentComponents.Cms.Tests
{
    [TestFixture]
    public class TreeNodeExtensionsTests : PageTypeComponentUnitTests
    {
        [Test]
        public void GetContentComponent_NonComponentField_ReturnsNull()
        {
            var page = TreeNode.New<Page>().With(node =>
            {
                node.DocumentName = "Fake page";
            });

            // `page.Metadata` uses the extension method we are testing
            PageMetadata actual = page.Metadata;

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void GetContentComponent_ComponentField_ReturnsComponent()
        {
            var serializer = new PageTypeComponentSerializer();

            var expected = TestData.PageMetadataComponent;

            var page = TreeNode.New<Page>().With(node =>
            {
                node.DocumentName = "Fake page";
                node.PageMetadata = serializer.Serialize(expected);
            });

            // `page.Metadata` uses the extension method we are testing
            PageMetadata actual = page.Metadata;

            Assert.Multiple(() =>
            {
                Assert.That(actual.DocumentPageTitle, Is.EqualTo(expected.DocumentPageTitle));
                Assert.That(actual.DocumentPageDescription, Is.EqualTo(expected.DocumentPageDescription));
                Assert.That(actual.OpenGraph.OpenGraphMetadataTitle, Is.EqualTo(expected.OpenGraph.OpenGraphMetadataTitle));
                Assert.That(actual.OpenGraph.OpenGraphMetadataDescription, Is.EqualTo(expected.OpenGraph.OpenGraphMetadataDescription));
            });
        }

        [Test]
        public void IsContentComponent_WithNewTreeNode_IsNotContentComponent()
        {
            var node = TreeNode.New<Page>();

            Assert.That(node.IsContentComponent(), Is.False);
        }
    }
}
