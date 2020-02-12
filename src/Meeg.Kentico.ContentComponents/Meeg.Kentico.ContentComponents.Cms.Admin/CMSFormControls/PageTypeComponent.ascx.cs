using CMS.DocumentEngine;
using CMS.FormEngine;
using CMS.FormEngine.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Relationships;

namespace Meeg.Kentico.ContentComponents.Cms.Admin.CMSFormControls
{
    /// <summary>
    /// Form Control used to edit Page Type Component data in the CMS.
    /// </summary>
    public partial class PageTypeComponent : FormEngineUserControl
    {
        private string deserializationError;

        private string PageType
        {
            get => GetValue("PageType", string.Empty);
            set => SetValue("PageType", value);
        }

        private string AlternativeFormName
        {
            get => GetValue("AlternativeFormName", string.Empty);
            set => SetValue("AlternativeFormName", value);
        }

        private string PageTypeFormName
        {
            get
            {
                if (string.IsNullOrEmpty(AlternativeFormName))
                {
                    return PageType;
                }

                return $"{PageType}.{AlternativeFormName}";
            }
        }

        /// <summary>
        /// Gets and sets the component field value. When getting the value, the values of the component Page Type's fields are serialised to XML. When setting the value the XML is deserialised and used to populate the fields of the component Page Type.
        /// </summary>
        public override object Value
        {
            get
            {
                // Get the component fields and use the field data to create a TreeNode representing the component

                ContentComponentFieldCollection componentFields = GetContentComponentFormFields();
                ContentComponentNode = GetContentComponentNode(componentFields);

                // Use the field data to set any system fields on the page this component belongs to

                SetPageSystemFields(componentFields);

                // Set the parent of the component to the node that is currently being edited

                if (ParentNode.NodeID > 0)
                {
                    ContentComponentNode.NodeParentID = ParentNode.NodeID;
                }

                // Return the component node serialized to XML

                var serializer = new PageTypeComponentSerializer();
                return serializer.Serialize(ContentComponentNode);
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                string componentXml = value.ToString();

                if (string.IsNullOrEmpty(componentXml))
                {
                    return;
                }

                // Deserialize the component XML to a TreeNode that represents the component

                var deserializer = new PageTypeComponentDeserializer();

                try
                {
                    ContentComponentNode = deserializer.Deserialize(PageType, componentXml);
                }
                catch (Exception e)
                {
                    // N.B. ShowError does not result in the error being rendered if called at this point

                    deserializationError = e.Message;
                }
            }
        }

        private TreeNode ContentComponentNode
        {
            get;
            set;
        }

        private TreeNode parentNode;
        private TreeNode ParentNode => parentNode ?? (parentNode = GetParentNode(Form));

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LoadContentComponentForm();
        }

        protected override void OnLoad(EventArgs e)
        {
            ShowErrorMessages();
        }

        private void ShowErrorMessages()
        {
            // Show the deserialisation error if there was one

            if (!string.IsNullOrEmpty(deserializationError))
            {
                string errorMessage = ResHelper.GetString("Meeg.Kentico.ContentComponents.FormControls.PageTypeComponent.DeserializationError");

                ShowError($"{Field}: {errorMessage}", deserializationError);
            }
        }

        private void LoadContentComponentForm()
        {
            // Set some basic form properties

            ContentComponentForm.SubmitButton.Visible = false;
            ContentComponentForm.MessagesPlaceHolder = MessagesPlaceHolder;

            ValidationError = ResHelper.GetString("Meeg.Kentico.ContentComponents.FormControls.PageTypeComponent.ValidationError");

            // Load and populate the form

            FormInfo form = FormHelper.GetFormInfo(PageTypeFormName, true);

            ContentComponentForm.FormInformation = form;

            if (ContentComponentNode != null)
            {
                // Populate form with existing data

                ContentComponentForm.Data = ContentComponentNode;
                ContentComponentForm.EditedObject = ContentComponentNode;
            }
            else
            {
                // Populate form with default data

                DataRow dr = form.GetDataRow();

                form.LoadDefaultValues(dr, true);

                ContentComponentForm.DataRow = dr;
            }

            // Reload the form to populate the UI controls with component data

            ContentComponentForm.ReloadData();

            // Ensure that the form fields are loaded correctly

            LoadContentComponentFormFields();
        }

        private void LoadContentComponentFormFields()
        {
            ContentComponentForm.Fields.ForEach(fieldName =>
            {
                FormEngineUserControl field = ContentComponentForm.FieldControls[fieldName];

                switch (field.FieldInfo.DataType.ToLowerInvariant())
                {
                    case "docrelationships":
                        LoadContentComponentPagesField(field);

                        return;
                    default:
                        return;
                }
            });
        }

