using System;
using System.Collections.Generic;
using System.Linq;
using Meeg.Configuration;

namespace Meeg.Kentico.Configuration.Cms
{
    public class CmsAppConfigurationSection : IAppConfigurationSection
    {
        private readonly IAppConfigurationRoot root;
        private readonly string siteName;
        private readonly IAppConfigurationSection siteSection;
        private readonly IAppConfigurationSection globalSection;

        private string sectionKey;
        public string Key => sectionKey ?? (sectionKey = AppConfigurationPath.GetSectionKey(Path));
        public string Value { get; }
        public string Path { get; }

        public string this[string key] => GetValue(key);

        public CmsAppConfigurationSection(IAppConfigurationRoot root, string siteName)
            : this(root, siteName, siteName)
        {
        }

        public CmsAppConfigurationSection(IAppConfigurationRoot root, string key, string siteName)
        {
            if (string.IsNullOrEmpty(siteName))
            {
                throw new ArgumentException("Cannot be null or empty.", nameof(siteName));
            }

            this.root = root;
            this.siteName = siteName;

            string siteKey, globalKey;

            if (siteName == key)
            {
                siteKey = siteName;
                globalKey = null;
            }
            else if (key.StartsWith($"{siteName}{AppConfigurationPath.KeyDelimiter}", StringComparison.OrdinalIgnoreCase))
            {
                siteKey = key;
                globalKey = key.Substring(siteName.Length + 1);
            }
            else
            {
                siteKey = AppConfigurationPath.Combine(siteName, key);
                globalKey = key;
            }

            siteSection = root.GetSection(siteKey);
            globalSection = globalKey == null ? null : root.GetSection(globalKey);

            Path = siteKey;
            Value = siteSection.Exists() ? siteSection.Value : globalSection?.Value;
        }

        private string GetValue(string key)
        {
            var siteKeySection = siteSection
                .GetChildren()
                .FirstOrDefault(section => section.Key.Equals(key, StringComparison.OrdinalIgnoreCase));

            if (siteKeySection != null)
            {
                return siteKeySection.Value;
            }

            return globalSection?[key];
        }

        public IAppConfigurationSection GetSection(string key)
        {
            return new CmsAppConfigurationSection(
                root,
                AppConfigurationPath.Combine(Path, key),
                siteName
            );
        }

        public IEnumerable<IAppConfigurationSection> GetChildren()
        {
            // We will loop through all of the global children and see if there is a site equivalent

            var children = new List<IAppConfigurationSection>();

            var globalChildren = globalSection == null
                ? root.GetChildren()
                : globalSection.GetChildren();

            foreach (IAppConfigurationSection globalChild in globalChildren)
            {
                if (globalChild.Path.Equals(siteName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string siteKey = AppConfigurationPath.Combine(siteName, globalChild.Path).ToLower();

                // Site settings take precedence over global settings

                if (root.AllKeys.Contains(siteKey)){

                    var siteChild = root.GetSection(siteKey);

                    children.Add(siteChild);
                }
                else
                {
                    var siteChild = new CmsAppConfigurationSection(
                        root,
                        AppConfigurationPath.Combine(siteName, globalChild.Path),
                        siteName
                    );

                    children.Add(siteChild);
                }
            }

            return children.AsReadOnly();
        }
    }
}
