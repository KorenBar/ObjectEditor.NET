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
using ObjectEditor.WinForms.Forms;
using ObjectEditor.Extensions;
using ObjectEditor.Controllers.Fields;

namespace ObjectEditor.WinForms.Controls
{
    internal class KeyValuePairFieldControl : BaseFieldControl
    {
        private BaseFieldControl _keyFieldControl;
        private BaseFieldControl _valueFieldControl;

        private KeyValuePairFieldController KeyValuePairFieldController => (KeyValuePairFieldController)Controller;

        /// <summary>
        /// Creates a new KeyValuePairFieldControl.
        /// </summary>
        /// <param name="value">The initial value of the field.</param>
        /// <param name="fieldInfo">The information of the field.</param>
        /// <param name="parentForm">The containing form of the field.</param>
        public KeyValuePairFieldControl(KeyValuePairFieldController controller, ObjectEditorForm parentForm) : base(controller, parentForm)
        {
            ShowNameLabel = false;
        }

        protected override Control CreateViewControl(FieldMetadata fieldInfo)
        { // Assume this method is called only once, so we can create the controls here.
            // create a layout panel to hold the key and value controls
            TableLayoutPanel panel = new()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            _keyFieldControl = KeyValuePairFieldController.KeyFieldController.CreateFieldControl(ParentEditorForm);
            _valueFieldControl = KeyValuePairFieldController.ValueFieldController.CreateFieldControl(ParentEditorForm);

            panel.Controls.Add(_keyFieldControl, 0, 0);
            panel.Controls.Add(_valueFieldControl, 1, 0);

            return panel;
        }

        protected override void UpdateValueControl(object value)
        {
            // already handled by the key and value field controls themselves (they have their own controllers)
        }
    }
}
