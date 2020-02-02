namespace Meeg.Kentico.Configuration.Cms
{
    internal class CmsSetting
    {
        public string Name { get; }
        public string Value { get; }
        public string CategoryName { get; }
        public string SiteName { get; }

        public CmsSetting(string name, string value, string categoryName, string siteName)
        {
            Name = name;
            Value = value;
            CategoryName = categoryName;
            SiteName = siteName;
        }
    }
}
