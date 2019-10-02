using System.Xml;
using CMS.DocumentEngine.Types.KenticoContrib;
using NUnit.Framework;

namespace Meeg.Kentico.ContentComponents.Tests
{
    [TestFixture]
    public class ContentComponentSerializerTests : ContentComponentUnitTests
    {
        [Test]
        public void Serialize_Null_ReturnsEmptyString()
        {
            var serializer = new ContentComponentSerializer();

            string actual = serializer.Serialize(null);
            string expected = string.Empty;

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Serialize_Component_ReturnsValidXml()
        {
            var serializer = new ContentComponentSerializer();

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
