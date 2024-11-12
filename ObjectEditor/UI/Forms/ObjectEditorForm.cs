using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using ObjectEditor.Extensions;
using ObjectEditor.UI.Controls;
using System.Xml;
using System.Xml.Serialization;
using ObjectEditor.Controllers.Editors;
using ObjectEditor.Controllers.Fields;
using ObjectEditor.Controllers;

namespace ObjectEditor.UI.Forms
{
    /// <summary>
    /// A form to view and edit the properties of an object and its sub-objects recursively, of any type.
    /// </summary>
    public partial class ObjectEditorForm : Form
    {
        private readonly Dictionary<ValueFieldController, BaseFieldControl> _fieldControls = new();

        #region Form Properties
        public new Form Owner
        { // There is no virtual method for that, so hiding it using the new keyword.
            get => base.Owner;
            set
            {
                base.Owner = value;
                // If there is an owner, don't show this child form in the taskbar.
                this.ShowInTaskbar = value == null;
            }
        }

        /// <summary>
        /// The parent form that this form was opened from.
        /// </summary>
        private ObjectEditorForm ParentEditorForm { get; }

        /// <summary>
        /// The content panel that contains the fields controls.
        /// </summary>
        private Panel ContentPanel => this.flowLayoutPanel1;

        /// <summary>
        /// Whether to show the add button at the bottom of the content panel.
        /// </summary>
        protected bool ShowAddButton
        {
            get => this.tableLayoutPanel1.RowStyles[2].Height > 0;
            set => this.tableLayoutPanel1.RowStyles[2].Height = value ? 25 : 0;
        }
        #endregion

        /// <summary>
        /// The controller of the object editor. Never null.
        /// </summary>
        public ObjectEditorController Controller { get; }

        #region Constructors
        /// <summary>
        /// Default constructor for the designer.
        /// </summary>
        public ObjectEditorForm() : this(new object()) { }

        /// <param name="sourceObject">The object to show its properties in the form.</param>
        public ObjectEditorForm(object sourceObject) : this(ControllerFactory.CreateEditor(sourceObject), null) { }

        /// <param name="controller">The controller of the object editor.</param>
        /// <param name="parent">The owner of this form (optional).</param>
        internal ObjectEditorForm(ObjectEditorController controller, ObjectEditorForm parent)
        {
            Controller = controller ?? throw new ArgumentNullException(nameof(controller));
            ParentEditorForm = parent;

            InitializeComponent();

            if (controller is CollectionEditorController collectionController)
            {
                this.ShowAddButton = true; // Show the Add button for collections anyway
                this.btnAdd.Enabled = !collectionController.IsReadOnly; // Enable the Add button if the collection is editable
            }

            // TODO: do it when value changed
            this.Enabled = controller.SourceObject != null;

            this.Owner = parent;

            controller.FieldAdded += Controller_FieldAdded;
            controller.FieldRemoved += Controller_FieldRemoved;
            controller.ChangesPendingChanged += Controller_ChangesPendingChanged;
            controller.SaveRequiredChanged += Controller_SaveRequiredChanged;
        }
        #endregion

        #region Form Actions
        /// <summary>
        /// Reload the controls of the form from the source object asynchronously while showing a loading animation.
        /// In case of error, a message box will be shown to the user and no exception will be thrown.
        /// </summary>
        protected async void ReloadAsync()
        {
            try
            {
                loadingPictureBox.Visible = true;
                ContentPanel.Visible = false;
                await Task.Run(() => ((Action)Controller.ReloadFields).InvokeUserAction("Reload"));
            }
            finally
            {
                ContentPanel.Visible = true;
                loadingPictureBox.Visible = false;
            }
        }

        public new void CenterToParent() => base.CenterToParent();

