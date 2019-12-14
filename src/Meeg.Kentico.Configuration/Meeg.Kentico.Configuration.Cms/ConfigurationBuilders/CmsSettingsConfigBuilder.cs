using System.Collections.Generic;
using System.Collections.Specialized;
using Meeg.Configuration;
using Meeg.Kentico.Configuration.Cms.Sql;
using Microsoft.Configuration.ConfigurationBuilders;

namespace Meeg.Kentico.Configuration.Cms.ConfigurationBuilders
{
    public class CmsSettingsConfigBuilder : KeyValueConfigBuilder
    {
        private const string QueryNameTag = "queryName";
        private const string UseCategorySectionsTag = "useCategorySections";

        protected string QueryName { get; private set; }

        protected bool UseCategorySections { get; private set; }

        internal CmsSettingsConfigBuilderInternal ConfigBuilder { get; private set; }

        public override void Initialize(string name, NameValueCollection config)
        {
            QueryName = config[QueryNameTag];

            bool.TryParse(config[UseCategorySectionsTag] ?? "True", out bool useCategorySections);
            UseCategorySections = useCategorySections;

            // This class serves as an adapter for the actual config builder impl
            // Config builders are based on the provider model and so we can't inject dependencies
            // So we will compose our implementation and its dependencies here

            var configuration = new AppConfiguration();
            var sqlQueryExecutor = new SqlQueryExecutor(configuration);
            var allSettingsQueryHandler = new AllConfigCmsSettingsQueryHandler(sqlQueryExecutor);
            var configKeyNameFactory = new CmsSettingConfigKeyNameFactory(configuration, UseCategorySections);

            ConfigBuilder = new CmsSettingsConfigBuilderInternal(
                new CmsSettingsConfigBuilderOptions(QueryName, UseCategorySections),
                allSettingsQueryHandler,
                configKeyNameFactory
            );

            base.Initialize(name, config);
        }

        public override string GetValue(string key)
        {
            return ConfigBuilder.GetValue(key);
        }

        public override ICollection<KeyValuePair<string, string>> GetAllValues(string prefix)
        {
            return ConfigBuilder.GetAllValues(prefix);
        }
    }
}
