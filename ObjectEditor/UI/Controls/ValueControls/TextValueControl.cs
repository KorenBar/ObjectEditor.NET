using ObjectEditor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.UI.Controls.ValueControls
{
    /// <summary>
    /// Represents a control that is used to display a string value.
    /// </summary>
    internal class TextValueControl : TextBox, IValueControl
    {
        private bool _changingValue;

        public Control Control => this;

        public object Value
        {
            get => Text;
            set => Text = value?.ToString();
        }

        // The TextBox.ReadOnly already does what we need.
        //bool IValueControl.ReadOnly
        //{
        //    get => ReadOnly;
        //    set => ReadOnly = value;
        //}

        public event EventHandler<object> ValueChanged;

        public TextValueControl() { }

        public void Initialize(Type valueType) { }

        protected override void OnTextChanged(EventArgs e)
        { // assuming that the TextChanged event is raised only when the text is changed by the user, not programmatically.
            base.OnTextChanged(e);
            _changingValue = true;
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            var changed = _changingValue;
            _changingValue = false; // reset the flag first
            if (changed)
                ValueChanged?.Invoke(this, Value);
        }
    }
}
