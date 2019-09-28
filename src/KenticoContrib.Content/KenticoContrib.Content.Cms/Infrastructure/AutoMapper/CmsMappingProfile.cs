using System;
using AutoMapper;

namespace KenticoContrib.Content.Cms.Infrastructure.AutoMapper
{
    public abstract class CmsMappingProfile : Profile
    {
        private const string DocumentPrefix = "Document";

        protected string[] GetCmsPrefixes(string className)
        {
            if (string.IsNullOrEmpty(className))
            {
                throw new ArgumentOutOfRangeException(nameof(className), "Value cannot be null or empty.");
            }

            string[] classNameParts = className.Split('.');
            int numParts = classNameParts.Length;

            string classPrefix = numParts == 1 ? classNameParts[0] : classNameParts[numParts - 1];

            return new[] {DocumentPrefix, classPrefix};
        }
    }
}
