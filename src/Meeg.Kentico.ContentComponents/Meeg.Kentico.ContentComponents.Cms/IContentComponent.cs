using CMS.DocumentEngine;

namespace Meeg.Kentico.ContentComponents.Cms
{
    public interface IContentComponent
    {
        TreeNode Parent { get; }
        string NodeClassName { get; }
    }
}
