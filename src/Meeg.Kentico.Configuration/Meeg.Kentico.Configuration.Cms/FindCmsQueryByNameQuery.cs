using System;
using System.Linq;

namespace Meeg.Kentico.Configuration.Cms
{
    internal class FindCmsQueryByNameQuery : IQuery<CmsQuery>
    {
        private const string CodeNameDelimiter = ".";

        public string ClassName { get; }
        public string QueryName { get; }

        public FindCmsQueryByNameQuery(string queryName)
        {
            if (string.IsNullOrEmpty(queryName) || !queryName.Contains(CodeNameDelimiter))
            {
                throw new ArgumentException("Please supply a valid query name in `ClassCodeName.QueryName` format.", nameof(queryName));
            }

            string[] queryNameParts = queryName.Split(CodeNameDelimiter.ToCharArray());

            ClassName = string.Join(
                CodeNameDelimiter,
                queryNameParts.Take(queryNameParts.Length - 1)
            );

            QueryName = queryNameParts.Last();
        }
    }
}
