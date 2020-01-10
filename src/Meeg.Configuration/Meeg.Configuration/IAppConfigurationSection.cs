namespace Meeg.Configuration
{
    public interface IAppConfigurationSection : IAppConfiguration
    {
        string Key { get; }

        string Value { get; }

        string Path { get; }
    }
}
