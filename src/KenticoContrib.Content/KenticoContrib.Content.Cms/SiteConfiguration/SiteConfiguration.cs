using Meeg.Kentico.ContentComponents.Cms;

// ReSharper disable once CheckNamespace
namespace CMS.DocumentEngine.Types.KenticoContrib
{
    public partial class SiteConfiguration
    {
        public PageMetadata DefaultMetadata => this.GetContentComponent<PageMetadata>(nameof(SiteConfigurationDefaultMetadata));
    }
}
