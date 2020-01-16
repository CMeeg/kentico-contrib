using System.Collections.Generic;
using System.Linq;
using Meeg.Configuration;

namespace Meeg.Kentico.Configuration.Cms
{
    public static class AppConfigurationExtensions
    {
        public static string GetValue(this IAppConfigurationRoot appConfig, string key, string siteName)
        {
            return appConfig.GetValue<string>(key, siteName);
        }

        public static string GetValue(this IAppConfigurationRoot appConfig, string key, string siteName, string defaultValue)
        {
            return appConfig.GetValue<string>(key, siteName, defaultValue);
        }

        public static IAppConfigurationSection GetSection(this IAppConfigurationRoot appConfig, string key, string siteName)
        {
            if (string.IsNullOrEmpty(siteName))
            {
                // Return the "global" section 

                return appConfig.GetSection(key);
            }

            return new CmsAppConfigurationSection(appConfig, key, siteName);
        }

        public static IEnumerable<IAppConfigurationSection> GetChildren(this IAppConfigurationRoot appConfig, string siteName)
        {
            if (string.IsNullOrEmpty(siteName))
            {
                return Enumerable.Empty<IAppConfigurationSection>();
            }

            IAppConfigurationSection section = new CmsAppConfigurationSection(appConfig, siteName);

            return section.GetChildren();
        }
    }
}
