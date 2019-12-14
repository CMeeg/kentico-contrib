namespace Meeg.Kentico.Configuration.Cms.ConfigurationBuilders
{
    internal class CmsSettingsConfigBuilderOptions
    {
        public const string DefaultQueryName = "Meeg_Kentico_Configuration.QueryContainer.AllConfigCmsSettings";

        public string QueryName { get; }
        public bool UseCategorySections { get; }

        public CmsSettingsConfigBuilderOptions(string queryName, bool useCategorySections)
        {
            QueryName = queryName ?? DefaultQueryName;
            UseCategorySections = useCategorySections;
        }
    }
}
