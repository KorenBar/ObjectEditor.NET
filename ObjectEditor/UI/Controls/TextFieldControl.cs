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
    internal class TextFieldControl : BaseFieldControl
    {
        private bool _changingValue;

        private TextBox TextBox => (TextBox)ValueControl;

        /// <summary>
        /// Creates a new text field control.
        /// </summary>
        /// <param name="value">Initial value.</param>
        /// <param name="fieldInfo">Field information.</param>
        public TextFieldControl(object value, BaseFieldInfo fieldInfo) : base(value, fieldInfo) { }

        protected override Control CreateValueControl(BaseFieldInfo fieldInfo)
        {
            var textBox = new TextBox();
            textBox.ReadOnly = fieldInfo.IsReadOnly;

            if (fieldInfo is PropertyFieldInfo propertyFieldInfo)
            {
                var infoAttr = propertyFieldInfo.PropertyInfo.GetCustomAttribute<InfoAttribute>();
                textBox.UseSystemPasswordChar = infoAttr?.IsPassword ?? false;
            }

            return textBox;
        }

        protected override void UpdateControlValue(object value)
        {
            TextBox.Text = value?.ToString();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Registering the events here to prevent rising when showing the form.
            TextBox.TextChanged += (s, args) => _changingValue = true;
            TextBox.Leave += (s, args) =>
            {
                var changed = _changingValue;
                _changingValue = false; // reset the flag first
                if (changed)
                    OnUserChangedValue(TextBox.Text);
            };
        }
    }
}
