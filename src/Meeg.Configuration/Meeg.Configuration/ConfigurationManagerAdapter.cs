using System.Collections.Specialized;
using System.Configuration;

namespace Meeg.Configuration
{
    public class ConfigurationManagerAdapter : IConfigurationManager
    {
        private NameValueCollection connectionStrings;

        public NameValueCollection AppSettings => ConfigurationManager.AppSettings;

        public NameValueCollection ConnectionStrings
        {
            get
            {
                if (connectionStrings == null)
                {
                    connectionStrings = new NameValueCollection();

                    foreach (ConnectionStringSettings connectionString in ConfigurationManager.ConnectionStrings)
                    {
                        connectionStrings.Add(connectionString.Name, connectionString.ConnectionString);
                    }
                }

                return connectionStrings;
            }
        }
    }
}
