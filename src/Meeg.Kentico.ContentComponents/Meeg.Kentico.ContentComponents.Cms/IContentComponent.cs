using CMS.DocumentEngine;

namespace Meeg.Kentico.ContentComponents.Cms
{
    /// <summary>
    /// Represents a Content Component.
    /// </summary>
    public interface IContentComponent
    {
        /// <summary>
        /// The page that this component "belongs" to i.e. is a field of.
        /// </summary>
        TreeNode Parent { get; }

        /// <summary>
        /// The class code name of the component.
        /// </summary>
        string NodeClassName { get; }
    }
}
