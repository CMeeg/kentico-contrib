using Meeg.Kentico.Configuration.Cms.ConfigurationBuilders;
using NUnit.Framework;

namespace Meeg.Kentico.Configuration.Cms.Tests
{
    [TestFixture]
    public class FindCmsQueryByNameQueryHandlerTests : SqlQueryHandlerTests
    {
        [TestCase("InvalidClass.InvalidQuery")]
        public void Handle_WithInvalidQueryName_ReturnsNull(string queryName)
        {
            var query = new FindCmsQueryByNameQuery(queryName);
            var handler = new FindCmsQueryByNameQueryHandler(SqlQueryExecutor);
            var actual = handler.Handle(query);

            Assert.That(actual, Is.Null);
        }

        [TestCase(CmsSettingsConfigBuilder.DefaultQueryName)]
        public void Handle_WithValidQueryName_ReturnsCmsQuery(string queryName)
        {
            const string settingsTableName = "CMS_SettingsKey";

            var query = new FindCmsQueryByNameQuery(queryName);
            var handler = new FindCmsQueryByNameQueryHandler(SqlQueryExecutor);
            var actual = handler.Handle(query);

            Assert.That(actual.Text, Contains.Substring(settingsTableName));
        }
    }
}
