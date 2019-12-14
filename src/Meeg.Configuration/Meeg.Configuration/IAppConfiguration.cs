namespace Meeg.Configuration
{
    public interface IAppConfiguration
    {
        string SectionDelimiter { get; }

        string GetValue(string key);

        string GetConnectionString(string name);
    }
}
