using System.Xml;
using CMS.DocumentEngine.Types.KenticoContrib;
using NUnit.Framework;

namespace Meeg.Kentico.ContentComponents.Cms.Tests
{
    [TestFixture]
    public class PageTypeComponentSerializerTests : PageTypeComponentUnitTests
    {
        [Test]
        public void Serialize_Null_ReturnsEmptyString()
        {
            var serializer = new PageTypeComponentSerializer();

            string actual = serializer.Serialize(null);
            string expected = string.Empty;

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Serialize_Component_ReturnsValidXml()
        {
            var serializer = new PageTypeComponentSerializer();

            PageMetadata component = TestData.PageMetadataComponent;

            string componentXml = serializer.Serialize(component);

            try
            {
                var actual = new XmlDocument();
                actual.LoadXml(componentXml);

                Assert.Pass();
            }
            catch (XmlException e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
