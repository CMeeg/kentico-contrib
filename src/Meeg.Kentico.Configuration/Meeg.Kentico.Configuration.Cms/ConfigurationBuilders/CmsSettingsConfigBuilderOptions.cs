namespace Meeg.Kentico.Configuration.Cms.ConfigurationBuilders
{
    internal class CmsSettingsConfigBuilderOptions
    {
        public const string DefaultProcName = "Proc_Meeg_Configuration_AllConfigCmsSettings";

        private string procName;
        public string ProcName
        {
            get => procName ?? DefaultProcName;
            set => procName = value;
        }

        public bool UseCategorySections { get; set; }
        public string KeyPrefix { get; set; }
        public bool StripPrefix { get; set; }
        public bool Optional { get; set; }
    }
}
