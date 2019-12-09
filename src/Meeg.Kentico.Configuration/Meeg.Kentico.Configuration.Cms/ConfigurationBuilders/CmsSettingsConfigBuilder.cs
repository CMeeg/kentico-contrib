using System.Collections.Generic;
using System.Collections.Specialized;
using CMS.DataEngine;
using Meeg.Configuration;
using Meeg.Kentico.Configuration.Cms.Sql;
using Microsoft.Configuration.ConfigurationBuilders;

namespace Meeg.Kentico.Configuration.Cms.ConfigurationBuilders
{
    public class CmsSettingsConfigBuilder : KeyValueConfigBuilder
    {
        public const string DefaultQueryName = "Meeg_Kentico_Configuration.QueryContainer.AllConfigCmsSettings";
        private const string QueryNameTag = "queryName";

        protected string QueryName { get; private set; }

        protected ConfigCmsSettingsFactory ConfigSettingsFactory { get; private set; }

        public override void Initialize(string name, NameValueCollection config)
        {
            QueryName = config[QueryNameTag] ?? DefaultQueryName;

            ConfigSettingsFactory = new ConfigCmsSettingsFactory();

            base.Initialize(name, config);
        }

        public override string GetValue(string key)
        {
            // Any prefix will be added back to the `key` if it has been stripped so we don't need to handle that here - we do need to handle site vs global settings though

            SettingsKeyName settingsKeyName = ConfigSettingsFactory.CreateSettingsKeyName(key);

            return SettingsKeyInfoProvider.GetValue(settingsKeyName);
        }

        public override ICollection<KeyValuePair<string, string>> GetAllValues(string prefix)
        {
            var configuration = new AppConfiguration();
            var sqlQueryExecutor = new SqlQueryExecutor(configuration);

            var query = new AllConfigCmsSettingsQuery(QueryName, prefix);
            var queryHandler = new AllConfigCmsSettingsQueryHandler(sqlQueryExecutor);
            IReadOnlyCollection<CmsSetting> allConfigSettings = queryHandler.Handle(query);

            return ConfigSettingsFactory.CreateConfigSettings(allConfigSettings);
        }
    }
}
