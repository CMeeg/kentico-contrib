namespace KenticoContrib.Content
{
    public interface IPage
    {
        PageMetadata Metadata { get; set; }
        int Id { get; set; }
        string Name { get; set; }
    }
}
