using Meeg.Kentico.ContentComponents;

// ReSharper disable once CheckNamespace
namespace CMS.DocumentEngine.Types.KenticoContrib
{
    public partial class Page
    {
        public Metadata MetadataComponent => this.GetContentComponent<Metadata>(nameof(Metadata));
    }
}
