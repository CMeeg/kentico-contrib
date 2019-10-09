using Meeg.Kentico.ContentComponents.Cms;

// ReSharper disable once CheckNamespace
namespace CMS.DocumentEngine.Types.KenticoContrib
{
    public partial class Page
    {
        public PageMetadata Metadata => this.GetPageTypeComponent<PageMetadata>(nameof(PageMetadata));
    }
}
