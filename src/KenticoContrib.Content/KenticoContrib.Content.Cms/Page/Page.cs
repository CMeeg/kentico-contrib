using Meeg.Kentico.ContentComponents;

// ReSharper disable once CheckNamespace
namespace CMS.DocumentEngine.Types.KenticoContrib
{
    public partial class Page
    {
        public Metadata Metadata => this.GetContentComponent<Metadata>(nameof(PageMetadata));
    }
}
