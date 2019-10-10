using CMS.DocumentEngine;

namespace KenticoContrib.Content.Cms
{
    public static class ColumnDefinitions
    {
        public static readonly string[] IPageColumns = {
            nameof(TreeNode.DocumentID),
            nameof(TreeNode.DocumentName),
            nameof(TreeNode.NodeAlias)
        };
    }
}
