using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FakeItEasy;
using Meeg.Configuration;
using Meeg.Kentico.Configuration.Cms.ConfigurationBuilders;
using NUnit.Framework;

namespace Meeg.Kentico.Configuration.Cms.Tests.ConfigurationBuilders
{
    [TestFixture]
    public class CmsSettingsConfigBuilderTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void CreateConfigSettings_WithCategorySections_PreservesGlobalSettingsForSites(bool useCategorySections)
        {
            var globalSetting = new CmsSetting("Key1", "Value1", "Category1", null);
            var siteSetting = new CmsSetting("Key2", "Value2", "Category1", "Site");

            ReadOnlyCollection<CmsSetting> settings = new List<CmsSetting>
            {
                globalSetting,
                siteSetting
            }
            .AsReadOnly();

            IQueryHandler<AllConfigCmsSettingsQuery, IReadOnlyCollection<CmsSetting>> allSettingsQueryHandler = A.Fake<IQueryHandler<AllConfigCmsSettingsQuery, IReadOnlyCollection<CmsSetting>>>();

            A.CallTo(() => allSettingsQueryHandler.Handle(A<AllConfigCmsSettingsQuery>.Ignored))
                .Returns(settings);

            var configuration = new AppConfiguration();
            var options = new CmsSettingsConfigBuilderOptions(null, useCategorySections, null, false);
            var configKeyNameFactory = new CmsSettingConfigKeyNameFactory(configuration, options);

            var sut = new CmsSettingsConfigBuilderInternal(
                options,
                allSettingsQueryHandler,
                configKeyNameFactory
            );

            ICollection<KeyValuePair<string, string>> values = sut.GetAllValues(null);

            // If a category has at least one Site setting value, all other keys in that category should be made available within the corresponding Site "section" by falling back to Global settings values where a Site specific value is not available

            string globalSettingKey = configKeyNameFactory.CreateConfigKeyName(globalSetting);
            string expected = globalSetting.Value;

            KeyValuePair<string, string> configSetting = values.FirstOrDefault(setting => setting.Key == globalSettingKey);
            string actual = configSetting.Value;

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
