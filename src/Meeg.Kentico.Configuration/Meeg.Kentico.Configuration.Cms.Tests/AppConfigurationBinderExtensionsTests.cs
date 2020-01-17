using Meeg.Configuration;
using NUnit.Framework;

namespace Meeg.Kentico.Configuration.Cms.Tests
{
    [TestFixture]
    public class AppConfigurationBinderExtensionsTests
    {
        public class TestSettings
        {
            public string Key1 { get; set; }
            public int Key2 { get; set; }
        }

        [Test]
        public void Get_WithSiteSetting_ReturnsSiteValue()
        {
            var configManager = new ConfigurationManagerFixture()
                .WithAppSetting("Key1", "Value1")
                .WithAppSetting("Key2", 2)
                .WithAppSetting("Site:Key2", 3)
                .Build();

            var config = new AppConfiguration(configManager);

            var settings = config.Get<TestSettings>("Site");

            Assert.Multiple(() =>
            {
                Assert.That(settings.Key1, Is.EqualTo("Value1"));

                Assert.That(settings.Key2, Is.EqualTo(3));
            });
        }

        [Test]
        public void Get_WithSiteSettingInCategory_ReturnsSiteValue()
        {
            var configManager = new ConfigurationManagerFixture()
                .WithAppSetting("Category1:Key1", "Value1")
                .WithAppSetting("Category1:Key2", 2)
                .WithAppSetting("Site:Category1:Key2", 3)
                .Build();

            var config = new AppConfiguration(configManager);

            var settings = config.GetSection("Category1", "Site").Get<TestSettings>();

            Assert.Multiple(() =>
            {
                Assert.That(settings.Key1, Is.EqualTo("Value1"));

                Assert.That(settings.Key2, Is.EqualTo(3));
            });
        }

        [Test]
        public void Bind_WithSiteSetting_ReturnsSiteValue()
        {
            var configManager = new ConfigurationManagerFixture()
                .WithAppSetting("Key1", "Value1")
                .WithAppSetting("Key2", 2)
                .WithAppSetting("Site:Key2", 3)
                .Build();

            var config = new AppConfiguration(configManager);

            var settings = new TestSettings();
            config.Bind("Site", settings);

            Assert.Multiple(() =>
            {
                Assert.That(settings.Key1, Is.EqualTo("Value1"));

                Assert.That(settings.Key2, Is.EqualTo(3));
            });
        }

        [Test]
        public void Bind_WithSiteSettingInCategory_ReturnsSiteValue()
        {
            var configManager = new ConfigurationManagerFixture()
                .WithAppSetting("Category1:Key1", "Value1")
                .WithAppSetting("Category1:Key2", 2)
                .WithAppSetting("Site:Category1:Key2", 3)
                .Build();

            var config = new AppConfiguration(configManager);

            var settings = new TestSettings();
            config.Bind("Category1", "Site", settings);

            Assert.Multiple(() =>
            {
                Assert.That(settings.Key1, Is.EqualTo("Value1"));

                Assert.That(settings.Key2, Is.EqualTo(3));
            });
        }
    }
}
