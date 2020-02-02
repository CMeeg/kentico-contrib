using System.Collections.Generic;
using System.Linq;

namespace Meeg.Kentico.Configuration.Cms.Sql
{
    internal class SqlQuery
    {
        public string QueryText { get; }
        public IEnumerable<SqlQueryParameter> Parameters { get; }

        public SqlQuery(string queryText)
            : this(queryText, null)
        {
        }

        public SqlQuery(string queryText, IEnumerable<SqlQueryParameter> parameters)
        {
            QueryText = queryText;
            Parameters = parameters ?? Enumerable.Empty<SqlQueryParameter>();
        }

        public SqlQuery(string queryText, params SqlQueryParameter[] parameters)
        {
            QueryText = string.Format(
                queryText,
                args: parameters.Select(parameter => parameter.Name as object).ToArray()
            );

            Parameters = parameters;
        }
    }
}
