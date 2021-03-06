using CMS.DataEngine;
using CMS.SiteProvider;
using CMS.Tests;
using Meeg.Configuration;
using Meeg.Kentico.Configuration.Cms.ConfigurationBuilders;
using NUnit.Framework;

namespace Meeg.Kentico.Configuration.Cms.Tests.ConfigurationBuilders
{
    [TestFixture]
    public class CmsSettingConfigKeyNameFactoryTests : UnitTests
    {
        private IAppConfiguration Configuration { get; set; }

        [OneTimeSetUp]
        public void SetupDependencies()
        {
            var configurationManager = new ConfigurationManagerAdapter();
            Configuration = new AppConfiguration(configurationManager);
        }

        [SetUp]
        public void SetUp()
        {
            Fake<SiteInfo, SiteInfoProvider>().WithData(
                new SiteInfo
                {
                    SiteName = "Site",
                    SiteID = 1
                }
            );
        }

        [TestCase("Key", "Site", null, true)]
        [TestCase("Key", "Site", null, false)]
        [TestCase("Key", "Site", "Category", true)]
        [TestCase("Key", "Site", "Category", false)]
        [TestCase("Category:Key", "Site", null, true)]
        [TestCase("Category:Key", "Site", null, false)]
        public void CreateConfigKeyName_WithSiteName_StartsWithSiteNameSection(string settingsKeyName, string siteName, string categoryName, bool useCategorySections)
        {
            var options = new CmsSettingsConfigBuilderOptions
            {
                UseCategorySections = useCategorySections
            };

            var sut = new CmsSettingConfigKeyNameFactory(Configuration, options);

            var setting = new CmsSetting(settingsKeyName, string.Empty, categoryName, siteName);
            string actual = sut.CreateConfigKeyName(setting);

            Assert.That(
                actual,
                Does.StartWith($"{siteName}{AppConfigurationPath.KeyDelimiter}")
            );
        }

        [TestCase("Key", null, "Category", true)]
        [TestCase("Key", "Site", "Category", true)]
        [TestCase("Category:Key", null, null, true)]
        [TestCase("Category:Key", null, null, false)]
        [TestCase("Category:Key", "Site", null, true)]
        [TestCase("Category:Key", "Site", null, false)]
        public void CreateConfigKeyName_WithCategoryName_ContainsCategoryNameSection(string settingsKeyName, string siteName, string categoryName, bool useCategorySections)
        {
            var options = new CmsSettingsConfigBuilderOptions
            {
                UseCategorySections = useCategorySections
            };

            var sut = new CmsSettingConfigKeyNameFactory(Configuration, options);

            var setting = new CmsSetting(settingsKeyName, string.Empty, categoryName, siteName);
            string actual = sut.CreateConfigKeyName(setting);

            Assert.That(
                actual,
                Does.Contain($"{categoryName}{AppConfigurationPath.KeyDelimiter}")
            );
        }

        [TestCase("", "", "Prefix", "Key", false, false)]
        [TestCase("", "", "Prefix", "Key", true, false)]
        [TestCase("", "Category", "Prefix", "Key", false, false)]
        [TestCase("", "Category", "Prefix", "Key", true, false)]
        [TestCase("", "Category", "Prefix", "Key", false, true)]
        [TestCase("", "Category", "Prefix", "Key", true, true)]
        [TestCase("Site", "Category", "Prefix", "Key", false, false)]
        [TestCase("Site", "Category", "Prefix", "Key", true, false)]
        [TestCase("Site", "Category", "Prefix", "Key", false, true)]
        [TestCase("Site", "Category", "Prefix", "Key", true, true)]
        public void CreateConfigKeyName_WithKeyPrefix_StripsPrefixWhenDirected(string siteName, string categoryName, string keyPrefix, string keyName, bool stripPrefix, bool useCategorySections)
        {
            var options = new CmsSettingsConfigBuilderOptions
            {
                UseCategorySections = useCategorySections,
                KeyPrefix = keyPrefix,
                StripPrefix = stripPrefix
            };

            var sut = new CmsSettingConfigKeyNameFactory(Configuration, options);

            string settingsKeyName = $"{keyPrefix}{keyName}";

            var setting = new CmsSetting(settingsKeyName, string.Empty, categoryName, siteName);
            string actual = sut.CreateConfigKeyName(setting);

            Assert.That(
                actual,
                stripPrefix ? Does.Not.Contain(keyPrefix) : Does.Contain(keyPrefix)
            );
        }

