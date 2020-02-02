using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Meeg.Configuration;

namespace Meeg.Kentico.Configuration.Cms.Sql
{
    internal class SqlQueryExecutor : ISqlQueryExecutor
    {
        private readonly IAppConfiguration configuration;

        public SqlQueryExecutor(IAppConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IReadOnlyCollection<TResult> ExecuteReader<TResult>(SqlQuery query, string connectionStringName, Func<SqlQueryDataReader, TResult> createResult)
        {
            var results = new List<TResult>();

            string connectionString = configuration.GetConnectionString(connectionStringName);

            if (string.IsNullOrEmpty(connectionStringName))
            {
                throw new ArgumentException($"Connection string does not exist: `{connectionStringName}`.", nameof(connectionStringName));
            }

            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand command = CreateSqlCommand(query, connection);

                connection.Open();

                using (var dataReader = new SqlQueryDataReader(command.ExecuteReader()))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            TResult result = createResult.Invoke(dataReader);

                            if (result == null)
                            {
                                continue;
                            }

                            results.Add(result);
                        }
                    }
                }
            }

            return results.AsReadOnly();
        }

        private SqlCommand CreateSqlCommand(SqlQuery query, SqlConnection connection)
        {
            var command = new SqlCommand(query.QueryText, connection);

            foreach (SqlQueryParameter parameter in query.Parameters)
            {
                string parameterName = parameter.Name.StartsWith("@")
                    ? parameter.Name
                    : $"@{parameter.Name}";

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = parameterName,
                    Value = parameter.Value
                });
            }

            return command;
        }
    }
}
