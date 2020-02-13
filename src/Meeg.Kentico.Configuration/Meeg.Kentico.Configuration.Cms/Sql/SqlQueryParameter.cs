namespace Meeg.Kentico.Configuration.Cms.Sql
{
    internal class SqlQueryParameter
    {
        public string Name { get; }
        public object Value { get; }

        public SqlQueryParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
