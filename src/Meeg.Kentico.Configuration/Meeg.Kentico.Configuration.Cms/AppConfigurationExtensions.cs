using Meeg.Configuration;

namespace Meeg.Kentico.Configuration.Cms
{
    public static class AppConfigurationExtensions
    {
        public static string GetValue(this IAppConfiguration appConfig, string key, string siteName)
        {
            if (string.IsNullOrEmpty(siteName))
            {
                // Return the "global" setting

                return appConfig.GetValue(key);
            }

            // Try to get the site setting

            string siteConfigKey = $"{siteName}{appConfig.SectionDelimiter}{key}";

            // TODO: Can we check if the key exists? Does null always mean "not found" or can it genuinely be null - in which case we want to return it
            // https://stackoverflow.com/questions/3295293/how-to-check-if-an-appsettings-key-exists

            string siteValue = appConfig.GetValue(siteConfigKey);

            if (siteValue != null)
            {
                return siteValue;
            }

            // Default to "global" setting

            return appConfig.GetValue(key);
        }
    }
}
