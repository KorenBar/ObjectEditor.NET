using ObjectEditor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.WinForms.Controls.ValueControls
{
    /// <summary>
    /// Represents a control that is used to display a numeric value.
    /// </summary>
    internal class NumericValueControl : NumericBox, IValueControl
    {
        public Control Control => this;

        object IValueControl.Value
        {
            get => Value;
            set => Value = value.ChangeType<decimal>(); // 0m will be returned if the value is null.
        }

        bool IValueControl.ReadOnly
        {
            get => !Enabled;
            set => Enabled = !value;
        }

        // The NumericBox control already has a ValueChanged event, so we need to explicitly implement the IValueControl.ValueChanged event.
        // CS0071: An explicit interface implementation of an event must use the event accessor syntax.
        private EventHandler<object> valueChanged;
        event EventHandler<object> IValueControl.ValueChanged
        {
            add => valueChanged += value;
            remove => valueChanged -= value;
        }

        public NumericValueControl() { }

        public void Initialize(Type valueType)
        {
            var isInteger = valueType.IsInteger();
            Increment = isInteger ? 1M : 0.5M;
            DecimalPlaces = isInteger ? 0 : 2;
            Maximum = valueType.MaxValue();
            Minimum = valueType.MinValue();
        }

        protected override void OnValueChanged(EventArgs e)
        {
            base.OnValueChanged(e); // The NumericBox's ValueChanged event is raised here.
            valueChanged?.Invoke(this, ((IValueControl)this).Value); // Raise also the IValueControl.ValueChanged event.
        }
    }
}
