using Meeg.Kentico.ContentComponents;

// ReSharper disable once CheckNamespace
namespace CMS.DocumentEngine.Types.KenticoContrib
{
    public partial class Page
    {
        private Metadata metadataComponent;
        public Metadata MetadataComponent
        {
            get
            {
                // TODO: Need a better way to track if this has been attempted before rather than relying on null

                if (metadataComponent == null)
                {
                    metadataComponent = this.GetContentComponent<Metadata>(nameof(Metadata));
                }

                return metadataComponent;
            }
        }
    }
}
