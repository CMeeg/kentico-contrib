using CMS.DocumentEngine;

namespace Meeg.Kentico.ContentComponents.Cms
{
    /// <summary>
    /// Creates new instances of page type content components.
    /// </summary>
    public class PageTypeContentComponentFactory
    {
        internal const string IsContentComponentFieldName = "IsContentComponent";

        /// <summary>
        /// Creates a new instance of a content component TreeNode given the class name of the desired page type.
        /// </summary>
        /// <param name="className">The class name of the page type content component being created.</param>
        /// <returns>A new instance of a TreeNode representing the content component.</returns>
        public TreeNode Create(string className)
        {
            var componentNode = TreeNode.New(className);

            SetDefaultComponentData(componentNode);

            return componentNode;
        }

        /// <summary>
        /// Creates a new instance of a content component TreeNode of type T.
        /// </summary>
        /// <typeparam name="T">A type that derives from TreeNode and represents the page type content component being created.</typeparam>
        /// <returns>A new instance of a TreeNode of type T representing the content component.</returns>
        public T Create<T>()
            where T : TreeNode, new()
        {
            var componentNode = TreeNode.New<T>();

            SetDefaultComponentData(componentNode);

            return componentNode;
        }

        private void SetDefaultComponentData(TreeNode componentNode)
        {
            // Add a custom data field to flag that this node is a component

            componentNode.NodeCustomData.SetValue(IsContentComponentFieldName, true);
        }
    }
}
