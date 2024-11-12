using ObjectEditor.Controllers.Fields;
using ObjectEditor.UI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.UI.Controls
{
    internal class ValueFieldControl : BaseFieldControl
    {
        /// <summary>
        /// Creates a field control for a value type.
        /// </summary>
        /// <param name="controller">The controller of the field.</param>
        /// <param name="parentForm">The parent form that contains this field.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ValueFieldControl(ValueFieldController controller, ObjectEditorForm parentForm)
            : base(controller, parentForm)
        {
        }

        protected override Control CreateValueControl(FieldMetadata fieldInfo)
        {
            var valueControl = fieldInfo.CreateValueControl();
            valueControl.ValueChanged += (s, e) => OnUserChangedValue(e);
            return valueControl.Control;
        }

        protected override void UpdateValueControl(object value)
        {
            if (value != null)
                ((IValueControl)ValueControl).Value = value;
        }
    }
}
