using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using CMS.FormEngine;
using CMS.Relationships;

namespace Meeg.Kentico.ContentComponents.Cms
{
    /// <summary>
    /// IContentComponent extension methods to use with Content Components.
    /// </summary>
    public static class ContentComponentExtensions
    {
        /// <summary>
        /// Get related pages for a Pages field specified on a component.
        /// </summary>
        /// <param name="component">The component that has the Pages field.</param>
        /// <param name="fieldName">The code name of the Pages field on the component.</param>
        /// <returns>The related pages for the specified component field.</returns>
        public static IEnumerable<TreeNode> GetRelatedDocumentsForComponent(this IContentComponent component, string fieldName)
        {
            TreeNode parent = component.Parent;

            if (parent == null)
            {
                return Enumerable.Empty<TreeNode>();
            }

            FormInfo form = FormHelper.GetFormInfo(component.NodeClassName, false);
            FormFieldInfo formField = form.GetFormField(fieldName);

            if (formField == null)
            {
                return Enumerable.Empty<TreeNode>();
            }

            string relationshipNameCodeName = RelationshipNameInfoProvider.GetAdHocRelationshipNameCodeName(parent.NodeClassName, formField);

            RelationshipNameInfo relationshipNameInfo = RelationshipNameInfoProvider.GetRelationshipNameInfo(relationshipNameCodeName);

            var relationshipQuery = DocumentHelper.GetDocuments()
                .Culture(parent.DocumentCulture)
                .CombineWithDefaultCulture(parent.TreeProvider.GetCombineWithDefaultCulture(parent.Site.SiteName))
                .Published(!parent.IsLastVersion)
                .PublishedVersion(!parent.IsLastVersion)
                .WithCoupledColumns()
                .InRelationWith(
                    parent.NodeGUID,
                    relationshipNameCodeName,
                    RelationshipSideEnum.Left
                );

            return RelationshipInfoProvider.ApplyRelationshipOrderData(
                relationshipQuery,
                parent.NodeID,
                relationshipNameInfo.RelationshipNameId
            );
        }
    }
}
