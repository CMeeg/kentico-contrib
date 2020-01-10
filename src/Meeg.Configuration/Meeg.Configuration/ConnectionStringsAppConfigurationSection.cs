using System.Collections.Generic;
using System.Linq;

namespace Meeg.Configuration
{
    public class ConnectionStringsAppConfigurationSection : IAppConfigurationSection
    {
        public const string SectionKey = "ConnectionStrings";

        private readonly IConfigurationManager configurationManager;

        private string sectionKey;
        public string Key => sectionKey ?? (sectionKey = AppConfigurationPath.GetSectionKey(Path));
        public string Value => configurationManager.ConnectionStrings[Key];
        public string Path { get; }

        public string this[string key] => configurationManager.ConnectionStrings[key];

        public ConnectionStringsAppConfigurationSection(IConfigurationManager configurationManager, string path)
        {
            this.configurationManager = configurationManager;

            Path = path ?? SectionKey;
        }

        public IAppConfigurationSection GetSection(string key)
        {
            return new ConnectionStringsAppConfigurationSection(
                configurationManager,
                AppConfigurationPath.Combine(Path, key)
            );
        }

        public IEnumerable<IAppConfigurationSection> GetChildren()
        {
            if (!Path.Equals(SectionKey))
            {
                // Only the root level has children - connection strings are not hierarchical

                return Enumerable.Empty<IAppConfigurationSection>();
            }

            return configurationManager.ConnectionStrings.AllKeys
                .Select(GetSection);
        }
    }
}
