using System.Collections.Specialized;

namespace Meeg.Configuration
{
    public interface IConfigurationManager
    {
        NameValueCollection AppSettings { get; }

        NameValueCollection ConnectionStrings { get; }
    }
}
