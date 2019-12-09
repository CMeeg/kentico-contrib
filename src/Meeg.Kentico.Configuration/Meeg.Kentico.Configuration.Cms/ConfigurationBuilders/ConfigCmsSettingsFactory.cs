using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;

namespace Meeg.Kentico.Configuration.Cms.ConfigurationBuilders
{
    public class ConfigCmsSettingsFactory
    {
        public const string SiteNameDelimiter = "__";

        internal ICollection<KeyValuePair<string, string>> CreateConfigSettings(IEnumerable<CmsSetting> settings)
        {
            var values = new Dictionary<string, string>();

            foreach (CmsSetting setting in settings)
            {
                string key = CreateConfigKeyName(setting.Name, setting.SiteName);

                values[key] = setting.Value;
            }

            return values;
        }

        public string CreateConfigKeyName(string settingKeyName, string siteName)
        {
            return $"{settingKeyName}{SiteNameDelimiter}{siteName ?? string.Empty}".TrimEnd(SiteNameDelimiter.ToCharArray());
        }

        public SettingsKeyName CreateSettingsKeyName(string configKey)
        {
            string[] configKeyParts = configKey.Split(
                SiteNameDelimiter.ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries
            );

            if (configKeyParts.Length == 1)
            {
                return new SettingsKeyName(configKey);
            }

            string siteName = configKeyParts.Length == 1
                ? null
                : configKeyParts.Last();

            string keyName = string.Join(
                SiteNameDelimiter,
                configKeyParts.Take(configKeyParts.Length - 1)
            );

            return new SettingsKeyName(keyName, siteName);
        }
    }
}
