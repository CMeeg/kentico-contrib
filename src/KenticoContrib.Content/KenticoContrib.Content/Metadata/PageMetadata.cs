namespace KenticoContrib.Content.Metadata
{
    public class PageMetadata
    {
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string PageTitleSuffix { get; set; }
        public OpenGraphMetadata OpenGraph { get; set; }
        public TwitterMetadata Twitter { get; set; }
    }
}
