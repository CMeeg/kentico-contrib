using Meeg.Kentico.ContentComponents;

// ReSharper disable once CheckNamespace
namespace CMS.DocumentEngine.Types.KenticoContrib
{
    public partial class Home
    {
        public PageMetadata Metadata => this.GetContentComponent<PageMetadata>(nameof(HomeMetadata));
    }
}
