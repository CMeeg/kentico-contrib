using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.FormEngine;
using CMS.Relationships;

namespace Meeg.Kentico.ContentComponents.Cms
{
    public static class ContentComponentExtensions
    {
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
