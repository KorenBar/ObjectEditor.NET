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
        /// The value control that displays the value and used as the view control.
        /// </summary>
        private IValueControl _valueControl;

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

        protected override Control CreateViewControl(FieldMetadata fieldInfo)
        { // The base class will call this method from the constructor to create the view control.
            _valueControl = fieldInfo.CreateValueControl();
            return _valueControl.Control;
        }

        protected override void UpdateValueControl(object value)
        {
            if (value != null)
                _valueControl.Value = value;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (_valueControl == null) // This should never happen, and if it does, it's a programming error.
                throw new InvalidOperationException("The value control is not initialized.");

            base.OnLoad(e);

            // Register events here after the control is completely loaded,
            // to prevent them from being called during initialization.
            var valueControl = (IValueControl)ViewControl;
            valueControl.ValueChanged += (s, e) => OnUserChangedValue(e);
        }
    }
}