        protected void ScrollDown()
        {
            ContentPanel.InvokeUI(() =>
            {
                if (!ContentPanel.VerticalScroll.Enabled) return;
                ContentPanel.VerticalScroll.Value = ContentPanel.VerticalScroll.Maximum;
                ContentPanel.PerformLayout();
            });
        }
        #endregion

        #region Controller Events
        private void Controller_FieldAdded(object sender, FieldEventArgs e)
        {
            var control = e.Field.CreateFieldControl(this);
            _fieldControls[e.Field] = control;
            ContentPanel.InvokeUI(() => ContentPanel.Controls.Add(control));
        }

        private void Controller_FieldRemoved(object sender, FieldEventArgs e)
        {
            if (_fieldControls.Remove(e.Field, out var control))
                this.InvokeUI(() => control.Dispose()); // will also remove it from the ContentPanel
        }

        private void Controller_ChangesPendingChanged(object sender, ChangesPendingChangedEventArgs e)
        {
            btnReset.Enabled = btnApply.Enabled = e.ChangesPending;
        }

        private void Controller_SaveRequiredChanged(object sender, SaveRequiredChangedEventArgs e)
        {
            btnSave.BackColor = e.SaveRequired ? Color.Orange : Color.Empty;
        }
        #endregion

        #region User Events
        private void BtnOK_Click(object sender, EventArgs e)
        {
            bool applied = ((Action)Controller.ApplyChanges).InvokeUserAction("Apply");
            if (!applied) return;
            Close(); // The closing event will be canceled and the form will be hidden instead.
            // When the closing event is canceled, the dialog result will reset to None,
            // so we need to set it manually even though the button is set as AcceptButton.
            DialogResult = DialogResult.OK;
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            ((Action)Controller.ApplyChanges).InvokeUserAction("Apply");
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            ((Action)Controller.Save).InvokeUserAction("Save");
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ((Action)Controller.Reset).InvokeUserAction("Reset");
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        { // TODO: check where should we call the IgnoreInnerChanges method
            //((Action)(() => Reset())).InvokeUserAction("Cancel");
            Close();
            this.DialogResult = DialogResult.Cancel; // Should be automatically since already was defined as CancelButton
        }

        private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
            => BtnReset_Click(sender, e);

        private void ReloadToolStripMenuItem_Click(object sender, EventArgs e)
            => ReloadAsync();

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = this.Text;
            saveFileDialog1.ShowDialog();
        }

        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private async void OpenFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            var filename = openFileDialog1.FileName;

            await ((Action)(() => { // TODO
                throw new NotImplementedException("The import feature is not implemented yet.");
            })).InvokeUserActionAsync("Import");
        }

        private async void SaveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            var filename = saveFileDialog1.FileName;

            await ((Action)(() => { // TODO
                throw new NotImplementedException("The export feature is not implemented yet.");
            })).InvokeUserActionAsync("Export");
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(menuButton, new Point(0, menuButton.Height));
        }

        private async void BtnAdd_Click(object sender, EventArgs e)
        {
            if (Controller is not CollectionEditorController collectionController)
            {
                MessageBox.Show($"It's not a collection editor.", "Can't add an item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (collectionController.IsReadOnly)
            {   // Can't edit
                MessageBox.Show($"It's a read only collection.", "Can't add an item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // The AddNewItem method will rise the FieldAdded event after the item is added.
            if (await ((Action)collectionController.AddNewItem).InvokeUserActionAsync("New Item"))
                ScrollDown(); // The new item was added successfully, scroll down to see it.
        }
        #endregion

        #region Overrides
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ReloadAsync();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            // Hide any child form when the parent form is hidden.
            if (!this.Visible) foreach (var f in this.OwnedForms) f.Hide();
            base.OnVisibleChanged(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = ParentEditorForm != null;
            if (e.Cancel) // it's a child form, don't close it, just hide it.
            {
                this.Hide();
                this.Owner?.Focus();
            } // else, it's a main form. close it normally, the user will decide what to do.

            base.OnFormClosing(e);
        }
        #endregion
    }
}
