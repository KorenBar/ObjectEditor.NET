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
    internal partial class BooleanFieldControl : BaseFieldControl
    {
        public BooleanFieldControl(object value, BaseFieldInfo fieldInfo) : base(value, fieldInfo) { }

        protected override void Initialize()
        {
            InitializeComponent();
            checkBox.Enabled = !FieldInfo.IsReadOnly;
            SetControl(checkBox);
        }

        protected override object GetValue()
        {
            return checkBox.Checked;
        }

        protected override void SetValue(object value)
        {
            checkBox.Checked = Convert.ToBoolean(value);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Add events here to prevent rising when showing the form.
            checkBox.CheckedChanged += (s, args) => OnValueChanged(checkBox.Checked);
        }
    }
}
