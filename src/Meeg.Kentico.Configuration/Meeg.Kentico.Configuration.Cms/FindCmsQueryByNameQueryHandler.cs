using System.Linq;
using CMS.DataEngine;
using Meeg.Kentico.Configuration.Cms.Sql;

namespace Meeg.Kentico.Configuration.Cms
{
    internal class FindCmsQueryByNameQueryHandler : IQueryHandler<FindCmsQueryByNameQuery, CmsQuery>
    {
        private readonly ISqlQueryExecutor sqlQueryExecutor;

        public FindCmsQueryByNameQueryHandler(ISqlQueryExecutor sqlQueryExecutor)
        {
            this.sqlQueryExecutor = sqlQueryExecutor;
        }

        public CmsQuery Handle(FindCmsQueryByNameQuery query)
        {
            SqlQuery sqlQuery = GetSqlQuery(query);

            return sqlQueryExecutor.ExecuteReader(
                sqlQuery,
                ConnectionHelper.DEFAULT_CONNECTIONSTRING_NAME,
                dataReader => new CmsQuery(
                    dataReader.GetString(0),
                    dataReader.GetString(1)
                )
            ).FirstOrDefault();
        }

        private SqlQuery GetSqlQuery(FindCmsQueryByNameQuery query)
        {
            const string classNameParam = "@ClassName";
            const string queryNameParam = "@QueryName";

            const string queryText = @"SELECT
	            QueryText,
	            QueryConnectionString
            FROM
	            CMS_Query q
            INNER JOIN
	            CMS_Class c ON q.ClassID = c.ClassID
            WHERE
	            ClassName = {0}
            AND
	            QueryName = {1}";

            return new SqlQuery(
                queryText,
                new SqlQueryParameter(classNameParam, query.ClassName),
                new SqlQueryParameter(queryNameParam, query.QueryName)
            );
        }
    }
}
