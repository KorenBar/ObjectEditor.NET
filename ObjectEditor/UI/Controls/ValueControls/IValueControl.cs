using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.WinForms.Controls
{
    /// <summary>
    /// Represents a control that can be used to display and edit a value.
    /// </summary>
    public interface IValueControl
    {
        /// <summary>
        /// Gets the control that is used to display the value.
        /// </summary>
        internal Control Control { get; }

        /// <summary>
        /// Gets or sets the value of the control.
        /// </summary>
        object Value { get; internal set; }

        /// <summary>
        /// Gets or sets whether the control is read-only.
        /// </summary>
        bool ReadOnly { get; internal set; }

        /// <summary>
        /// Occurs when the value of the control has changed.
        /// </summary>
        event EventHandler<object> ValueChanged;

        /// <summary>
        /// Initializes the control for the specified value type.
        /// </summary>
        /// <param name="valueType">The type of the value that the control will display.</param>
        internal void Initialize(Type valueType);
    }
}