        private void LoadContentComponentPagesField(FormEngineUserControl field)
        {
            // Set the TreeNode that the relationships are to be added to to the "Parent" node of this component

            var editedObject = ParentNode;

            if (editedObject == null)
            {
                return;
            }

            if (editedObject.NodeID == 0)
            {
                return;
            }

            // The form control is a UserControl in the CMSApp project so we have to use reflection as we don't know the type

            const string propertyName = "TreeNode";

            PropertyInfo fieldProperty = field.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            if (fieldProperty == null || !fieldProperty.CanWrite)
            {
                return;
            }

            fieldProperty.SetValue(field, editedObject);

            // Reload the control to make sure it loads correctly with the TreeNode property set

            field.ReloadControl();

            // We also need to make sure that the ad-hoc relationship name has been created

            var dataClass = DataClassInfoProvider.GetDataClassInfo(editedObject.ClassName);

            RelationshipNameInfoProvider.EnsureAdHocRelationshipNameInfo(dataClass,
                field.FieldInfo);
        }

        private ContentComponentFieldCollection GetContentComponentFormFields()
        {
            var componentFieldCollection = new ContentComponentFieldCollection();

            ContentComponentForm.Fields.ForEach(fieldName =>
            {
                FormEngineUserControl field = ContentComponentForm.FieldControls[fieldName];

                if (!FieldControlHasValue(field))
                {
                    return;
                }

                object fieldValue = field.Value;

                bool fieldIsSystem = field.FieldInfo.System;

                componentFieldCollection.AddField(fieldName, fieldValue, fieldIsSystem);
            });

            return componentFieldCollection;
        }

        private TreeNode GetParentNode(BasicForm form)
        {
            var formControl = form.Parent as FormEngineUserControl;

            if (formControl == null)
            {
                return form.EditedObject as TreeNode;
            }

            if (formControl.GetType() == GetType())
            {
                return GetParentNode(formControl.Form);
            }

            return form.EditedObject as TreeNode;
        }

        private bool FieldControlHasValue(FormEngineUserControl field)
        {
            if (field == null || !field.HasValue)
            {
                return false;
            }

            // field.HasValue can return true even if the field doesn't have a value so we need to check the actual value also

            if (field.Value == null || string.IsNullOrEmpty(field.Value.ToString()))
            {
                return false;
            }

            return true;
        }

        private TreeNode GetContentComponentNode(ContentComponentFieldCollection componentFields)
        {
            var componentNode = TreeNode.New(PageType);

            componentFields.ApplyFieldsTo(componentNode);

            return componentNode;
        }

        private void SetPageSystemFields(ContentComponentFieldCollection componentFields)
        {
            var page = ParentNode;

            componentFields.ApplyFieldsTo(page, field => field.IsSystem);
        }

        /// <summary>
        /// Validates the fields of the component Page Type.
        /// </summary>
        /// <returns>True if all fields are valid; otherwise false.</returns>
        public override bool IsValid()
        {
            return ContentComponentForm.ValidateData();
        }

        private class ContentComponentFieldCollection
        {
            private List<Field> Fields { get; }

            public ContentComponentFieldCollection()
            {
                Fields = new List<Field>();
            }

            public void AddField(string name, object value, bool isSystem)
            {
                Fields.Add(new Field(name, value, isSystem));
            }

            public void ApplyFieldsTo(TreeNode node, Func<Field, bool> fieldPredicate = null)
            {
                if (node == null)
                {
                    return;
                }

                foreach (Field field in Fields.Where(f => fieldPredicate?.Invoke(f) ?? true))
                {
                    // Prefer setting TreeNode properties over using TreeNode.SetValue in case there is any specific logic in the property setter that wouldn't be executed when using TreeNode.SetValue

                    PropertyInfo fieldProperty = node.GetType().GetProperty(field.Name, BindingFlags.Public | BindingFlags.Instance);

                    if (fieldProperty != null && fieldProperty.CanWrite)
                    {
                        // There is a corresponding property we can set

                        fieldProperty.SetValue(node, field.Value);
                    }
                    else
                    {
                        // No property, so use SetValue instead

                        node.SetValue(field.Name, field.Value);
                    }
                }
            }

            public struct Field
            {
                public string Name { get; }
                public object Value { get; }
                public bool IsSystem { get; }

                public Field(string name, object value, bool isSystem)
                {
                    Name = name;
                    Value = value;
                    IsSystem = isSystem;
                }
            }
        }
    }
}
