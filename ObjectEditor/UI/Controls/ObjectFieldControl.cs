using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;
using TechnosoCommons.Configuration;
using TechnosoCommons.Configuration.UI.Forms;
using TechnosoCommons.Data;
using TechnosoUI.Configuration;
using TechnosoUI.Configuration.UI;
using System.Runtime.InteropServices;

namespace TechnosoCommons.Configuration.UI.Controls
{
    internal class ObjectFieldControl : BaseFieldControl
    {
        private Button SetButton => (Button)ValueControl;

        public ObjectEditorForm ObjectEditorForm { get; private set; }
        public ObjectEditorForm ParentEditorForm { get; }

        /// <summary>
        /// Creates a field control for an object.
        /// </summary>
        /// <param name="value">The object to view and edit.</param>
        /// <param name="fieldInfo">Field information.</param>
        /// <param name="parentForm">The parent form which contains this field.</param>
        public ObjectFieldControl(object value, BaseFieldInfo fieldInfo, ObjectEditorForm parentForm)
            : base(value, fieldInfo)
        {
            ParentEditorForm = parentForm;
        }

        protected override Control CreateValueControl(BaseFieldInfo fieldInfo)
        {
            var btnSet = new Button();
            btnSet.Text = "Set";
            btnSet.UseVisualStyleBackColor = true;
            btnSet.Enabled = false;
            btnSet.Click += BtnSet_Click;
            return btnSet;
        }

        #region UI Events
        private void ObjectEditorForm_ChangesApplied(object sender, ChangesAppliedEventArgs e) { }

        private void ObjectEditorForm_ChangesPendingChanged(object sender, ChangesPendingChangedEventArgs e)
            => Status = e.ChangesPending
                ? Status | FieldStatus.InnerValueChanged  // add the flag
                : Status & ~FieldStatus.InnerValueChanged; // remove the flag

        private void ObjectEditorForm_Closing(object sender, FormClosingEventArgs e)
        { // if is closing by pressing OK, the changes will be applied before closing.
            Status &= ~FieldStatus.InnerValueChanged; // remove the flag to prevent applying unnecessary canceled changes.
        }

        private void BtnSet_Click(object sender, EventArgs e)
        {
            Show:
            try
            {
                object value = Value;
                if (value == null)
                    throw new InvalidOperationException("The value is null.");
                
                if (ObjectEditorForm == null)
                { // create a new form
                    ObjectEditorForm = ObjectEditorFactory.CreateForm(value, ParentEditorForm);
                    ObjectEditorForm.Text = this.Text;
                    ObjectEditorForm.ValueChanged += (s, e) => OnInnerValueChanged(e);
                    ObjectEditorForm.ChangesApplied += ObjectEditorForm_ChangesApplied;
                    ObjectEditorForm.ChangesPendingChanged += ObjectEditorForm_ChangesPendingChanged;
                    ObjectEditorForm.FormClosing += ObjectEditorForm_Closing;
                }

                if (!ObjectEditorForm.Visible) 
                { // was hidden or not created yet - initialize size, position, and values.
                    if (ParentForm != null) ObjectEditorForm.Size = ParentForm.Size;
                    ObjectEditorForm.CenterToParent();
                    ObjectEditorForm.Reset(); // if was not loaded yet, the reset will do nothing (effective).
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

        #region Overrides
        protected override void UpdateControlValue(object value)
        { // the value has changed, dispose the current form.
            var form = ObjectEditorForm;
            if (form != null && !form.IsDisposed && !form.Disposing)
            { // dispose the old form
                form.Close();
                form.Dispose();
            }
            ObjectEditorForm = null;

            UpdateName();
            SetButton.Enabled = value != null; // when pressing the button, the form will be created.
            //btnSet.Text = ObjectEditorForm is CollectionEditorForm ? "Collection" : "Edit";
        }

        public override void Apply()
        {
            // The caller form already set the source object to the parent object property (in case the reference changed)
            Status &= ~FieldStatus.ValueChanged; // remove that flag first, even if an exception will be thrown.
            if (Status.HasFlag(FieldStatus.InnerValueChanged))
                // apply the changes to the source object only if you know about inner changes, to prevent unnecessary changes applied (of a closed form).
                ObjectEditorForm?.ApplyChanges();
            base.Apply();
        }

        protected override void UpdateName()
        {
            base.UpdateName();
            if (ObjectEditorForm != null)
                ObjectEditorForm.Text = this.Text;
        }

        protected override void OnStatusChanged(FieldStatus status)
        {
            base.OnStatusChanged(status);
            UpdateName(); // the display name may change
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (ObjectEditorForm != null)
                ObjectEditorForm.Owner = this.ParentForm;
            base.OnParentChanged(e);
        }
        #endregion
    }
}
