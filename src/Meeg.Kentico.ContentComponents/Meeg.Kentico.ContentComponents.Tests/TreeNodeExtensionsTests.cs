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

            // `page.MetadataComponent` uses the extension method we are testing
            Metadata actual = page.MetadataComponent;

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void GetContentComponent_ComponentField_ReturnsComponent()
        {
            var serializer = new ContentComponentSerializer();

            var expected = TestData.MetadataComponent;

            var page = TreeNode.New<Page>();
            page.DocumentName = "Fake page";
            page.Metadata = serializer.Serialize(expected);

            // `page.MetadataComponent` uses the extension method we are testing
            Metadata actual = page.MetadataComponent;

            Assert.Multiple(() =>
            {
                Assert.That(actual.PageTitle, Is.EqualTo(expected.PageTitle));
                Assert.That(actual.PageDescription, Is.EqualTo(expected.PageDescription));
                Assert.That(actual.OpenGraphTitle, Is.EqualTo(expected.OpenGraphTitle));
                Assert.That(actual.OpenGraphDescription, Is.EqualTo(expected.OpenGraphDescription));
            });
        }
    }
}
