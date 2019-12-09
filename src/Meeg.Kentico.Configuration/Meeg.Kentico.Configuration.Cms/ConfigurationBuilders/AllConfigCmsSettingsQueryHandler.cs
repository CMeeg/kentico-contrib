using System;
using System.Collections.Generic;
using Meeg.Kentico.Configuration.Cms.Sql;

namespace Meeg.Kentico.Configuration.Cms.ConfigurationBuilders
{
    internal class AllConfigCmsSettingsQueryHandler : IQueryHandler<AllConfigCmsSettingsQuery, IReadOnlyCollection<CmsSetting>>
    {
        private readonly ISqlQueryExecutor sqlQueryExecutor;

        public AllConfigCmsSettingsQueryHandler(ISqlQueryExecutor sqlQueryExecutor)
        {
            this.sqlQueryExecutor = sqlQueryExecutor ?? throw new ArgumentNullException(nameof(sqlQueryExecutor));
        }

        public IReadOnlyCollection<CmsSetting> Handle(AllConfigCmsSettingsQuery query)
        {
            // Using Kentico's InfoProvider, ObjectQuery, DataQuery, ConnectionHelper all result in calls to get appSettings, infinite loops and stack overflows - so we will use ADO.NET directly

            CmsQuery cmsQuery = GetConfigSettingsQuery(query.QueryName);

            if (cmsQuery == null)
            {
                throw new ArgumentException($"Query could not be found: `{query.QueryName}`.");
            }

            string prefix = query.Prefix ?? string.Empty;

            const string keyNamePrefixParam = "@KeyNamePrefix";

            var sqlQuery = new SqlQuery(
                cmsQuery.Text,
                new []
                {
                    new SqlQueryParameter(keyNamePrefixParam, $"{prefix}%")
                }
            );

            return sqlQueryExecutor.ExecuteReader(
                sqlQuery,
                cmsQuery.ConnectionStringName,
                dataReader => new CmsSetting(
                    dataReader.GetString(0),
                    dataReader.GetString(1),
                    dataReader.GetString(2)
                )
            );
        }

        private CmsQuery GetConfigSettingsQuery(string queryName)
        {
            var query = new FindCmsQueryByNameQuery(queryName);
            var queryHandler = new FindCmsQueryByNameQueryHandler(sqlQueryExecutor);
            return queryHandler.Handle(query);
        }
    }
}
