using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KenticoContrib;

namespace Meeg.Kentico.ContentComponents.Cms.Tests
{
    public static class TestData
    {
        public static readonly PageMetadata PageMetadataComponent;

        static TestData()
        {
            PageMetadataComponent = CreatePageMetadataComponent();
        }

        private static PageMetadata CreatePageMetadataComponent()
        {
            var node = TreeNode.New<PageMetadata>();
            node.SetValue(nameof(PageMetadata.DocumentPageTitle), "Fake page title");
            node.SetValue(nameof(PageMetadata.DocumentPageDescription), "Fake page description");

            var serialiser = new PageTypeComponentSerializer();
            node.PageMetadataOpenGraph = serialiser.Serialize(CreateOpenGraphMetadataComponent());

            return node;
        }

        private static OpenGraphMetadata CreateOpenGraphMetadataComponent()
        {
            var node = TreeNode.New<OpenGraphMetadata>();
            node.SetValue(nameof(OpenGraphMetadata.OpenGraphMetadataTitle), "Fake Open Graph title");
            node.SetValue(nameof(OpenGraphMetadata.OpenGraphMetadataDescription), "Fake Open Graph description");

            return node;
        }
    }
}
