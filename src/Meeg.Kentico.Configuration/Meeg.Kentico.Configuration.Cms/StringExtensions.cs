using System;

namespace Meeg.Kentico.Configuration.Cms
{
    internal static class StringExtensions
    {
        public static string[] SplitOnLastIndexOf(this string source, string value)
        {
            return SplitOnLastIndexOf(source, value, StringComparison.CurrentCulture);
        }

        public static string[] SplitOnLastIndexOf(this string source, string value, StringComparison comparisonType)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(value))
            {
                return new[] { source };
            }

            int lastSeparatorIndex = source.LastIndexOf(value, comparisonType);

            if (lastSeparatorIndex == -1)
            {
                return new[] { source };
            }

            string leftPart = source.Substring(0, lastSeparatorIndex);
            string rightPart = lastSeparatorIndex == source.Length
                ? string.Empty
                : source.Substring(lastSeparatorIndex + 1);

            return new[]
            {
                leftPart,
                rightPart
            };
        }
    }
}
