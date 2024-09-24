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
    internal partial class EnumFieldControl : BaseFieldControl
    {
        public EnumFieldControl(object value, BaseFieldInfo fieldInfo) : base(value, fieldInfo) { }

        protected override void Initialize()
        {
            InitializeComponent();
            comboBox.DataSource = FieldInfo.Type.GetEnumValues();
            comboBox.Enabled = !FieldInfo.IsReadOnly;
            SetControl(comboBox);
        }

        protected override object GetValue()
        {
            return comboBox.SelectedItem;
        }

        protected override void SetValue(object value)
        {
            comboBox.SelectedItem = value;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Add events here to prevent rising when showing the form.
            comboBox.SelectedValueChanged += (s, args) => OnValueChanged(comboBox.SelectedItem);
        }
    }
}
