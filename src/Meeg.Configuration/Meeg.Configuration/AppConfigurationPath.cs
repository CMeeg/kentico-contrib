using System;

namespace Meeg.Configuration
{
    public static class AppConfigurationPath
    {
        public const string KeyDelimiter = ":";

        public static string GetSectionKey(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            int lastDelimiterIndex = path.LastIndexOf(
                KeyDelimiter,
                StringComparison.OrdinalIgnoreCase
            );

            return lastDelimiterIndex == -1
                ? path
                : path.Substring(lastDelimiterIndex + 1);
        }

        public static string Combine(params string[] pathSegments)
        {
            if (pathSegments == null)
            {
                throw new ArgumentNullException(nameof(pathSegments));
            }
            return string.Join(KeyDelimiter, pathSegments);
        }
    }
}
