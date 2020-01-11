using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KenticoContrib;
using NUnit.Framework;

namespace Meeg.Kentico.ContentComponents.Cms.Tests
{
    [TestFixture]
    public class PageTypeComponentDeserializerTests : PageTypeComponentUnitTests
    {
        [TestCase(null)]
        [TestCase("")]
        public void Deserialize_NullOrEmptyXml_ReturnsNull(string componentXml)
        {
            var deserializer = new PageTypeComponentDeserializer();

            TreeNode actualUntyped = deserializer.Deserialize(PageMetadata.CLASS_NAME, componentXml);
            PageMetadata actualTyped = deserializer.Deserialize<PageMetadata>(componentXml);

            Assert.Multiple(() =>
            {
                Assert.That(actualUntyped, Is.Null);
                Assert.That(actualTyped, Is.Null);
            });
        }

        [Test]
        public void Deserialize_Component_RetainsSerializedValues()
        {
            var serializer = new PageTypeComponentSerializer();

            PageMetadata expected = TestData.PageMetadataComponent;

            string componentXml = serializer.Serialize(expected);

            var deserializer = new PageTypeComponentDeserializer();

            TreeNode actual = deserializer.Deserialize(PageMetadata.CLASS_NAME, componentXml);

            Assert.Multiple(() =>
            {
                Assert.That(actual.DocumentPageTitle, Is.EqualTo(expected.DocumentPageTitle));
                Assert.That(actual.DocumentPageDescription, Is.EqualTo(expected.DocumentPageDescription));
            });
        }

        [Test]
        public void Deserialize_ComponentOfType_RetainsSerializedValues()
        {
            var serializer = new PageTypeComponentSerializer();

            PageMetadata expected = TestData.PageMetadataComponent;

            string componentXml = serializer.Serialize(expected);

            var deserializer = new PageTypeComponentDeserializer();

            PageMetadata actual = deserializer.Deserialize<PageMetadata>(componentXml);

            Assert.Multiple(() =>
            {
                Assert.That(actual.DocumentPageTitle, Is.EqualTo(expected.DocumentPageTitle));
                Assert.That(actual.DocumentPageDescription, Is.EqualTo(expected.DocumentPageDescription));
            });
        }

        [Test]
        public void Deserialize_NestedComponent_RetainsSerializedValues()
        {
            var serializer = new PageTypeComponentSerializer();

            PageMetadata metadata = TestData.PageMetadataComponent;
            OpenGraphMetadata expected = metadata.OpenGraph;

            string componentXml = serializer.Serialize(metadata);

            var deserializer = new PageTypeComponentDeserializer();

            TreeNode actual = deserializer.Deserialize(PageMetadata.CLASS_NAME, componentXml)
                .GetPageTypeComponent(OpenGraphMetadata.CLASS_NAME, nameof(PageMetadata.PageMetadataOpenGraph));

            Assert.Multiple(() =>
            {
                Assert.That(actual.GetStringValue(nameof(OpenGraphMetadata.OpenGraphMetadataTitle), string.Empty), Is.EqualTo(expected.OpenGraphMetadataTitle));
                Assert.That(actual.GetStringValue(nameof(OpenGraphMetadata.OpenGraphMetadataDescription), string.Empty), Is.EqualTo(expected.OpenGraphMetadataDescription));
            });
        }

        [Test]
        public void Deserialize_NestedComponentOfType_RetainsSerializedValues()
        {
            var serializer = new PageTypeComponentSerializer();

            PageMetadata metadata = TestData.PageMetadataComponent;
            OpenGraphMetadata expected = metadata.OpenGraph;

            string componentXml = serializer.Serialize(metadata);

            var deserializer = new PageTypeComponentDeserializer();

            OpenGraphMetadata actual = deserializer.Deserialize<PageMetadata>(componentXml).OpenGraph;

            Assert.Multiple(() =>
            {
                Assert.That(actual.OpenGraphMetadataTitle, Is.EqualTo(expected.OpenGraphMetadataTitle));
                Assert.That(actual.OpenGraphMetadataDescription, Is.EqualTo(expected.OpenGraphMetadataDescription));
            });
        }

        [Test]
        public void Deserialize_ComponentWithNullValues_DoesNotThrowException()
        {
            var node = TreeNode.New<ContentComponentTest>();

            var serialiser = new PageTypeComponentSerializer();
            string nodeXml = serialiser.Serialize(node);

            var deserializer = new PageTypeComponentDeserializer();

            Assert.That(() => deserializer.Deserialize<ContentComponentTest>(nodeXml), Throws.Nothing);
        }
    }
}
