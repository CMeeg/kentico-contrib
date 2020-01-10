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

            bool.TryParse(config[UseCategorySectionsTag] ?? "false", out bool useCategorySections);
            UseCategorySections = useCategorySections;

            string keyPrefix = config[prefixTag] ?? string.Empty;

            bool.TryParse(config[stripPrefixTag] ?? "false", out bool stripPrefix);

            // This class serves as an adapter for the actual config builder impl
            // Config builders are based on the provider model and so we can't inject dependencies
            // So we will compose our implementation and its dependencies here

            var options = new CmsSettingsConfigBuilderOptions(QueryName, UseCategorySections, keyPrefix, stripPrefix);

            var configurationManager = new ConfigurationManagerAdapter();
            var configuration = new AppConfiguration(configurationManager);
            var sqlQueryExecutor = new SqlQueryExecutor(configuration);
            var allSettingsQueryHandler = new AllConfigCmsSettingsQueryHandler(sqlQueryExecutor);

            var configKeyNameFactory = new CmsSettingConfigKeyNameFactory(configuration, options);

            ConfigBuilder = new CmsSettingsConfigBuilderInternal(
                options,
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
