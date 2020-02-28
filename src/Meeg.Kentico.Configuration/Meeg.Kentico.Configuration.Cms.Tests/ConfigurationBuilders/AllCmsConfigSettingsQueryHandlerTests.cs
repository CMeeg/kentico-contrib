using System.Collections.Generic;
using System.Linq;
using Meeg.Kentico.Configuration.Cms.ConfigurationBuilders;
using NUnit.Framework;

namespace Meeg.Kentico.Configuration.Cms.Tests.ConfigurationBuilders
{
    [TestFixture]
    public class AllCmsConfigSettingsQueryHandlerTests : SqlQueryHandlerTestsBase
    {
        [TestCase(CmsSettingsConfigBuilderOptions.DefaultProcName)]
        public void Handle_WithValidProcName_ReturnsSettings(string procName)
        {
            var query = new AllConfigCmsSettingsQuery(procName, null);
            var queryHandler = new AllConfigCmsSettingsQueryHandler(SqlQueryExecutor);
            IReadOnlyCollection<CmsSetting> settings = queryHandler.Handle(query);

            CollectionAssert.IsNotEmpty(settings);
        }

        [TestCase(CmsSettingsConfigBuilderOptions.DefaultProcName, "CMSSMTP")]
        public void Handle_WithValidProcNameAndPrefix_ReturnsSettingsWithPrefix(string procName, string prefix)
        {
            var query = new AllConfigCmsSettingsQuery(procName, prefix);
            var queryHandler = new AllConfigCmsSettingsQueryHandler(SqlQueryExecutor);
            IReadOnlyCollection<CmsSetting> settings = queryHandler.Handle(query);

            Assert.That(() => settings.All(setting => setting.Name.StartsWith(prefix)));
        }
    }
}
