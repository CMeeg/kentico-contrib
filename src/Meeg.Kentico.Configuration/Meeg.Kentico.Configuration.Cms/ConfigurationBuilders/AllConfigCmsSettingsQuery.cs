using System;
using System.Collections.Generic;

namespace Meeg.Kentico.Configuration.Cms.ConfigurationBuilders
{
    internal class AllConfigCmsSettingsQuery : IQuery<IReadOnlyCollection<CmsSetting>>
    {
        public string QueryName { get; }
        public string Prefix { get; }

        public AllConfigCmsSettingsQuery(string queryName, string prefix)
        {
            if (string.IsNullOrEmpty(queryName))
            {
                throw new ArgumentException("Please provide the name of the CMS query to execute.", nameof(queryName));
            }

            QueryName = queryName;
            Prefix = prefix;
        }
    }
}
