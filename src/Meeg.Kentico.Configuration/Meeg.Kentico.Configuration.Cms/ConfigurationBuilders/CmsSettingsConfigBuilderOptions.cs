namespace Meeg.Kentico.Configuration.Cms.ConfigurationBuilders
{
    internal class CmsSettingsConfigBuilderOptions
    {
        public const string DefaultQueryName = "Meeg_Kentico_Configuration.QueryContainer.AllConfigCmsSettings";

        private string queryName;
        public string QueryName
        {
            get => queryName ?? DefaultQueryName;
            set => queryName = value;
        }

        public bool UseCategorySections { get; set; }
        public string KeyPrefix { get; set; }
        public bool StripPrefix { get; set; }
        public bool Optional { get; set; }
    }
}
