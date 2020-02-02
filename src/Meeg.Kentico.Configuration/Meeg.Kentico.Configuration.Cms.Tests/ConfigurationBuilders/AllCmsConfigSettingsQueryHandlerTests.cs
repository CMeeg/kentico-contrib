using System.Collections.Generic;
using System.Linq;
using Meeg.Kentico.Configuration.Cms.ConfigurationBuilders;
using NUnit.Framework;

namespace Meeg.Kentico.Configuration.Cms.Tests.ConfigurationBuilders
{
    [TestFixture]
    public class AllCmsConfigSettingsQueryHandlerTests : SqlQueryHandlerTestsBase
    {
        [TestCase(CmsSettingsConfigBuilderOptions.DefaultQueryName)]
        public void Handle_WithValidQueryName_ReturnsSettings(string queryName)
        {
            var query = new AllConfigCmsSettingsQuery(queryName, null);
            var queryHandler = new AllConfigCmsSettingsQueryHandler(SqlQueryExecutor);
            IReadOnlyCollection<CmsSetting> settings = queryHandler.Handle(query);

            CollectionAssert.IsNotEmpty(settings);
        }

        [TestCase(CmsSettingsConfigBuilderOptions.DefaultQueryName, "CMSSMTP")]
        public void Handle_WithValidQueryNameAndPrefix_ReturnsSettingsWithPrefix(string queryName, string prefix)
        {
            var query = new AllConfigCmsSettingsQuery(queryName, prefix);
            var queryHandler = new AllConfigCmsSettingsQueryHandler(SqlQueryExecutor);
            IReadOnlyCollection<CmsSetting> settings = queryHandler.Handle(query);

            Assert.That(() => settings.All(setting => setting.Name.StartsWith(prefix)));
        }
    }
}
