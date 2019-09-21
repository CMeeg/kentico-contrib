using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KenticoContrib;

namespace Meeg.Kentico.ContentComponents.Tests
{
    public static class TestData
    {
        public static readonly Metadata MetadataComponent;

        static TestData()
        {
            MetadataComponent = CreateMetadataComponent();
        }

        private static Metadata CreateMetadataComponent()
        {
            var metadata = TreeNode.New<Metadata>();
            metadata.PageTitle = "Fake page title";
            metadata.PageDescription = "Fake page description";
            metadata.MetadataOpenGraphTitle = "Fake Open Graph title";
            metadata.MetadataOpenGraphDescription = "Fake Open Graph description.";

            return metadata;
        }
    }
}
