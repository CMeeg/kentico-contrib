using System.Data;
using CMS.DocumentEngine;
using CMS.Helpers;

namespace Meeg.Kentico.ContentComponents.Cms
{
    /// <summary>
    /// This class can be used to deserialise Content Component data that has been serialised to XML.
    /// </summary>
    public class ContentComponentDeserializer
    {
        /// <summary>
        /// Deserialises Content Component XML and returns a new TreeNode instance containing component data.
        /// </summary>
        /// <param name="pageType">The full class name of the component Page Type.</param>
        /// <param name="componentXml">The Content Component XML to deserialise.</param>
        /// <returns>A new TreeNode instance containing the deserialised component data.</returns>
        public TreeNode Deserialize(string pageType, string componentXml)
        {
            DataRow componentData = GetComponentData(componentXml);

            if (componentData == null)
            {
                return null;
            }

            return TreeNode.New(pageType, componentData);
        }

        /// <summary>
        /// Deserialises Content Component XML and returns a new TreeNode instance of type T containing component data.
        /// </summary>
        /// <typeparam name="T">A type representing the Page Type of the component.</typeparam>
        /// <param name="componentXml">The Content Component XML to deserialise.</param>
        /// <returns>A new TreeNode instance of Type T containing the deserialised component data.</returns>
        public T Deserialize<T>(string componentXml)
            where T : TreeNode, new()
        {
            DataRow componentData = GetComponentData(componentXml);

            if (componentData == null)
            {
                return null;
            }

            return TreeNode.New<T>(componentData);
        }

        private DataRow GetComponentData(string componentXml)
        {
            if (string.IsNullOrEmpty(componentXml))
            {
                return null;
            }

            DataSet componentData = DataHelper.GetDataSetFromXml(componentXml);

            if (DataHelper.DataSourceIsEmpty(componentData))
            {
                return null;
            }

            return componentData.Tables[0].Rows[0];
        }
    }
}
