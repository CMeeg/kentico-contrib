using CMS.DocumentEngine.Types.KenticoContrib;
using NUnit.Framework;

namespace Meeg.Kentico.ContentComponents.Cms.Tests
{
    [TestFixture]
    public class PageTypeContentComponentFactoryTests : PageTypeComponentUnitTests
    {
        [Test]
        public void Create_WithClassName_CreatesComponent()
        {
            var nodeFactory = new PageTypeContentComponentFactory();
            var node = nodeFactory.Create(ContentComponentTest.CLASS_NAME);

            Assert.That(node.IsContentComponent(), Is.True);
        }

        [Test]
        public void Create_WithPageType_CreatesComponent()
        {
            var nodeFactory = new PageTypeContentComponentFactory();
            var node = nodeFactory.Create<ContentComponentTest>();

            Assert.That(node.IsContentComponent(), Is.True);
        }
    }
}
