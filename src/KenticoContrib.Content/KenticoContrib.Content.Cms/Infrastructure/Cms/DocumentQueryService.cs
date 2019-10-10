using CMS.DocumentEngine;

namespace KenticoContrib.Content.Cms.Infrastructure.Cms
{
    public class DocumentQueryService
    {
        public DocumentQuery<TDocument> GetQuery<TDocument>()
            where TDocument : TreeNode, new()
        {
            // TODO: Current site, current culture, preview mode

            return DocumentHelper.GetDocuments<TDocument>()
                .Culture("en-gb"); // TODO: Don't hardcode this
        }
    }
}
