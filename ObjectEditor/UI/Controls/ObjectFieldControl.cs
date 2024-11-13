using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using ObjectEditor;
using ObjectEditor.UI.Forms;
using ObjectEditor.Data;
using System.Runtime.InteropServices;
using ObjectEditor.Controllers.Fields;

namespace ObjectEditor.UI.Controls
{
    internal class ObjectFieldControl : BaseFieldControl
    {
        private Button SetButton => (Button)ViewControl;
        private ObjectFieldController ObjectFieldController => (ObjectFieldController)Controller;

        /// <summary>
        /// Editor form for the value object.
        /// </summary>
        public ObjectEditorForm ObjectEditorForm { get; private set; }

        /// <summary>
        /// Creates a field control for an object.
        /// </summary>
        /// <param name="controller">The controller of the field.</param>
        /// <param name="parentForm">The parent form which contains this field.</param>
        public ObjectFieldControl(ObjectFieldController controller, ObjectEditorForm parentForm)
            : base(controller, parentForm) { }

        protected override Control CreateViewControl(FieldMetadata fieldInfo)
        {
            var btnSet = new Button()
            {
                Text = "Set",
                UseVisualStyleBackColor = true,
                Enabled = false
            };

            btnSet.Click += BtnSet_Click;

            return btnSet;
        }

        #region UI Events
        private void ObjectEditorForm_Closing(object sender, FormClosingEventArgs e)
        { // if is closing by pressing OK, the changes will be applied before closing.
            ObjectFieldController.IgnoreInnerChanges(); // prevent applying the changes to the source object.
        }

        private void BtnSet_Click(object sender, EventArgs e)
        {
            Show:
            try
            {
                var objectEditorController = ObjectFieldController.ObjectEditorController;
                if (objectEditorController == null)
                    throw new InvalidOperationException("The value is null.");
                
                if (ObjectEditorForm == null)
                { // create a new form
                    ObjectEditorForm = new ObjectEditorForm(objectEditorController, ParentEditorForm);
                    ObjectEditorForm.Text = this.Text;
                    ObjectEditorForm.FormClosing += ObjectEditorForm_Closing;
                    objectEditorController.UnloadFields(); // in case it was already loaded before connecting to the form.
                }

                if (!ObjectEditorForm.Visible) 
                { // was hidden or not created yet - initialize size, position, and values.
                    if (ParentForm != null) ObjectEditorForm.Size = ParentForm.Size;
                    ObjectEditorForm.CenterToParent();
                    objectEditorController.Reset(); // if was not loaded yet, the reset will do nothing
                    // TODO: focus to the containing control
                }

                ObjectEditorForm.Show(); // the controls will be loaded before showing for the first time.
                ObjectEditorForm.Focus(); // if the form was already shown, it will be focused anyway.
            }
            catch(Exception ex)
            {
                if (MessageBox.Show(ex.Message, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                    goto Show;
            }
        }
        #endregion

        /// <summary>
        /// Disposes the editor form and sets the reference to null.
        /// </summary>
        private void DisposeEditorForm()
        {
            var form = ObjectEditorForm;
            if (form != null && !form.IsDisposed && !form.Disposing)
            { // dispose the old form
                form.Close();
                form.Dispose();
            }
            ObjectEditorForm = null;
        }

        #region Overrides
        protected override void UpdateValueControl(object value)
        {
            if (ObjectFieldController.ObjectEditorController == ObjectEditorForm?.Controller)
                return; // the value has not changed.

            DisposeEditorForm(); // the value has changed, dispose the current form.
            SetButton.Enabled = value != null; // when pressing the button, the form will be created.
        }

        protected override void UpdateControl()
        {
            base.UpdateControl();
            if (ObjectEditorForm != null)
                ObjectEditorForm.Text = this.Text;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (ObjectEditorForm != null)
                ObjectEditorForm.Owner = this.ParentForm;
            base.OnParentChanged(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) DisposeEditorForm();
            base.Dispose(disposing);
        }
        #endregion
    }
}
