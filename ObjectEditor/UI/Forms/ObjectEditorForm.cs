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

namespace ObjectEditor.UI.Forms
{
    /// <summary>
    /// A form to view and edit the properties of an object and its sub-objects recursively, of any type.
    /// </summary>
    public partial class ObjectEditorForm : Form
    {
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

        // Controls
        private Panel ContentPanel => this.flowLayoutPanel1;
        private Panel ButtonsPanel => this.panel1;
        protected Panel CustomPanel => this.panel2;

        /// <summary>
        /// Whether to show the custom panel at the bottom of the content panel.
        /// </summary>
        protected bool ShowCustomPanel
        {
            get => this.tableLayoutPanel1.RowStyles[2].Height > 0;
            set => this.tableLayoutPanel1.RowStyles[2].Height = value ? 25 : 0;
        }

        protected IEnumerable<BaseFieldControl> FieldControls => this.ContentPanel.Controls.OfType<BaseFieldControl>();
        #endregion

        #region Flags Properties
        private bool _saveRequired;
        /// <summary>
        /// Data has changed and was not saved to file yet.
        /// </summary>
        public bool SaveRequired
        {
            get => _saveRequired;
            internal set
            {
                if (_saveRequired == value) return;
                _saveRequired = value;
                OnSaveRequiredChanged(_saveRequired);
            }
        }

        private bool _changesPending;
        /// <summary>
        /// Data has changed and needs to be set on the source object.
        /// </summary>
        public bool ChangesPending
        {
            get => _changesPending;
            private set
            {
                if (_changesPending == value) return;
                _changesPending = value;
                OnChangesPendingChanged(_changesPending);
            }
        }

        private bool _isSaveable;
        /// <summary>
        /// Whether the changes on the source object can be saved to file.
        /// </summary>
        public bool IsSaveable
        {
            get => _isSaveable;
            private set
            {
                if (_isSaveable == value) return;
                _isSaveable = value;
                btnSave.Enabled = btnSave.Visible = _isSaveable;
            }
        }
        #endregion

        #region Settings
        // These properties will be inherited from their parents if was not set
        private bool? _showReadonlyProperties;
        public bool ShowReadonlyProperties
        {
            get => _showReadonlyProperties ?? ParentEditorForm?.ShowReadonlyProperties ?? true;
            set => _showReadonlyProperties = value;
        }
        private bool? _showReferenceProperties;
        public bool ShowReferenceProperties
        {
            get => _showReferenceProperties ?? ParentEditorForm?.ShowReferenceProperties ?? true;
            set => _showReferenceProperties = value;
        }
        #endregion

        public object SourceObject { get; }

        #region Events
        /// <summary>
        /// Occurs when one of the values was changed and there is no form that can save the changes to a file.
        /// </summary>
        public event EventHandler<FieldValueChangedEventArgs> ValueChanged;
        /// <summary>
        /// The source object was updated (true) or the data was saved (false).
        /// Will not be arise when the changes are on a form that able to save changes to file by itself.
        /// </summary>
        public event EventHandler<SaveRequiredChangedEventArgs> SaveRequiredChanged;
        /// <summary>
        /// Changes on this form was applied to the source object.
        /// </summary>
        public event EventHandler<ChangesAppliedEventArgs> ChangesApplied;
        /// <summary>
        /// Occurs when the changes pending flag was changed.
        /// </summary>
        public event EventHandler<ChangesPendingChangedEventArgs> ChangesPendingChanged;
        /// <summary>
        /// The data was saved to file.
        /// </summary>
        public event EventHandler<EventArgs> DataSaved;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor for the designer.
        /// </summary>
        public ObjectEditorForm() : this(new object()) { }

        /// <param name="sourceObject">The object to show its properties in the form.</param>
        public ObjectEditorForm(object sourceObject) : this(sourceObject, null) { }

        /// <param name="sourceObject">The object to show its properties in the form.</param>
        /// <param name="parent">Parent form to inherit settings from (optional).</param>
        internal ObjectEditorForm(object sourceObject, ObjectEditorForm parent)
        {
            SourceObject = sourceObject;
            ParentEditorForm = parent;

            InitializeComponent();

            this.Enabled = sourceObject != null;
            // TODO: reimplement
            //importToolStripMenuItem.Enabled = sourceObject is IMergable && (exportToolStripMenuItem.Enabled = sourceObject is IXmlSerializable);

            IsSaveable = false; // TODO: Determine if it's possible to save this specific source object.

            this.Owner = parent;
            if (parent != null)
                parent.SaveRequiredChanged += ParentEditorForm_SaveRequiredChanged;
        }
        #endregion

