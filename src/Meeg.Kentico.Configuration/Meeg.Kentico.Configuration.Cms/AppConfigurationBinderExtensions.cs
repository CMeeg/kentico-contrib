using System.Linq;
using Meeg.Configuration;

namespace Meeg.Kentico.Configuration.Cms
{
    public static class AppConfigurationBinderExtensions
    {
        public static T GetValue<T>(this IAppConfigurationRoot appConfig, string key, string siteName)
        {
            return appConfig.GetValue(key, siteName, default(T));
        }

        public static T GetValue<T>(this IAppConfigurationRoot appConfig, string key, string siteName, T defaultValue)
        {
            if (string.IsNullOrEmpty(siteName))
            {
                // Return the "global" setting

                return appConfig.GetValue(key, defaultValue);
            }

            // Try to get the site setting

            string siteKey = AppConfigurationPath.Combine(siteName, key);

            if (appConfig.AllKeys.Contains(siteKey))
            {
                return appConfig.GetValue(siteKey, defaultValue);
            }

            // Default to "global" setting

            return appConfig.GetValue(key, defaultValue);
        }
    }
}
