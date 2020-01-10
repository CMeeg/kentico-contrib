using System;
using System.Collections.Generic;
using System.Linq;

namespace Meeg.Configuration
{
    public static class AppConfigurationExtensions
    {
        public static string GetConnectionString(this IAppConfiguration configuration, string name)
        {
            return configuration?.GetSection(ConnectionStringsAppConfigurationSection.SectionKey)?[name];
        }

        public static bool Exists(this IAppConfigurationSection section)
        {
            if (section == null)
            {
                return false;
            }

            return section.Value != null || section.GetChildren().Any();
        }

        internal static IEnumerable<IAppConfigurationSection> GetChildren(this IAppConfiguration configuration, string path)
        {
            if (configuration == null)
            {
                return Enumerable.Empty<IAppConfigurationSection>();
            }

            if (!(configuration is IAppConfigurationRoot root))
            {
                return configuration.GetChildren(path);
            }

            IEnumerable<string> childKeys = GetChildKeys(root, path);

            return childKeys.Select(key => root.GetSection(path == null
                ? key
                : AppConfigurationPath.Combine(path, key))
            ); 
        }

        private static IEnumerable<string> GetChildKeys(IAppConfigurationRoot root, string parentPath)
        {
            string prefix = parentPath == null
                ? string.Empty
                : $"{parentPath}{AppConfigurationPath.KeyDelimiter}";

            return root.AllKeys
                .Where(key => key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .Select(key => Segment(key, prefix.Length))
                .OrderBy(key => key, AppConfigurationKeyComparer.Instance);
        }

        private static string Segment(string key, int prefixLength)
        {
            int indexOf = key.IndexOf(
                AppConfigurationPath.KeyDelimiter,
                prefixLength,
                StringComparison.OrdinalIgnoreCase
            );

            return indexOf < 0
                ? key.Substring(prefixLength)
                : key.Substring(prefixLength, indexOf - prefixLength);
        }
    }
}
