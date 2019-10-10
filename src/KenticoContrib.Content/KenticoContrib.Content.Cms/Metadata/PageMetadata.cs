using Meeg.Kentico.ContentComponents.Cms;

// ReSharper disable once CheckNamespace
namespace CMS.DocumentEngine.Types.KenticoContrib
{
    public partial class PageMetadata
    {
        public OpenGraphMetadata OpenGraph => this.GetPageTypeComponent<OpenGraphMetadata>(nameof(PageMetadataOpenGraph));
        public TwitterMetadata Twitter => this.GetPageTypeComponent<TwitterMetadata>(nameof(PageMetadataTwitter));
    }
}
