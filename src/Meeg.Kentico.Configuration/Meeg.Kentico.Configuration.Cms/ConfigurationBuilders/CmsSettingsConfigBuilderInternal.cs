using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;

namespace Meeg.Kentico.Configuration.Cms.ConfigurationBuilders
{
    internal class CmsSettingsConfigBuilderInternal
    {
        private readonly CmsSettingsConfigBuilderOptions options;
        private readonly IQueryHandler<AllConfigCmsSettingsQuery, IReadOnlyCollection<CmsSetting>> allSettingsQueryHandler;
        private readonly CmsSettingConfigKeyNameFactory configKeyNameFactory;

        public CmsSettingsConfigBuilderInternal(
            CmsSettingsConfigBuilderOptions options,
            IQueryHandler<AllConfigCmsSettingsQuery, IReadOnlyCollection<CmsSetting>> allSettingsQueryHandler,
            CmsSettingConfigKeyNameFactory configKeyNameFactory
        )
        {
            this.options = options;
            this.allSettingsQueryHandler = allSettingsQueryHandler;
            this.configKeyNameFactory = configKeyNameFactory;
        }

        public string GetValue(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return null;
                }

                SettingsKeyName settingsKeyName = configKeyNameFactory.CreateSettingsKeyName(key);

                return SettingsKeyInfoProvider.GetValue(settingsKeyName);
            }
            catch (Exception)
            {
                if (!options.Optional)
                {
                    throw;
                }
            }

            return null;
        }

        public ICollection<KeyValuePair<string, string>> GetAllValues(string prefix)
        {
            var values = new Dictionary<string, string>();

            try
            {
                // Get all settings

                var query = new AllConfigCmsSettingsQuery(options.ProcName, prefix);
                IReadOnlyCollection<CmsSetting> settings = allSettingsQueryHandler.Handle(query);

                // Group settings by site (global settings represented by an empty string) so we can split up processing

                const string globalSiteName = "";

                var settingsBySite = settings.GroupBy(setting => setting.SiteName ?? globalSiteName)
                    .ToDictionary(group => group.Key, group => group.ToList());

                // Add global settings first as these are a straight-forward add

                List<CmsSetting> globalSettings = settingsBySite[globalSiteName];

                foreach (CmsSetting setting in globalSettings)
                {
                    AddSetting(setting, values);
                }

                // Group global settings by category - these will be used as default values for site settings

                const string nullCategoryName = "";

                var globalSettingsByCategory = globalSettings
                    .GroupBy(setting => setting.CategoryName ?? nullCategoryName)
                    .ToDictionary(group => group.Key, group => group.ToList());

                // Now add site settings

                foreach (string siteName in settingsBySite.Keys)
                {
                    if (siteName == globalSiteName)
                    {
                        // We have already added global settings so continue

                        continue;
                    }

                    // We will add one category at a time and fallback to global values for any related category setting that is not set at site level - this is done to make it easier to get config settings by section (category)

                    var siteSettingsByCategory = settingsBySite[siteName]
                        .GroupBy(setting => setting.CategoryName ?? nullCategoryName)
                        .ToDictionary(group => group.Key, group => group.ToList());

                    foreach (string categoryName in siteSettingsByCategory.Keys)
                    {
                        List<CmsSetting> categoryGlobalSettings = globalSettingsByCategory[categoryName];
                        List<CmsSetting> categorySiteSettings = siteSettingsByCategory[categoryName];

                        foreach (CmsSetting globalSetting in categoryGlobalSettings)
                        {
                            // There may not be a site setting present for each global setting in the category - if there is not add the setting with the global value; if there is add the setting with the site value

                            CmsSetting siteSetting = categorySiteSettings.FirstOrDefault(setting => setting.Name == globalSetting.Name);

                            var categorySetting = new CmsSetting(
                                globalSetting.Name,
                                siteSetting == null ? globalSetting.Value : siteSetting.Value,
                                categoryName,
                                siteName
                            );

                            AddSetting(categorySetting, values);
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (!options.Optional)
                {
                    throw;
                }
            }

            return values;
        }

        private void AddSetting(CmsSetting setting, Dictionary<string, string> values)
        {
            string key = configKeyNameFactory.CreateConfigKeyName(setting);

            values[key] = setting.Value;
        }
    }
}
