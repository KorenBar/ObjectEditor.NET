using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;
using TechnosoCommons.Extensions;
using TechnosoUI.Configuration;

namespace TechnosoCommons.Configuration.UI.Controls
{
    internal class NumericFieldControl : BaseFieldControl
    {
        private NumericUpDown NumericBox => (NumericUpDown)ValueControl;

        public NumericFieldControl(object value, BaseFieldInfo fieldInfo) : base(value, fieldInfo) { }

        protected override Control CreateValueControl(BaseFieldInfo fieldInfo)
        {
            var numericBox = new TechnosoCommons.UI.Controls.NumericBox();

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

        protected override object GetValue()
        {
            return Convert.ChangeType(NumericBox.Value, FieldInfo.Type);
        }

        protected override void SetValue(object value)
        {
            NumericBox.Value = Convert.ToDecimal(value ?? 0m);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Add events here after the form is ready, to prevent rising when showing the form.
            NumericBox.ValueChanged += (s, args) => OnValueChanged(this.Value);
        }
    }
}
