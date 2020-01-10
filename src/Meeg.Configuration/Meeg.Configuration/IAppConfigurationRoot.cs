namespace Meeg.Configuration
{
    public interface IAppConfigurationRoot : IAppConfiguration
    {
        string[] AllKeys { get; }
    }
}
