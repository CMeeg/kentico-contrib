using System;
using System.Data.Common;

namespace Meeg.Kentico.Configuration.Cms.Sql
{
    internal class SqlQueryDataReader : IDisposable
    {
        private readonly DbDataReader dataReader;

        public bool HasRows => dataReader.HasRows;

        public SqlQueryDataReader(DbDataReader dataReader)
        {
            this.dataReader = dataReader;
        }

        public bool Read()
        {
            return dataReader.Read();
        }

        public string GetString(int ordinal)
        {
            return dataReader.IsDBNull(ordinal) ? null : dataReader.GetString(ordinal);
        }

        public void Dispose()
        {
            dataReader?.Dispose();
        }
    }
}
