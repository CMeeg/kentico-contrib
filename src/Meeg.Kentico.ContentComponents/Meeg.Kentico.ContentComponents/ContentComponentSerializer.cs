using CMS.DataEngine.Serialization;
using CMS.DocumentEngine;
using System.Xml;

namespace Meeg.Kentico.ContentComponents
{
    public class ContentComponentSerializer
    {
        public string Serialize(TreeNode component)
        {
            if (component == null)
            {
                return string.Empty;
            }

            XmlElement xml = component.Serialize();

            return xml.OuterXml;
        }
    }
}
