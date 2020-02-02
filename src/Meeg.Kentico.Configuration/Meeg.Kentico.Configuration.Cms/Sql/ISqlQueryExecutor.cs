using System;
using System.Collections.Generic;

namespace Meeg.Kentico.Configuration.Cms.Sql
{
    internal interface ISqlQueryExecutor
    {
        IReadOnlyCollection<TResult> ExecuteReader<TResult>(SqlQuery query, string connectionStringName,
            Func<SqlQueryDataReader, TResult> createResult);
    }
}
