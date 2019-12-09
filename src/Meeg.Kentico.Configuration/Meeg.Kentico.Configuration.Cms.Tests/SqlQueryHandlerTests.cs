using Meeg.Configuration;
using Meeg.Kentico.Configuration.Cms.Sql;
using NUnit.Framework;

namespace Meeg.Kentico.Configuration.Cms.Tests
{
    public class SqlQueryHandlerTests
    {
        internal ISqlQueryExecutor SqlQueryExecutor { get; private set; }

        [OneTimeSetUp]
        public void SetupDepenencies()
        {
            var configuration = new AppConfiguration();
            SqlQueryExecutor = new SqlQueryExecutor(configuration);
        }
    }
}
