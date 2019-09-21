using CMS.DocumentEngine;
using CMS.FormEngine;
using CMS.FormEngine.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Meeg.Kentico.ContentComponents.Cms.CMSFormControls
{
    public partial class ContentComponent : FormEngineUserControl
    {
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

        public override object Value
        {
            get
            {
                // Get the component fields and use the field data to create a TreeNode representing the component

                ContentComponentFieldCollection componentFields = GetContentComponentFormFields();
                ContentComponentNode = GetContentComponentNode(componentFields);

                // Use the field data to set any system fields on the page this component belongs to

                SetPageSystemFields(componentFields);

                // Return the component node serialized to XML

                var serializer = new ContentComponentSerializer();
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

                var deserializer = new ContentComponentDeserializer();
                ContentComponentNode = deserializer.Deserialize(PageType, componentXml);
            }
        }

        private TreeNode ContentComponentNode
        {
            get;
            set;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LoadContentComponentForm();
        }

        private void LoadContentComponentForm()
        {
            // Set some basic form properties

            ContentComponentForm.SubmitButton.Visible = false;
            ContentComponentForm.MessagesPlaceHolder = MessagesPlaceHolder;

            // TODO: Resource string  
            ValidationError = "Please correct the above validation errors.";

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
        }

        private ContentComponentFieldCollection GetContentComponentFormFields()
        {
            var componentFieldCollection = new ContentComponentFieldCollection();

            ContentComponentForm.Fields.ForEach(fieldName =>
            {
                FormEngineUserControl field = ContentComponentForm.FieldControls[fieldName];

                object fieldValue = field.Value;
                bool fieldIsSystem = field.FieldInfo.System;

                componentFieldCollection.AddField(fieldName, fieldValue, fieldIsSystem);
            });

            return componentFieldCollection;
        }

        private TreeNode GetContentComponentNode(ContentComponentFieldCollection componentFields)
        {
            var componentNode = TreeNode.New(PageType);

            componentFields.ApplyFieldsTo(componentNode);

            return componentNode;
        }

        private void SetPageSystemFields(ContentComponentFieldCollection componentFields)
        {
            var page = Form.EditedObject as TreeNode;

            componentFields.ApplyFieldsTo(page, field => field.IsSystem);
        }

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