        #region Form Actions
        /// <summary>
        /// Reload the controls of the form from the source object asynchronously while showing a loading animation.
        /// In case of error, a message box will be shown to the user and no exception will be thrown.
        /// </summary>
        protected async void ReloadControlsAsync()
        {
            try
            {
                //ContentPanel.SuspendLayout();
                loadingPictureBox.Visible = true;
                ContentPanel.Visible = false;
                await Task.Run(() => ((Action)ReloadControls).InvokeUserAction("Reload"));
            }
            finally
            {
                ContentPanel.Visible = true;
                loadingPictureBox.Visible = false;
                //ContentPanel.ResumeLayout(true);
            }
        }

        /// <summary>
        /// Reload the controls of the form from the source object.
        /// </summary>
        public virtual void ReloadControls()
        {
            this.Clear();

            if (SourceObject == null) return;

            SourceObject.GetType().GetPropertiesFiltered().ForEachAll(p => AddField(p));
        }

        /// <summary>
        /// Create a field control of a field info.
        /// </summary>
        /// <param name="fieldInfo">The field information to create a field control for.</param>
        /// <returns>The new created field control.</returns>
        protected virtual BaseFieldControl CreateFieldControl(BaseFieldInfo fieldInfo, object value)
        {
            if (fieldInfo == null) return null;

            var isSimple = fieldInfo.Type.IsSimpleType();

            if (isSimple && !ShowReadonlyProperties && fieldInfo.IsReadOnly)
                return null; // don't show unwritable value fields
            if (!isSimple && !ShowReferenceProperties)
                return null; // don't show reference fields

            //var value = fieldInfo.GetValue(SourceObject);
            return fieldInfo.CreateFieldControl(value, this);
        }

        /// <summary>
        /// Add a field for a property.
        /// </summary>
        /// <param name="propertyInfo">The property to create a field for.</param>
        /// <returns></returns>
        protected bool AddField(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) return false;
            if (!propertyInfo.CanRead) return false; // unreadable property

            var fieldInfo = new PropertyFieldInfo(propertyInfo);
            var value = fieldInfo.GetValue(SourceObject);
            var fieldControl = CreateFieldControl(fieldInfo, value);
            if (fieldControl == null) return false;

            AddField(fieldControl);
            return true;
        }

        /// <summary>
        /// Add a field control to the form and register its events.
        /// </summary>
        /// <param name="fieldControl"></param>
        protected void AddField(BaseFieldControl fieldControl)
        {
            ContentPanel.InvokeUI(() =>
            {
                fieldControl.ValueChanged += (s, e) => OnValueChanged(e);
                ContentPanel.Controls.Add(fieldControl);
            });
        }

        /// <summary>
        /// Clear all fields controls from the form.
        /// </summary>
        protected virtual void Clear()
        {
            ContentPanel.InvokeUI(() =>
            {
                var oldControls = ContentPanel.Controls;
                ContentPanel.Controls.Clear();
                foreach (Control c in oldControls) c.Dispose(); // also clearing DataBindings
            });
        }

        /// <summary>
        /// Update the source object from the values of the controls.
        /// </summary>
        public void ApplyChanges()
        {
            FieldControls.ForEachAll(f => 
            {
                ApplyField(f); // may throw an exception
                if (f.Status != FieldStatus.Synced) // otherwise, canceled changes of closed forms will be saved!
                    f.Apply();
            });
            OnChangesApplied();
        }

        /// <summary>
        /// Apply a field value to the source object.
        /// </summary>
        /// <param name="fieldControl"></param>
        protected virtual void ApplyField(BaseFieldControl fieldControl)
        {
            if (fieldControl.FieldInfo is PropertyFieldInfo p
                && fieldControl.Status.HasFlag(FieldStatus.ValueChanged) // otherwise, nullable fields will be set to a value
                && !p.IsReadOnly)
                p.PropertyInfo.SetValue(SourceObject, fieldControl.Value?.ChangeType(p.Type));
        }

        /// <summary>
        /// Reload values from source,
        /// will not affect if fields have not yet loaded (before calling the ReloadControls method or first showing, when the form is cleared)
        /// </summary>
        public void Reset()
        {
            FieldControls.ForEachAll(f => ResetField(f));
            ChangesPending = false;
        }

        /// <summary>
        /// Reset a field to its original value from the source object.
        /// </summary>
        /// <param name="fieldControl"></param>
        protected virtual void ResetField(BaseFieldControl fieldControl)
        {
            if (fieldControl.FieldInfo is PropertyFieldInfo p)
                fieldControl.Value = p.PropertyInfo.GetValue(SourceObject);
        }

        protected virtual void Save()
        {
            ApplyChanges();
            // TODO: if it's possible to save a specific item to its same place on the source file, do it here.
            OnDataSaved();
        }
        #endregion

