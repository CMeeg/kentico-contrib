using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace KenticoContrib.Content.Cms.Infrastructure.AutoMapper
{
    public abstract class CmsMappingProfile : Profile
    {
        private const string DocumentPrefix = "Document";

        protected string[] GetCmsPrefixes(params string[] classNames)
        {
            var prefixes = new List<string>();

            prefixes.Add(DocumentPrefix);

            prefixes.AddRange(classNames.Select(GetClassNamePrefix));

            return prefixes.ToArray();
        }

        private string GetClassNamePrefix(string className)
        {
            if (string.IsNullOrEmpty(className))
            {
                throw new ArgumentOutOfRangeException(nameof(className), "Value cannot be null or empty.");
            }

            string[] classNameParts = className.Split('.');
            int numParts = classNameParts.Length;

            return numParts == 1 ? classNameParts[0] : classNameParts[numParts - 1];
        }
    }
}
