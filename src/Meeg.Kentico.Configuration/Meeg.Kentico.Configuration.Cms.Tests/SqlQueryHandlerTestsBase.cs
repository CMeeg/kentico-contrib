using Meeg.Configuration;
using Meeg.Kentico.Configuration.Cms.Sql;
using NUnit.Framework;

namespace Meeg.Kentico.Configuration.Cms.Tests
{
    public abstract class SqlQueryHandlerTestsBase
    {
        internal ISqlQueryExecutor SqlQueryExecutor { get; private set; }

        [OneTimeSetUp]
        public void SetupDependencies()
        {
            var configurationManager = new ConfigurationManagerAdapter();
            var configuration = new AppConfiguration(configurationManager);
            SqlQueryExecutor = new SqlQueryExecutor(configuration);
        }
    }
}
