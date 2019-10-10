namespace KenticoContrib.Content
{
    public class CurrentPageContext : ICurrentPageContext
    {
        public IPage Page { get; private set; }

        public void SetCurrentPage(IPage page)
        {
            Page = page;
        }
    }
}
