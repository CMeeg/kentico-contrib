using System;
using System.Linq;
using Meeg.Configuration;

namespace Meeg.Kentico.Configuration.Cms
{
    public static class AppConfigurationBinderExtensions
    {
        public static T GetValue<T>(this IAppConfigurationRoot configuration, string key, string siteName)
        {
            return configuration.GetValue(key, siteName, default(T));
        }

        public static T GetValue<T>(this IAppConfigurationRoot configuration, string key, string siteName,
            T defaultValue)
        {
            if (string.IsNullOrEmpty(siteName))
            {
                // Return the "global" setting

                return configuration.GetValue(key, defaultValue);
            }

            // Try to get the site setting

            string siteKey = AppConfigurationPath.Combine(siteName, key);

            if (configuration.AllKeys.Contains(siteKey))
            {
                return configuration.GetValue(siteKey, defaultValue);
            }

            // Default to "global" setting

            return configuration.GetValue(key, defaultValue);
        }

        public static T Get<T>(this IAppConfigurationRoot configuration, string siteName)
        {
            return configuration.Get<T>(siteName, _ => { });
        }

        public static T Get<T>(this IAppConfigurationRoot configuration, string siteName,
            Action<AppConfigurationBinderOptions> configureOptions)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var result = configuration.Get(typeof(T), siteName, configureOptions);

            if (result == null)
            {
                return default;
            }

            return (T) result;
        }

        public static object Get(this IAppConfigurationRoot configuration, Type type, string siteName)
        {
            return configuration.Get(type, siteName, _ => { });
        }

        public static object Get(this IAppConfigurationRoot configuration, Type type, string siteName,
            Action<AppConfigurationBinderOptions> configureOptions)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var section = new CmsAppConfigurationSection(configuration, siteName);

            return section.Get(type, configureOptions);
        }

        public static void Bind(this IAppConfigurationRoot configuration, string key, string siteName, object instance)
        {
            configuration.GetSection(key, siteName).Bind(instance);
        }

        public static void Bind(this IAppConfigurationRoot configuration, string siteName, object instance)
        {
            configuration.Bind(siteName, instance, o => { });
        }

        public static void Bind(this IAppConfigurationRoot configuration, string siteName, object instance, Action<AppConfigurationBinderOptions> configureOptions)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var section = new CmsAppConfigurationSection(configuration, siteName);

            section.Bind(instance, configureOptions);
        }
    }
}
