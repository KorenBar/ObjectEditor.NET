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

namespace ObjectEditor.UI.Controls
{
    internal class BooleanFieldControl : BaseFieldControl
    {
        private CheckBox CheckBox => (CheckBox)ValueControl;

        public BooleanFieldControl(object value, BaseFieldInfo fieldInfo) : base(value, fieldInfo) { }

        protected override Control CreateValueControl(BaseFieldInfo fieldInfo)
        {
            var checkBox = new CheckBox();
            checkBox.Dock = DockStyle.Fill;
            checkBox.Text = "";
            checkBox.Enabled = !fieldInfo.IsReadOnly;
            return checkBox;
        }

        protected override void UpdateControlValue(object value)
        {
            CheckBox.Checked = Convert.ToBoolean(value);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Add events here to prevent rising when showing the form.
            CheckBox.CheckedChanged += (s, args) => OnUserChangedValue(CheckBox.Checked);
        }
    }
}
