using System.Collections.Generic;
using System.Collections.Specialized;
using FakeItEasy;

namespace Meeg.Configuration.Tests
{
    public class ConfigurationManagerFixture
    {
        private readonly NameValueCollection appSettings;
        private readonly NameValueCollection connectionStrings;

        public ConfigurationManagerFixture()
        {
            appSettings = new NameValueCollection();
            connectionStrings = new NameValueCollection();
        }

        public ConfigurationManagerFixture WithAppSetting(string key, object value)
        {
            appSettings.Add(key, value.ToString());

            return this;
        }

        public ConfigurationManagerFixture WithAppSettings<T>(string key, List<T> values)
        {
            for (int valueIndex = 0; valueIndex < values.Count; valueIndex++)
            {
                string itemKey = AppConfigurationPath.Combine(key, valueIndex.ToString());
                T value = values[valueIndex];

                appSettings.Add(itemKey, value.ToString());
            }

            return this;
        }

        public ConfigurationManagerFixture WithAppSettings<TKey, TValue>(Dictionary<TKey, TValue> settings)
        {
            foreach (TKey key in settings.Keys)
            {
                appSettings.Add(key.ToString(), settings[key]?.ToString());
            }

            return this;
        }

        public ConfigurationManagerFixture WithConnectionString(string name, string connectionString)
        {
            connectionStrings.Add(name, connectionString);

            return this;
        }

        public IConfigurationManager Build()
        {
            IConfigurationManager configManager = A.Fake<IConfigurationManager>();

            A.CallTo(() => configManager.AppSettings).Returns(appSettings);

            A.CallTo(() => configManager.ConnectionStrings).Returns(connectionStrings);

            return configManager;
        }
    }
}
