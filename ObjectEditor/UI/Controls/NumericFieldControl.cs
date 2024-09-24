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
    internal partial class NumericFieldControl : BaseFieldControl
    {
        public NumericFieldControl(object value, BaseFieldInfo fieldInfo) : base(value, fieldInfo) { }

        protected override void Initialize()
        {
            InitializeComponent();

            if (FieldInfo.Type.IsInteger())
            {
                numericBox.Increment = 1M;
                numericBox.DecimalPlaces = 0;
            }
            else
            {
                numericBox.Increment = 0.5M;
                numericBox.DecimalPlaces = 2;
            }

            numericBox.Maximum = FieldInfo.Type.MaxValue();
            numericBox.Minimum = FieldInfo.Type.MinValue();
            numericBox.Enabled = !FieldInfo.IsReadOnly;
            
            SetControl(numericBox);
        }

        protected override object GetValue()
        {
            return Convert.ChangeType(numericBox.Value, FieldInfo.Type);
        }

        protected override void SetValue(object value)
        {
            numericBox.Value = Convert.ToDecimal(value ?? 0m);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Add events here after the form is ready, to prevent rising when showing the form.
            numericBox.ValueChanged += (s, args) => OnValueChanged(this.Value);
        }
    }
}
