namespace Meeg.Kentico.Configuration.Cms
{
    internal class CmsQuery
    {
        public string Text { get; }
        public string ConnectionStringName { get; }

        public CmsQuery(
            string text,
            string connectionStringName
        )
        {
            Text = text;
            ConnectionStringName = connectionStringName;
        }
    }
}
