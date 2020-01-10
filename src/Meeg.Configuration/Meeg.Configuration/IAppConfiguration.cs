using System.Collections.Generic;

namespace Meeg.Configuration
{
    public interface IAppConfiguration
    {
        string this[string key] { get; }

        IAppConfigurationSection GetSection(string key);

        IEnumerable<IAppConfigurationSection> GetChildren();
    }
}
