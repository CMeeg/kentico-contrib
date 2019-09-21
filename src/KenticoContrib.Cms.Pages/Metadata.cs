// ReSharper disable once CheckNamespace
namespace CMS.DocumentEngine.Types.KenticoContrib
{
    public partial class Metadata
    {
        public string PageTitle
        {
            get => DocumentPageTitle;
            set => SetValue("DocumentPageTitle", value);
        }

        public string PageDescription
        {
            get => DocumentPageDescription;
            set => SetValue("DocumentPageDescription", value);
        }
    }
}
