using ObjectEditor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.WinForms.Controls.ValueControls
{
    /// <summary>
    /// Represents a control that is used to display a boolean value.
    /// </summary>
    internal class BooleanValueControl : CheckBox, IValueControl
    {
        public Control Control => this;

        public object Value
        {
            get => Checked;
            set => Checked = value.ChangeType<bool>();
        }

        public bool ReadOnly
        {
            get => !Enabled;
            set => Enabled = !value;
        }

        public event EventHandler<object> ValueChanged;

        public BooleanValueControl()
        {
            Text = "";
        }

        public void Initialize(Type valueType) { }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            ValueChanged?.Invoke(this, Value);
        }
    }
}
