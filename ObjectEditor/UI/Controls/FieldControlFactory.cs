using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ObjectEditor.WinForms.Forms;
using ObjectEditor.Extensions;
using ObjectEditor.Data;
using ObjectEditor.WinForms.Controls.ValueControls;
using ObjectEditor.Controllers.Fields;
using ObjectEditor.Controllers.Editors;

namespace ObjectEditor.WinForms.Controls
{
    internal static class FieldControlFactory
    {
        /// <summary>
        /// Creates a field control for a given field controller.
        /// </summary>
        /// <param name="controller">The controller of the field.</param>
        /// <param name="parentForm">The containing form of the field.</param>
        /// <returns>The created field control.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static BaseFieldControl CreateFieldControl(this ValueFieldController controller, ObjectEditorForm parentForm)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return controller switch
            {
                ObjectFieldController c => new ObjectFieldControl(c, parentForm),
                KeyValuePairFieldController c => new KeyValuePairFieldControl(c, parentForm),
                _ => new ValueFieldControl(controller, parentForm)
            };
        }

        /// <summary>
        /// Creates a value control for a given field.
        /// </summary>
        /// <param name="fieldInfo">The field information.</param>
        /// <returns>The created value control.</returns>
        /// <exception cref="NotSupportedException">if the field type is a class type.</exception>
        internal static IValueControl CreateValueControl(this FieldMetadata fieldInfo)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException(nameof(fieldInfo));

            IValueControl valueControl;

            if (fieldInfo.Type.IsEnum) // enum field
                valueControl = new EnumValueControl();
            else if (fieldInfo.Type.IsNumeric()) // numeric field
                valueControl = new NumericValueControl();
            else if (fieldInfo.Type == typeof(bool)) // boolean field
                valueControl = new BooleanValueControl();
            else if (fieldInfo.Type.IsSimpleType()) // default field for any other simple type
                valueControl = new TextValueControl()
                {
                    UseSystemPasswordChar = fieldInfo is PropertyFieldMetadata propertyFieldInfo && propertyFieldInfo.IsPassword
                };
            else // it's a class type (reference)
                throw new NotSupportedException("Cannot create value control for a class type.");

            if (valueControl != null)
            { // initialize the value control
                valueControl.Initialize(fieldInfo.Type);
                valueControl.ReadOnly = fieldInfo.IsReadOnly;
            }

            return valueControl;
        }
    }
}
