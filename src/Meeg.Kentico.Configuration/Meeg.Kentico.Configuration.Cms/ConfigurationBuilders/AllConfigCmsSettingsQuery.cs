using System;
using System.Collections.Generic;

namespace Meeg.Kentico.Configuration.Cms.ConfigurationBuilders
{
    internal class AllConfigCmsSettingsQuery : IQuery<IReadOnlyCollection<CmsSetting>>
    {
        public string ProcName { get; }
        public string Prefix { get; }

        public AllConfigCmsSettingsQuery(string procName, string prefix)
        {
            if (string.IsNullOrEmpty(procName))
            {
                throw new ArgumentException("Please provide the name of the stored procedure that will query the database for CMS settings.", nameof(procName));
            }

            ProcName = procName;
            Prefix = prefix;
        }
    }
}
