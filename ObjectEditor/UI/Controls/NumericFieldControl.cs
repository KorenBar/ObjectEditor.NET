using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using ObjectEditor.Extensions;
using ObjectEditor.UI.Forms;

namespace ObjectEditor.UI.Controls
{
    internal class NumericFieldControl : BaseFieldControl
    {
        private NumericUpDown NumericBox => (NumericUpDown)ValueControl;

        public NumericFieldControl(object value, BaseFieldInfo fieldInfo, ObjectEditorForm parentForm) : base(value, fieldInfo, parentForm) { }

        protected override Control CreateValueControl(BaseFieldInfo fieldInfo)
        {
            var numericBox = new ObjectEditor.UI.Controls.NumericBox();

            if (fieldInfo.Type.IsInteger())
            {
                numericBox.Increment = 1M;
                numericBox.DecimalPlaces = 0;
            }
            else
            {
                numericBox.Increment = 0.5M;
                numericBox.DecimalPlaces = 2;
            }

            numericBox.Maximum = fieldInfo.Type.MaxValue();
            numericBox.Minimum = fieldInfo.Type.MinValue();
            numericBox.Enabled = !fieldInfo.IsReadOnly;
            
            return numericBox;
        }

        protected override void UpdateControlValue(object value)
        {
            NumericBox.Value = Convert.ToDecimal(value ?? 0m);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Add events here after the form is ready, to prevent rising when showing the form.
            NumericBox.ValueChanged += (s, args) => OnUserChangedValue(NumericBox.Value);
        }
    }
}