        #region UI Actions
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

        #region UI Events
        private void btnOK_Click(object sender, EventArgs e)
        {
            bool applied = ((Action)(() => ApplyChanges())).InvokeUserAction("Apply");
            if (!applied) return;
            Close(); // The closing event will be canceled and the form will be hidden instead.
            // When the closing event is canceled, the dialog result will reset to None,
            // so we need to set it manually even though the button is set as AcceptButton.
            DialogResult = DialogResult.OK;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ((Action)(() => ApplyChanges())).InvokeUserAction("Apply");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ((Action)(() => Save())).InvokeUserAction("Save");
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ((Action)(() => Reset())).InvokeUserAction("Reset");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //((Action)(() => Reset())).InvokeUserAction("Cancel");
            Close();
            this.DialogResult = DialogResult.Cancel; // Should be automatically since already was defined as CancelButton
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
            => btnReset_Click(sender, e);

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
            => ReloadControlsAsync();

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = this.Text;
            saveFileDialog1.ShowDialog();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private async void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //var filename = openFileDialog1.FileName;

            //import:
            //try
            //{ // TODO: reimplement
            //    object deserialized = null;

            //    await Task.Run(() =>
            //    {
            //        using (var sReader = new StreamReader(filename))
            //        using (var xmlReader = XmlReader.Create(sReader))
            //            deserialized = new XmlSerializer(SourceObject.GetType()).Deserialize(xmlReader);

            //        var mergableElement = SourceObject as IMergable;
            //        if (mergableElement == null) throw new InvalidCastException("Unmergable");
            //        mergableElement.Merge(deserialized, true);
            //    });

            //    Reset(); // refresh controls values
            //    SaveRequired = true;
            //}
            //catch (Exception ex)
            //{
            //    Logging.WriteError(ex);
            //    if (MessageBox.Show(ex.Message, "Import Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Retry)
            //        goto import;
            //}
        }

        private async void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            var filename = saveFileDialog1.FileName;

        export:
            try
            { // TODO: reimplement
                await Task.Run(() =>
                {
                    using (var writer = XmlWriter.Create(filename, new XmlWriterSettings { Indent = true }))
                        new XmlSerializer(SourceObject.GetType()).Serialize(writer, SourceObject);
                });
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.Message, "Export Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Retry)
                    goto export;
            }
        }

        private void ParentEditorForm_SaveRequiredChanged(object sender, SaveRequiredChangedEventArgs e)
        {
            if (!e.SaveRequired) // not required to save on the parent form, probably was saved,
                SaveRequired = false; // tell the children.
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(menuButton, new Point(0, menuButton.Height));
        }
        #endregion

        #region Event Handlers
        protected virtual void OnValueChanged(FieldValueChangedEventArgs e)
        {
            if (e.ByUser) ChangesPending = true;
            if (!IsSaveable) ValueChanged?.Invoke(this, e);
        }

        protected void OnChangesApplied() => OnChangesApplied(new ChangesAppliedEventArgs());
        protected virtual void OnChangesApplied(ChangesAppliedEventArgs e)
        {
            if (ChangesPending) SaveRequired = true;
            ChangesPending = false;
            ChangesApplied?.Invoke(this, e);
        }

        protected virtual void OnChangesPendingChanged(bool changesPending) => OnChangesPendingChanged(new ChangesPendingChangedEventArgs(changesPending));
        protected virtual void OnChangesPendingChanged(ChangesPendingChangedEventArgs e)
        {
            btnReset.Enabled = ChangesPending;
            btnApply.Enabled = ChangesPending;
            ChangesPendingChanged?.Invoke(this, e);
        }

        protected void OnSaveRequiredChanged(bool saveRequired) => OnSaveRequiredChanged(new SaveRequiredChangedEventArgs(saveRequired));
        protected virtual void OnSaveRequiredChanged(SaveRequiredChangedEventArgs e)
        {
            this.InvokeUI(() =>
            {
                btnSave.FlatStyle = _saveRequired ? FlatStyle.Flat : FlatStyle.Standard;

                if (e.SaveRequired) // required to save,
                    if (!IsSaveable) // if we can't save here,
                        if (ParentEditorForm != null) // tell the parent form.
                            ParentEditorForm.SaveRequired = true;
            });

            SaveRequiredChanged?.Invoke(this, e);
        }

        protected virtual void OnDataSaved() => OnDataSaved(new EventArgs());
        protected virtual void OnDataSaved(EventArgs e)
        {
            SaveRequired = false;
            DataSaved?.Invoke(this, e);
        }
        #endregion

        #region Overrides
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ReloadControlsAsync();
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
