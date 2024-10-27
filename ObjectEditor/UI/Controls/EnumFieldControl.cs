﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using ObjectEditor.UI.Forms;

namespace ObjectEditor.UI.Controls
{
    internal class EnumFieldControl : BaseFieldControl
    {
        private ComboBox ComboBox => (ComboBox)ValueControl;

        public EnumFieldControl(object value, BaseFieldInfo fieldInfo, ObjectEditorForm parentForm) : base(value, fieldInfo, parentForm) { }

        protected override Control CreateValueControl(BaseFieldInfo fieldInfo)
        {
            var comboBox = new ComboBox();
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.DataSource = fieldInfo.Type.GetEnumValues();
            comboBox.Enabled = !fieldInfo.IsReadOnly;
            return comboBox;
        }

        protected override void UpdateControlValue(object value)
        {
            ComboBox.SelectedItem = value;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Add events here to prevent rising when showing the form.
            ComboBox.SelectedValueChanged += (s, args) => OnUserChangedValue(ComboBox.SelectedItem);
        }
    }
}
