using System;
using System.Linq;
using NUnit.Framework;

namespace Meeg.Configuration.Tests
{
    [TestFixture]
    public class AppConfigurationTests
    {
        [Test]
        public void AllKeys_WithAppSettingsAndConnectionStrings_ReturnsOnlyAppSettingsKeys()
        {
            var configManagerFixture = new ConfigurationManagerFixture();

            configManagerFixture.WithAppSetting("Key", "Value1");
            configManagerFixture.WithAppSetting("Section:Key", "Value2");
            configManagerFixture.WithConnectionString("Name", "ConnString");

            var configManager = configManagerFixture.Build();

            var appConfig = new AppConfiguration(configManager);

            var actual = appConfig.AllKeys;

            var expected = configManager.AppSettings.AllKeys;

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void KeyIndexer_WithNotPresentKey_ReturnsNull()
        {
            var configManagerFixture = new ConfigurationManagerFixture();

            configManagerFixture.WithAppSetting("Key", "Value1");

            IConfigurationManager configManager = configManagerFixture.Build();

            var appConfig = new AppConfiguration(configManager);

            string actual = appConfig["KeyNotPresent"];

            Assert.That(actual, Is.Null);
        }

        [TestCase("Key", "Value1")]
        [TestCase("Section:Key", "Value2")]
        public void KeyIndexer_WithKey_ReturnsValue(string key, string value)
        {
            var configManagerFixture = new ConfigurationManagerFixture();

            configManagerFixture.WithAppSetting(key, value);

            IConfigurationManager configManager = configManagerFixture.Build();

            var appConfig = new AppConfiguration(configManager);

            string actual = appConfig[key];
            
            Assert.That(actual, Is.EqualTo(value));
        }

        [Test]
        public void GetSection_WithNotPresentSection_ReturnsEmptySection()
        {
            var configManagerFixture = new ConfigurationManagerFixture();

            configManagerFixture.WithAppSetting("Section:Key", "Value1");

            IConfigurationManager configManager = configManagerFixture.Build();

            var appConfig = new AppConfiguration(configManager);

            IAppConfigurationSection section = appConfig.GetSection("SectionNotPresent");

            bool actual = section.Exists();

            Assert.That(actual, Is.False);
        }

        [TestCase("Key", "Value1")]
        public void GetSection_WithKeyWithNoSection_ReturnsKeyAsSection(string key, string value)
        {
            var configManagerFixture = new ConfigurationManagerFixture();

            configManagerFixture.WithAppSetting(key, value);

            IConfigurationManager configManager = configManagerFixture.Build();

            var appConfig = new AppConfiguration(configManager);

            IAppConfigurationSection section = appConfig.GetSection(key);

            string actual = section.Value;

            Assert.That(actual, Is.EqualTo(value));
        }

        [TestCase("Section", "Key", "Value")]
        [TestCase("Section:SubSection", "Key", "Value")]
        public void GetSection_WithSection_ReturnsSectionWithKeys(string sectionKey, string key, string value)
        {
            var configManagerFixture = new ConfigurationManagerFixture();

            int numKeys = new Random().Next(1, 10);

            for (int keyCt = 1; keyCt <= numKeys; keyCt++)
            {
                configManagerFixture.WithAppSetting(AppConfigurationPath.Combine(sectionKey, $"{key}{keyCt}"), $"{value}{keyCt}");
            }

            IConfigurationManager configManager = configManagerFixture.Build();

            var appConfig = new AppConfiguration(configManager);

            IAppConfigurationSection section = appConfig.GetSection(sectionKey);

            int actual = section.GetChildren().Count();

            Assert.That(actual, Is.EqualTo(numKeys));
        }

        [Test]
        public void GetSection_WithSubSection_ReturnsSubSectionWithKeys()
        {
            var configManagerFixture = new ConfigurationManagerFixture();

            int numKeys = new Random().Next(1, 10);

            for (int keyCt = 1; keyCt <= numKeys; keyCt++)
            {
                configManagerFixture.WithAppSetting(AppConfigurationPath.Combine("Section", $"Key{keyCt}"), $"Value{keyCt}");
                configManagerFixture.WithAppSetting(AppConfigurationPath.Combine("Section:SubSection", $"Key{keyCt}"), $"Value{keyCt}");
                configManagerFixture.WithAppSetting(AppConfigurationPath.Combine("Section:OtherSubSection", $"Key{keyCt}"), $"Value{keyCt}");
            }

            IConfigurationManager configManager = configManagerFixture.Build();

            var appConfig = new AppConfiguration(configManager);

            IAppConfigurationSection section = appConfig.GetSection("Section");
            IAppConfigurationSection subSection = section.GetSection("SubSection");

            int actual = subSection.GetChildren().Count();

            Assert.That(actual, Is.EqualTo(numKeys));
        }

        [Test]
        public void GetChildren_WithSection_ReturnsAllDescendents()
        {
            var configManagerFixture = new ConfigurationManagerFixture();

            int numKeys = new Random().Next(1, 10);

            for (int keyCt = 1; keyCt <= numKeys; keyCt++)
            {
                configManagerFixture.WithAppSetting($"Key{keyCt}", $"Value{keyCt}");
                configManagerFixture.WithAppSetting($"Section:Key{keyCt}", $"Value{keyCt}");
                configManagerFixture.WithAppSetting($"Section:SubSection:Key{keyCt}", $"Value{keyCt}");
                configManagerFixture.WithAppSetting($"OtherSection:Key{keyCt}", $"Value{keyCt}");
            }

            IConfigurationManager configManager = configManagerFixture.Build();

            var appConfig = new AppConfiguration(configManager);

            IAppConfigurationSection section = appConfig.GetSection("Section");

            int actual = section.GetChildren().Count();

            const int numSections = 2;
            int expected = numKeys * numSections;

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetConnectionString_WithNotPresentName_ReturnsNull()
        {
            var configManagerFixture = new ConfigurationManagerFixture();

            configManagerFixture.WithConnectionString("Name", "ConnString");

            IConfigurationManager configManager = configManagerFixture.Build();

            var appConfig = new AppConfiguration(configManager);

            string actual = appConfig.GetConnectionString("NameNotPresent");

            Assert.That(actual, Is.Null);
        }

        [TestCase("Key", "Value1")]
        [TestCase("Section:Key", "Value2")]
        public void GetConnectionString_WithName_ReturnsConnectionString(string name, string connectionString)
        {
            var configManagerFixture = new ConfigurationManagerFixture();

            configManagerFixture.WithConnectionString(name, connectionString);

            IConfigurationManager configManager = configManagerFixture.Build();

            var appConfig = new AppConfiguration(configManager);

            string actual = appConfig.GetConnectionString(name);

            Assert.That(actual, Is.EqualTo(connectionString));
        }
    }
}
