using Meeg.Kentico.ContentComponents;

// ReSharper disable once CheckNamespace
namespace CMS.DocumentEngine.Types.KenticoContrib
{
    public partial class PageMetadata
    {
        public OpenGraphMetadata OpenGraph => this.GetContentComponent<OpenGraphMetadata>(nameof(PageMetadataOpenGraph));
        public TwitterMetadata Twitter => this.GetContentComponent<TwitterMetadata>(nameof(PageMetadataTwitter));
    }
}
