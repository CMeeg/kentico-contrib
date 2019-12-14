using System;
using System.Text;
using CMS.DataEngine;
using Meeg.Configuration;

namespace Meeg.Kentico.Configuration.Cms.ConfigurationBuilders
{
    internal class CmsSettingConfigKeyNameFactory : ICmsSettingConfigKeyNameFactory
    {
        private readonly IAppConfiguration appConfig;
        private readonly bool useCategorySections;

        public CmsSettingConfigKeyNameFactory(IAppConfiguration appConfig, bool useCategorySections)
        {
            this.appConfig = appConfig;
            this.useCategorySections = useCategorySections;
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
                keyName.Append($"{siteName}{appConfig.SectionDelimiter}");
            }

            if (useCategorySections && !string.IsNullOrEmpty(categoryName))
            {
                keyName.Append($"{categoryName}{appConfig.SectionDelimiter}");
            }

            keyName.Append(settingKeyName);

            return keyName.ToString();
        }

        public SettingsKeyName CreateSettingsKeyName(string configKey)
        {
            if (string.IsNullOrEmpty(configKey))
            {
                return null;
            }

            // Settings have up to three parts separated by a delimiter: An optional site name, an optional category name, and a setting key name

            int lastSeparatorIndex = configKey.LastIndexOf(appConfig.SectionDelimiter, StringComparison.Ordinal);

            if (lastSeparatorIndex == -1)
            {
                // No separator present so this is just a key name

                return new SettingsKeyName(configKey);
            }

            // We have two parts so we need to figure out if the first part is a site name or not

            string sectionName = configKey.Substring(0, lastSeparatorIndex);
            string keyName = configKey.Substring(lastSeparatorIndex + 1);

            string[] sectionNameParts = sectionName.Split(
                appConfig.SectionDelimiter.ToCharArray(),
                2,
                StringSplitOptions.RemoveEmptyEntries
            );

            string siteName = sectionNameParts[0];

            if (sectionNameParts.Length == 2)
            {
                // We have a site name and category name - the first part must be the site name

                return new SettingsKeyName(keyName, siteName);
            }

            // We could have a site name or category name so we need to check

            var siteId = new SiteInfoIdentifier(siteName);

            if (siteId.ObjectID == 0)
            {
                // It's not a valid site name so we will treat it as a global settings

                return new SettingsKeyName(keyName);
            }

            // This is a site setting

            return new SettingsKeyName(keyName, siteName);
        }
    }
}
