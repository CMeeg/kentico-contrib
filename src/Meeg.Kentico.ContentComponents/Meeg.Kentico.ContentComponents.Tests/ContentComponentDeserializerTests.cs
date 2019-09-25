using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KenticoContrib;
using NUnit.Framework;

namespace Meeg.Kentico.ContentComponents.Tests
{
    [TestFixture]
    public class ContentComponentDeserializerTests : ContentComponentUnitTests
    {
        [TestCase(null)]
        [TestCase("")]
        public void Deserialize_NullOrEmptyXml_ReturnsNull(string componentXml)
        {
            var deserializer = new ContentComponentDeserializer();

            TreeNode actualUntyped = deserializer.Deserialize(Metadata.CLASS_NAME, componentXml);
            Metadata actualTyped = deserializer.Deserialize<Metadata>(componentXml);

            Assert.Multiple(() =>
            {
                Assert.That(actualUntyped, Is.Null);
                Assert.That(actualTyped, Is.Null);
            });
        }

        [Test]
        public void Deserialize_Component_RetainsSerializedValues()
        {
            var serializer = new ContentComponentSerializer();

            Metadata expected = TestData.MetadataComponent;

            string componentXml = serializer.Serialize(expected);

            var deserializer = new ContentComponentDeserializer();

            TreeNode actual = deserializer.Deserialize(Metadata.CLASS_NAME, componentXml);

            Assert.Multiple(() =>
            {
                Assert.That(actual.DocumentPageTitle, Is.EqualTo(expected.DocumentPageTitle));
                Assert.That(actual.DocumentPageDescription, Is.EqualTo(expected.DocumentPageDescription));
                Assert.That(actual.GetValue("OpenGraphTitle", string.Empty), Is.EqualTo(expected.OpenGraphTitle));
                Assert.That(actual.GetValue("OpenGraphDescription", string.Empty), Is.EqualTo(expected.OpenGraphDescription));
            });
        }

        [Test]
        public void Deserialize_ComponentOfType_RetainsSerializedValues()
        {
            var serializer = new ContentComponentSerializer();

            Metadata expected = TestData.MetadataComponent;

            string componentXml = serializer.Serialize(expected);

            var deserializer = new ContentComponentDeserializer();

            Metadata actual = deserializer.Deserialize<Metadata>(componentXml);

            Assert.Multiple(() =>
            {
                Assert.That(actual.DocumentPageTitle, Is.EqualTo(expected.DocumentPageTitle));
                Assert.That(actual.DocumentPageDescription, Is.EqualTo(expected.DocumentPageDescription));
                Assert.That(actual.OpenGraphTitle, Is.EqualTo(expected.OpenGraphTitle));
                Assert.That(actual.OpenGraphDescription, Is.EqualTo(expected.OpenGraphDescription));
            });
        }
    }
}
