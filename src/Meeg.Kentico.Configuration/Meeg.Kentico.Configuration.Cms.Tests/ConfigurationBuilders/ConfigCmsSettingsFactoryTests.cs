using CMS.DataEngine;
using Meeg.Kentico.Configuration.Cms.ConfigurationBuilders;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Meeg.Kentico.Configuration.Cms.Tests.ConfigurationBuilders
{
    [TestFixture]
    public class ConfigCmsSettingsFactoryTests
    {
        [TestCase("Key", "Site")]
        public void CreateConfigKeyName_WithSiteName_HasSiteNameSuffix(string settingsKeyName, string siteName)
        {
            var sut = new ConfigCmsSettingsFactory();

            string actual = sut.CreateConfigKeyName(settingsKeyName, siteName);

            Assert.That(actual, new EndsWithConstraint(siteName));
        }

        [TestCase("Key", "Site")]
        public void CreateSettingsKeyName_WithSiteConfigKey_CreatesSiteSettingsKey(string settingsKeyName,
            string siteName)
        {
            var sut = new ConfigCmsSettingsFactory();

            string configKey = sut.CreateConfigKeyName(settingsKeyName, siteName);

            SettingsKeyName actual = sut.CreateSettingsKeyName(configKey);

            Assert.Multiple(() =>
            {
                Assert.That(actual.KeyName, Is.EqualTo(settingsKeyName));
                Assert.That(actual.SiteName, Is.EqualTo(siteName));
                Assert.IsFalse(actual.IsGlobal);
            });
        }

        [TestCase("Key", null)]
        [TestCase("Key", "")]
        public void CreateSettingsKeyName_WithGlobalConfigKey_CreatesGlobalSettingsKey(string settingsKeyName, string siteName)
        {
            var sut = new ConfigCmsSettingsFactory();

            string configKey = sut.CreateConfigKeyName(settingsKeyName, siteName);

            SettingsKeyName actual = sut.CreateSettingsKeyName(configKey);

            Assert.Multiple(() =>
            {
                Assert.That(actual.KeyName, Is.EqualTo(settingsKeyName));
                Assert.That(actual.SiteName, Is.Empty);
                Assert.IsTrue(actual.IsGlobal);
            });
        }

        //var query = new AllCmsConfigSettingsQuery("AllCmsConfigSettings");

        //var expected = Enumerable.Range(0, numSettings)
        //    .Select(count => new CmsSetting($"Name{count}", $"Value{count}", $"SiteName{count}"))
        //    .ToList()
        //    .AsReadOnly();

        //var cmsQueryExecutor = A.Fake<ICmsQueryExecutor<CmsSetting>>();
        //A.CallTo(() => cmsQueryExecutor.ExecuteQuery(query))
        //    .Returns(expected);

        //var actual = new AllCmsConfigSettingsQueryHandler(cmsQueryExecutor)
        //    .Handle(query);

        //CollectionAssert.AreEqual(expected, actual);
    }
}
