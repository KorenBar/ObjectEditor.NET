using ObjectEditor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.WinForms.Controls.ValueControls
{
    /// <summary>
    /// Represents a control that is used to display an enumeration value.
    /// </summary>
    internal class EnumValueControl : ComboBox, IValueControl
    {
        public Control Control => this;

        public object Value
        {
            get => SelectedItem;
            set => SelectedItem = value;
        }

        public bool ReadOnly
        {
            get => !Enabled;
            set => Enabled = !value;
        }

        public event EventHandler<object> ValueChanged;

        public EnumValueControl() { }

        public void Initialize(Type valueType)
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            DataSource = valueType.GetEnumValues();
        }

        protected override void OnSelectedValueChanged(EventArgs e)
        {
            base.OnSelectedValueChanged(e);
            ValueChanged?.Invoke(this, Value);
        }
    }
}
