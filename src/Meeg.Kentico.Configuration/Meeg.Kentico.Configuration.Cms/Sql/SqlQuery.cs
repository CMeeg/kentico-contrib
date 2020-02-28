using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Meeg.Kentico.Configuration.Cms.Sql
{
    internal class SqlQuery
    {
        public string QueryText { get; }
        public CommandType CommandType { get; }
        public IEnumerable<SqlQueryParameter> Parameters { get; }

        public SqlQuery(string queryText)
            : this(queryText, CommandType.Text, null)
        {
        }

        public SqlQuery(string queryText, CommandType commandType)
            : this(queryText, commandType, null)
        {
        }

        public SqlQuery(string queryText, CommandType commandType, IEnumerable<SqlQueryParameter> parameters)
        {
            QueryText = queryText;
            CommandType = commandType;
            Parameters = parameters ?? Enumerable.Empty<SqlQueryParameter>();
        }

        public SqlQuery(string queryText, params SqlQueryParameter[] parameters)
            : this(queryText, CommandType.Text, parameters)
        {
        }

        public SqlQuery(string queryText, CommandType commandType, params SqlQueryParameter[] parameters)
        {
            QueryText = string.Format(
                queryText,
                args: parameters.Select(parameter => parameter.Name as object).ToArray()
            );

            CommandType = commandType;

            Parameters = parameters;
        }
    }
}
