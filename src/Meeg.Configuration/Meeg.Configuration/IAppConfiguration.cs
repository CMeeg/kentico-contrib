namespace Meeg.Configuration
{
    public interface IAppConfiguration
    {
        string GetValue(string key);

        string GetConnectionString(string name);
    }
}
