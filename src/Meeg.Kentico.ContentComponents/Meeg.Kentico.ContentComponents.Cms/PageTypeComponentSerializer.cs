using System.Xml;
using CMS.DataEngine.Serialization;
using CMS.DocumentEngine;

namespace Meeg.Kentico.ContentComponents.Cms
{
    /// <summary>
    /// This class can be used to serialise Page Type Component data to XML.
    /// </summary>
    public class PageTypeComponentSerializer
    {
        /// <summary>
        /// Serialises a Page Type Component to XML.
        /// </summary>
        /// <param name="component">The Page Type Component to serialise - can be an instance of any type derived from TreeNode.</param>
        /// <returns>The component serialised to XML; or an empty string if the component is null.</returns>
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
