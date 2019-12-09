namespace Meeg.Kentico.Configuration.Cms
{
    internal class CmsSetting
    {
        public string Name { get; }
        public string Value { get; }
        public string SiteName { get; }

        public CmsSetting(string name, string value, string siteName)
        {
            Name = name;
            Value = value;
            SiteName = siteName;
        }
    }
}
