using System;
using CMS.DocumentEngine;

namespace Meeg.Kentico.ContentComponents
{
    public static class TreeNodeExtensions
    {
        public static TreeNode GetContentComponent(this TreeNode node, string pageType, string columnName)
        {
            string componentXml = node.GetValue(columnName, string.Empty);

            var deserializer = new ContentComponentDeserializer();

            return deserializer.Deserialize(pageType, componentXml);
        }

        public static T GetContentComponent<T>(this TreeNode node, string columnName)
            where T : TreeNode, new()
        {
            string componentXml = node.GetValue(columnName, string.Empty);

            var deserializer = new ContentComponentDeserializer();

            return deserializer.Deserialize<T>(componentXml);
        }

        public static TComponent GetContentComponent<TPage, TComponent>(this TPage page, Func<TPage, string> componentField)
            where TPage : TreeNode, new()
            where TComponent : TreeNode, new()
        {
            string componentXml = componentField.Invoke(page);

            var deserializer = new ContentComponentDeserializer();

            return deserializer.Deserialize<TComponent>(componentXml);
        }
    }
}
