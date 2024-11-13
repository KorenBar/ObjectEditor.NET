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
using ObjectEditor.Controllers.Fields;
using System.Reflection.Emit;

namespace ObjectEditor.WinForms.Controls
{
    /// <summary>
    /// Base class for all types of fields in an object editor.
    /// </summary>
    public abstract partial class BaseFieldControl : UserControl
    {
        #region Properties
        /// <summary>
        /// The controller of the field. Never null.
        /// </summary>
        public ValueFieldController Controller { get; }

        /// <summary>
        /// Get the control that displays the value.
        /// </summary>
        protected Control ViewControl { get; }

        /// <summary>
        /// Get the parent form that contains this field.
        /// </summary>
        public ObjectEditorForm ParentEditorForm { get; }

        /// <summary>
        /// Get or set whether the remove button is visible.
        /// </summary>
        public bool ShowRemoveButton
        {
            get => tableLayoutPanel1.ColumnStyles[2].Width > 0;
            protected set => tableLayoutPanel1.ColumnStyles[2].Width = value ? tableLayoutPanel1.Height : 0;
        }

        /// <summary>
        /// Get or set whether the name label is visible.
        /// </summary>
        public bool ShowNameLabel
        {
            get => tableLayoutPanel1.ColumnStyles[0].Width > 0;
            protected set => tableLayoutPanel1.ColumnStyles[0].Width = value ? 140F : 0;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new field control.
        /// </summary>
        /// <param name="controller">The controller of the field.</param>
        /// <param name="parentForm">The parent form that contains this field.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public BaseFieldControl(ValueFieldController controller, ObjectEditorForm parentForm)
        {
            Controller = controller ?? throw new ArgumentNullException(nameof(controller));
            ParentEditorForm = parentForm;

            InitializeComponent();

            var fieldInfo = controller.FieldInfo;

            if (fieldInfo is SubFieldMetadata)
                ShowNameLabel = false; // hide the name label for sub fields
            else if (fieldInfo is ItemFieldMetadata itemFieldInfo)
            {
                ShowRemoveButton = true;
                btnRemove.Enabled = itemFieldInfo.IsRemovable;
            }

            if (fieldInfo.IsReadOnly)
                label1.ForeColor = Color.Gray;

            if (fieldInfo.Description != null) // underline described fields
                label1.Font = new Font(label1.Font, FontStyle.Underline);

            ViewControl = CreateViewControl(fieldInfo);
            if (ViewControl == null)
                throw new NotImplementedException("The value control is not created.");

            ViewControl.Dock = DockStyle.Fill;
            viewControlPanel.Controls.Add(ViewControl);

            // don't update the control here, it will be updated when the form is loaded
        }
        #endregion

        #region Abstract Methods
        /// <summary>
        /// Initializes the field value control before the value is set.
        /// </summary>
        /// <param name="fieldInfo">Field information.</param>
        /// <returns>The created control to display and edit the value.</returns>
        protected abstract Control CreateViewControl(FieldMetadata fieldInfo);
        /// <summary>
        /// Sets the value of the value control, depending on the field type.
        /// </summary>
        /// <param name="value">The value to set, will never be null.</param>
        protected abstract void UpdateValueControl(object value);
        #endregion

        #region Field Actions
        /// <summary>
        /// Updates the control with the current value and name from the controller.
        /// </summary>
        protected virtual void UpdateControl()
        {
            var value = Controller.Value;
            nullLabel.Visible = value == null;
            viewControlPanel.Visible = value != null;
            UpdateValueControl(value);

            var name = Controller.Name;
            Text = name;
            label1.Text = name;
            toolTip1.RemoveAll();
            toolTip1.SetToolTip(label1, Controller.FieldInfo.Tip ?? name);
        }
        #endregion

        #region UI Events
        private void BtnRemove_Click(object sender, EventArgs e)
        {
            // prompt
            if (MessageBox.Show("The value will be removed with no option to be returned. \nDo you want to continue?", $"Removing {this.Text}", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            ((Action)Controller.Remove).InvokeUserAction("Remove");
        }

        private void FieldMenu_Opening(object sender, CancelEventArgs e)
        {
            copyToolStripMenuItem.Enabled = Controller.CanCopy;
            pasteToolStripMenuItem.Enabled = Controller.CanPaste;
            linkToolStripMenuItem.Enabled = Controller.CanLink;
            setNullToolStripMenuItem.Enabled = Controller.CanSetNull;
            createDefaultToolStripMenuItem.Enabled = Controller.CanSetDefault;
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Action)Controller.CopyValue).InvokeUserAction("Copy Value");
        }

        private void LinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Action)Controller.LinkValue).InvokeUserAction("Link Value");
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Action)Controller.PasteValue).InvokeUserAction("Paste Value");
        }

        private void SetNullToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Action)Controller.SetNull).InvokeUserAction("Set Null");
        }

        private void CreateDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Action)Controller.SetDefault).InvokeUserAction("Create Default");
        }
        #endregion

        #region Controller Events
        private void Controller_Removing(object sender, EventArgs e)
        {
            if (!IsDisposed && !Disposing)
                Dispose(); // remove and free the control
        }

        private void Controller_StatusChanged(object sender, ValueChangedEventArgs<FieldStatus> e)
        {
            BackColor = e.NewValue.HasFlag(FieldStatus.ValueChanged) ? Color.Orange
                : e.NewValue.HasFlag(FieldStatus.InnerValueChanged) ? Color.Yellow
                : Color.Empty;

            if (e.NewValue == FieldStatus.Synced) // applied or reset
                UpdateControl();
        }

        private void Controller_ValueChanged(object sender, ValueChangedEventArgs<object> e)
        {
            UpdateControl();
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Occurs when the user changes the value of the field.
        /// The inherited class should call this method when the user changes the value.
        /// </summary>
        /// <param name="value">The new value set by the user.</param>
        /// <exception cref="InvalidOperationException">The field is read-only.</exception>
        protected virtual void OnUserChangedValue(object value)
        {
            if (Disposing || IsDisposed)
                return; // ignore user events after disposing

            Action action = () => Controller.SetValue(value, true);
            if (!action.InvokeUserAction("Value Change"))
                UpdateControl(); // failed, revert changes
        }
        #endregion

        #region Overrides
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            UpdateControl(); // set the initial value before registering events

            // Add events here to prevent rising when showing the form.
            Controller.Removing += Controller_Removing;
            Controller.ValueChanged += Controller_ValueChanged;
            Controller.StatusChanged += Controller_StatusChanged;
        }
        #endregion
    }
}
