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

        public void WithAppSetting(string key, string value)
        {
            appSettings.Add(key, value);
        }

        public void WithConnectionString(string name, string connectionString)
        {
            connectionStrings.Add(name, connectionString);
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
