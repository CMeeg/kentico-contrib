using System;
using System.Text;
using CMS.DataEngine;
using Meeg.Configuration;

namespace Meeg.Kentico.Configuration.Cms.ConfigurationBuilders
{
    internal class CmsSettingConfigKeyNameFactory
    {
        private readonly IAppConfiguration appConfig;
        private readonly CmsSettingsConfigBuilderOptions options;

        public CmsSettingConfigKeyNameFactory(IAppConfiguration appConfig, CmsSettingsConfigBuilderOptions options)
        {
            this.appConfig = appConfig;
            this.options = options;
        }

        public string CreateConfigKeyName(CmsSetting setting)
        {
            return CreateConfigKeyName(
                setting.Name,
                setting.SiteName,
                setting.CategoryName
            );
        }

        private string CreateConfigKeyName(string settingKeyName, string siteName, string categoryName)
        {
            var keyName = new StringBuilder();

            if (!string.IsNullOrEmpty(siteName))
            {
                keyName.Append($"{siteName}{AppConfigurationPath.KeyDelimiter}");
            }

            if (options.UseCategorySections && !string.IsNullOrEmpty(categoryName))
            {
                keyName.Append($"{categoryName}{AppConfigurationPath.KeyDelimiter}");
            }

            if (options.StripPrefix && settingKeyName.StartsWith(options.KeyPrefix))
            {
                keyName.Append(settingKeyName.Substring(options.KeyPrefix.Length));
            }
            else
            {
                keyName.Append(settingKeyName);
            }

            return keyName.ToString();
        }

        public SettingsKeyName CreateSettingsKeyName(string configKey)
        {
            if (string.IsNullOrEmpty(configKey))
            {
                return null;
            }

            // Settings have up to three parts separated by a delimiter: An optional site name, an optional category name, and a setting key name

            string[] configKeyParts = configKey.SplitOnLastIndexOf(AppConfigurationPath.KeyDelimiter, StringComparison.Ordinal);

            if (configKeyParts.Length == 1)
            {
                // No separator present so this is just a key name

                return CreateSettingsKeyName(configKey, null);
            }

            // We have two parts - a "section" and a key

            string sectionName = configKeyParts[0];
            string keyName = configKeyParts[1];

            // If the prefix has been stripped we need to make sure it hasn't been added back in the wrong place i.e. we need it to be a prefix on the key name, not on the site or category name

            if (options.StripPrefix && sectionName.StartsWith(options.KeyPrefix))
            {
                sectionName = sectionName.Substring(options.KeyPrefix.Length);
            }

            // Now we need to extract the site name from the section name - if a site name is present it will come first

            string[] sectionNameParts = sectionName.Split(
                AppConfigurationPath.KeyDelimiter.ToCharArray(),
                2,
                StringSplitOptions.RemoveEmptyEntries
            );

            string siteName = sectionNameParts[0];

            return CreateSettingsKeyName(keyName, siteName);
        }

        private SettingsKeyName CreateSettingsKeyName(string keyName, string siteName)
        {
            string settingsKeyName = EnsureKeyPrefix(keyName);

            if (IsValidSiteName(siteName))
            {
                return new SettingsKeyName(settingsKeyName, siteName);
            }

            return new SettingsKeyName(settingsKeyName);
        }

        private string EnsureKeyPrefix(string keyName)
        {
            if (options.StripPrefix && !keyName.StartsWith(options.KeyPrefix))
            {
                return $"{options.KeyPrefix}{keyName}";
            }

            return keyName;
        }

        private bool IsValidSiteName(string siteName)
        {
            if (string.IsNullOrEmpty(siteName))
            {
                return false;
            }

            var siteId = new SiteInfoIdentifier(siteName);

            if (siteId.ObjectID == 0)
            {
                return false;
            }

            return true;
        }
    }
}
