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
using ObjectEditor.UI.Forms;
using ObjectEditor.Extensions;

namespace ObjectEditor.UI.Controls
{
    internal class KeyValuePairFieldControl : BaseFieldControl
    {
        private BaseFieldControl _keyFieldControl;
        private BaseFieldControl _valueFieldControl;

        private Type _keyType;
        private Type _valueType;

        private KeyValuePair<object, object>? KeyValuePair => Value?.CastKeyValuePair<object, object>();

        public KeyValuePairFieldControl(object value, BaseFieldInfo fieldInfo, ObjectEditorForm parentForm) : base(value, fieldInfo, parentForm)
        {
            ShowNameLabel = false;
        }

        protected override Control CreateValueControl(BaseFieldInfo fieldInfo)
        { // Assume this method is called only once, so we can create the controls here.
            // Initialize the key and value types
            var genericType = fieldInfo.Type.GetGenericType(typeof(KeyValuePair<,>));
            if (genericType == null)
                throw new ArgumentException("The field type is not a KeyValuePair.");

            var genericArguments = genericType.GetGenericArguments();
            _keyType = genericArguments[0];
            _valueType = genericArguments[1];


            // create a layout panel to hold the key and value controls
            TableLayoutPanel panel = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            _keyFieldControl = new SubFieldInfo(_keyType, "Key", fieldInfo).CreateFieldControl(Value, ParentEditorForm);
            _valueFieldControl = new SubFieldInfo(_valueType, "Value", fieldInfo).CreateFieldControl(Value, ParentEditorForm);

            _keyFieldControl.ValueChanged += (s, e) =>
            {
                if (KeyValuePair.HasValue)
                {
                    var kvp = KeyValuePair.Value;
                    kvp = new KeyValuePair<object, object>(_keyFieldControl.Value, kvp.Value);
                    SetValue(kvp, e.ByUser);
                }
            };
            _valueFieldControl.ValueChanged += (s, e) =>
            {
                if (KeyValuePair.HasValue)
                {
                    var kvp = KeyValuePair.Value;
                    kvp = new KeyValuePair<object, object>(kvp.Key, _valueFieldControl.Value);
                    SetValue(kvp, e.ByUser);
                }
            };

            panel.Controls.Add(_keyFieldControl, 0, 0);
            panel.Controls.Add(_valueFieldControl, 1, 0);

            return panel;
        }

        protected override void UpdateControlValue(object value)
        {
            var kvp = value.CastKeyValuePair<object, object>(); // will throw if not a KeyValuePair
            _keyFieldControl.Value = kvp.Key;
            _valueFieldControl.Value = kvp.Value;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Add events here to prevent rising when showing the form.
            //CheckBox.CheckedChanged += (s, args) => OnUserChangedValue(CheckBox.Checked);
        }

        // TODO: Implement Apply, Reset, ... methods
    }
}
