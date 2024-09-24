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
    internal partial class TextFieldControl : BaseFieldControl
    {
        /// <summary>
        /// Creates a new text field control.
        /// </summary>
        /// <param name="value">Initial value.</param>
        /// <param name="fieldInfo">Field information.</param>
        public TextFieldControl(object value, BaseFieldInfo fieldInfo) : base(value, fieldInfo) { }

        protected override void Initialize()
        {
            InitializeComponent();
            textBox.ReadOnly = FieldInfo.IsReadOnly;
            SetControl(textBox);

            if (FieldInfo is PropertyFieldInfo propertyFieldInfo)
            {
                var infoAttr = propertyFieldInfo.PropertyInfo.GetCustomAttribute<InfoAttribute>();
                textBox.UseSystemPasswordChar = infoAttr?.IsPassword ?? false;
            }
        }

        protected override object GetValue()
        {
            return textBox.Text;
        }

        protected override void SetValue(object value)
        {
            textBox.Text = value?.ToString();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Registering the events here to prevent rising when showing the form.
            textBox.TextChanged += (s, args) => OnValueChanged(textBox.Text);
        }
    }
}
