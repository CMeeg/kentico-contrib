using System;
using System.Collections.Generic;

namespace Meeg.Configuration
{
    public class AppConfigurationSection : IAppConfigurationSection
    {
        private readonly IAppConfiguration root;

        private string sectionKey;
        public string Key => sectionKey ?? (sectionKey = AppConfigurationPath.GetSectionKey(Path));

        public string Value => root[Path];

        public string Path { get; }

        public string this[string key] => root[AppConfigurationPath.Combine(Path, key)];

        public AppConfigurationSection(IAppConfiguration configuration, string path)
        {
            root = configuration ?? throw new ArgumentNullException(nameof(configuration));

            Path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public IAppConfigurationSection GetSection(string key)
        {
            return root.GetSection(AppConfigurationPath.Combine(Path, key));
        }

        public IEnumerable<IAppConfigurationSection> GetChildren()
        {
            return root.GetChildren(Path);
        }
    }
}
