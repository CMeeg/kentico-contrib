using System;
using CMS.DocumentEngine;

namespace Meeg.Kentico.ContentComponents.Cms
{
    /// <summary>
    /// TreeNode extension methods provided for convenience when using Content Components.
    /// </summary>
    public static class TreeNodeExtensions
    {
        /// <summary>
        /// Gets the value from the node using the provided columnName and deserialises it to a new TreeNode instance containing component data.
        /// </summary>
        /// <param name="node">The node that contains the component data.</param>
        /// <param name="pageType">The full class name of the component Page Type.</param>
        /// <param name="columnName">The name of the column that stores component data as XML.</param>
        /// <returns>A new TreeNode instance containing the deserialised component data.</returns>
        public static TreeNode GetPageTypeComponent(this TreeNode node, string pageType, string columnName)
        {
            string componentXml = node.GetValue(columnName, string.Empty);

            var deserializer = new PageTypeComponentDeserializer();

            var component = deserializer.Deserialize(pageType, componentXml);

            if (component == null)
            {
                return null;
            }

            // Set the parent of the component to the node that the component "belongs" to

            component.NodeParentID = node.NodeID;

            return component;
        }

        /// <summary>
        /// Gets the value from the node using the provided columnName and deserialises it to a new TreeNode instance of type T containing component data.
        /// </summary>
        /// <typeparam name="T">A type representing the Page Type of the component.</typeparam>
        /// <param name="node">The node that contains the component data.</param>
        /// <param name="columnName">The name of the column that stores component data as XML.</param>
        /// <returns>A new TreeNode instance of Type T containing the deserialised component data.</returns>
        public static T GetPageTypeComponent<T>(this TreeNode node, string columnName)
            where T : TreeNode, new()
        {
            string componentXml = node.GetValue(columnName, string.Empty);

            var deserializer = new PageTypeComponentDeserializer();

            var component = deserializer.Deserialize<T>(componentXml);

            if (component == null)
            {
                return null;
            }

            // Set the parent of the component to the node that the component "belongs" to

            component.NodeParentID = node.NodeID;

            return component;
        }

        /// <summary>
        /// Gets the value from the node using the provided function delegate and deserialises it to a new TreeNode instance of type TComponent containing component data.
        /// </summary>
        /// <typeparam name="TPage">A type representing the Page Type of the page that uses the component.</typeparam>
        /// <typeparam name="TComponent">A type representing the Page Type of the component.</typeparam>
        /// <param name="page">The TreeNode of type TPage that contains the component data.</param>
        /// <param name="componentField">A function delegate that returns component XML from the page.</param>
        /// <returns>A new TreeNode instance of Type TComponent containing the deserialised component data.</returns>
        public static TComponent GetPageTypeComponent<TPage, TComponent>(this TPage page, Func<TPage, string> componentField)
            where TPage : TreeNode, new()
            where TComponent : TreeNode, new()
        {
            string componentXml = componentField.Invoke(page);

            var deserializer = new PageTypeComponentDeserializer();

            var component = deserializer.Deserialize<TComponent>(componentXml);

            if (component == null)
            {
                return null;
            }

            // Set the parent of the component to the node that the component "belongs" to

            component.NodeParentID = page.NodeID;

            return component;
        }
    }
}
