namespace KenticoContrib.Content
{
    public interface ICurrentPageContext
    {
        IPage Page { get; }

        void SetCurrentPage(IPage page);
    }
}
