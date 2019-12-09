using System.Configuration;

namespace Meeg.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name]?.ConnectionString;
        }
    }
}
