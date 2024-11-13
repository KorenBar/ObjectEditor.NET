using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ObjectEditor.WinForms.Controls
{
    internal class NumericBox : NumericUpDown
    { // Fixing the problem with the mouse wheel increment.
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            HandledMouseEventArgs hme = e as HandledMouseEventArgs;
            if (hme != null)
                hme.Handled = true;

            try 
            { // inside a try block because an exception may will be thrown if the result is bigger/smaller than the max/min value of Decimal.
                var newValue = this.Value;
                if (e.Delta > 0) newValue += this.Increment;
                else if (e.Delta < 0) newValue -= this.Increment;

                if (this.Value != newValue) this.Value = Limit(newValue);
            }
            catch { }
        }
        private decimal Limit(decimal value) => Math.Min(this.Maximum, Math.Max(this.Minimum, value));
    }
}
