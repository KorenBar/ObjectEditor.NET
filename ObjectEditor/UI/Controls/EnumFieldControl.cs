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
using TechnosoUI.Configuration;

namespace TechnosoCommons.Configuration.UI.Controls
{
    internal class EnumFieldControl : BaseFieldControl
    {
        private ComboBox ComboBox => (ComboBox)ValueControl;

        public EnumFieldControl(object value, BaseFieldInfo fieldInfo) : base(value, fieldInfo) { }

        protected override Control CreateValueControl(BaseFieldInfo fieldInfo)
        {
            var comboBox = new ComboBox();
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.DataSource = fieldInfo.Type.GetEnumValues();
            comboBox.Enabled = !fieldInfo.IsReadOnly;
            return comboBox;
        }

        protected override object GetValue()
        {
            return ComboBox.SelectedItem;
        }

        protected override void SetValue(object value)
        {
            ComboBox.SelectedItem = value;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Add events here to prevent rising when showing the form.
            ComboBox.SelectedValueChanged += (s, args) => OnValueChanged(ComboBox.SelectedItem);
        }
    }
}
