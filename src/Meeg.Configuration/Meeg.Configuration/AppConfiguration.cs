using System.Collections.Generic;

namespace Meeg.Configuration
{
    public class AppConfiguration : IAppConfigurationRoot
    {
        private readonly IConfigurationManager configurationManager;

        public string[] AllKeys => configurationManager.AppSettings.AllKeys;

        public string this[string key] => configurationManager.AppSettings[key];

        public AppConfiguration(IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }

        public IAppConfigurationSection GetSection(string key)
        {
            if (key.Equals(ConnectionStringsAppConfigurationSection.SectionKey))
            {
                return new ConnectionStringsAppConfigurationSection(configurationManager, null);
            }

            return new AppConfigurationSection(this, key);
        }

        public IEnumerable<IAppConfigurationSection> GetChildren()
        {
            return this.GetChildren(null);
        }
    }
}