        [TestCase("Key", "Site", null, true)]
        [TestCase("Key", "Site", null, false)]
        [TestCase("Key", "Site", "Category", true)]
        [TestCase("Key", "Site", "Category", false)]
        public void CreateSettingsKeyName_WithSiteConfigKey_CreatesSiteSettingsKey(string settingsKeyName,
            string siteName, string categoryName, bool useCategorySections)
        {
            var options = new CmsSettingsConfigBuilderOptions
            {
                UseCategorySections = useCategorySections
            };

            var sut = new CmsSettingConfigKeyNameFactory(Configuration, options);

            var setting = new CmsSetting(settingsKeyName, string.Empty, categoryName, siteName);
            string configKey = sut.CreateConfigKeyName(setting);

            SettingsKeyName actual = sut.CreateSettingsKeyName(configKey);

            Assert.Multiple(() =>
            {
                Assert.That(actual.KeyName, Is.EqualTo(settingsKeyName));
                Assert.That(actual.SiteName, Is.EqualTo(siteName));
                Assert.IsFalse(actual.IsGlobal);
            });
        }

        [TestCase("", "", "Prefix", "Key", false, false)]
        [TestCase("", "", "Prefix", "Key", true, false)]
        [TestCase("", "Category", "Prefix", "Key", false, false)]
        [TestCase("", "Category", "Prefix", "Key", true, false)]
        [TestCase("", "Category", "Prefix", "Key", false, true)]
        [TestCase("", "Category", "Prefix", "Key", true, true)]
        [TestCase("Site", "Category", "Prefix", "Key", false, false)]
        [TestCase("Site", "Category", "Prefix", "Key", true, false)]
        [TestCase("Site", "Category", "Prefix", "Key", false, true)]
        [TestCase("Site", "Category", "Prefix", "Key", true, true)]
        public void CreateSettingsKeyName_WithKeyPrefix_RestoresPrefixWhenStripped(string siteName, string categoryName, string keyPrefix, string keyName, bool stripPrefix, bool useCategorySections)
        {
            var options = new CmsSettingsConfigBuilderOptions
            {
                UseCategorySections = useCategorySections,
                KeyPrefix = keyPrefix,
                StripPrefix = stripPrefix
            };

            var sut = new CmsSettingConfigKeyNameFactory(Configuration, options);

            string settingsKeyName = $"{keyPrefix}{keyName}";

            var setting = new CmsSetting(settingsKeyName, string.Empty, categoryName, siteName);
            string configKey = sut.CreateConfigKeyName(setting);
            
            SettingsKeyName actual = sut.CreateSettingsKeyName(configKey);

            Assert.That(
                actual.KeyName,
                Does.StartWith(keyPrefix)
            );
        }

        [TestCase("Key", null, null, true)]
        [TestCase("Key", null, null, false)]
        [TestCase("Key", "", null, true)]
        [TestCase("Key", "", null, false)]
        [TestCase("Key", null, "Category", true)]
        [TestCase("Key", null, "Category", false)]
        public void CreateSettingsKeyName_WithGlobalConfigKey_CreatesGlobalSettingsKey(string settingsKeyName, string siteName, string categoryName, bool useCategorySections)
        {
            var options = new CmsSettingsConfigBuilderOptions
            {
                UseCategorySections = useCategorySections
            };

            var sut = new CmsSettingConfigKeyNameFactory(Configuration, options);

            var setting = new CmsSetting(settingsKeyName, string.Empty, categoryName, siteName);
            string configKey = sut.CreateConfigKeyName(setting);

            SettingsKeyName actual = sut.CreateSettingsKeyName(configKey);

            Assert.Multiple(() =>
            {
                Assert.That(actual.KeyName, Is.EqualTo(settingsKeyName));
                Assert.That(actual.SiteName, Is.Empty);
                Assert.IsTrue(actual.IsGlobal);
            });
        }
    }
}
