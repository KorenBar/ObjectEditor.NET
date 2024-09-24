using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TechnosoCommons.UI.Forms
{
    /// <summary>
    /// A form that can be statically accessed by its type and id, with some custom features.
    /// </summary>
    /// <typeparam name="T">The type of the inheriting form class to accessable statically.</typeparam>
    public partial class ControllingForm<T> : Form where T : class
    {
        protected static Dictionary<string, T> _FormsCollection = new Dictionary<string, T>();

        public static T GetForm(string id) => _FormsCollection.ContainsKey(id) ? _FormsCollection[id] : null;

        public ControllingForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            opacityTrackBar.Value = (int)Math.Min(opacityTrackBar.Maximum, Math.Max(opacityTrackBar.Minimum, this.Opacity * 100));
        }

        public ControllingForm(string id) : this()
        {
            _FormsCollection[id] = this as T;
        }

        private void ControllingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (e.CloseReason != CloseReason.ApplicationExitCall && e.CloseReason != CloseReason.WindowsShutDown && e.CloseReason != CloseReason.FormOwnerClosing);
            this.Hide();
            this.Owner?.Focus();
        }

        private void opacityTrackBar_ValueChanged(object sender, EventArgs e) => this.Opacity = opacityTrackBar.Value / 100.0;
    }
}
