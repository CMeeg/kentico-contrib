using CMS.DataEngine;

namespace Meeg.Kentico.Configuration.Cms.ConfigurationBuilders
{
    internal interface ICmsSettingConfigKeyNameFactory
    {
        string CreateConfigKeyName(CmsSetting setting);

        SettingsKeyName CreateSettingsKeyName(string configKey);
    }
}
