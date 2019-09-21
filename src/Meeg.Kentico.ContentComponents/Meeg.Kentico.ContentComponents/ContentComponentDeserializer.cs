using CMS.DocumentEngine;
using CMS.Helpers;
using System.Data;

namespace Meeg.Kentico.ContentComponents
{
    public class ContentComponentDeserializer
    {
        public TreeNode Deserialize(string pageType, string componentXml)
        {
            DataRow componentData = GetComponentData(componentXml);

            if (componentData == null)
            {
                return null;
            }

            return TreeNode.New(pageType, componentData);
        }

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
