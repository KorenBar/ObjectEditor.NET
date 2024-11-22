using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectEditor.Controllers.Editors;

namespace ObjectEditor.Controllers.Fields
{
    /// <summary>
    /// Field controller for an object value.
    /// </summary>
    public class ObjectFieldController : ValueFieldController
    {
        /// <summary>
        /// The controller for the object value of this field.
        /// </summary>
        public ObjectEditorController ObjectEditorController { get; private set; }

        /// <summary>
        /// Creates a field controller for an object.
        /// </summary>
        /// <param name="value">Initial field value</param>
        /// <param name="fieldInfo">Field information</param>
        /// <param name="parentController">The controller that contains this field.</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal ObjectFieldController(object value, FieldMetadata fieldInfo, ObjectEditorController parentController)
            : base(value, fieldInfo, parentController)
        {
            ParentEditorController.SaveRequiredChanged += ParentEditorController_SaveRequiredChanged;
        }

        #region Parent Events
        private void ParentEditorController_SaveRequiredChanged(object sender, SaveRequiredChangedEventArgs e)
        {
            if (!e.SaveRequired) // a parent saved the applied data (including its children),
                if (ObjectEditorController != null) // tell the children.
                    ObjectEditorController.SaveRequired = false;
        }
        #endregion

        #region Inner Events
        private void ObjectEditorController_SaveRequiredChanged(object sender, SaveRequiredChangedEventArgs e)
        {
            if (e.SaveRequired) // required to save,
                if (!e.Saveable) // can't be saved by one of the children,
                    if (ParentEditorController != null) // tell the parent.
                        ParentEditorController.SaveRequired = true;
        }

        private void ObjectEditorController_ChangesPendingChanged(object sender, ChangesPendingChangedEventArgs e)
        {
            if (!e.ChangesPending)
                Status &= ~FieldStatus.InnerValueChanged; // remove the flag
        }
        #endregion

        /// <summary>
        /// Removes the InnerValueChanged flag until the next inner change, to prevent applying unnecessary canceled changes without resetting this field.
        /// </summary>
        public void IgnoreInnerChanges()
        {
            Status &= ~FieldStatus.InnerValueChanged;
        }

        public override void Apply()
        {
            // The caller controller already set the source object to the parent object property (in case the reference changed)
            Status &= ~FieldStatus.ValueChanged; // remove that flag first, even if an exception will be thrown.
            if (Status.HasFlag(FieldStatus.InnerValueChanged))
                // apply the changes to the source object only if you know about inner changes, to prevent unnecessary ignored changes applied.
                ObjectEditorController?.ApplyChanges();
            base.Apply();
        }

        protected override void OnValueChanged(FieldValueChangedEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            if (e.Sender == this) // the object value of this field has changed, update the controller before invoking the event
            {
                // remove the old controller (disposing it unregisters the events)
                ObjectEditorController?.Dispose();
                ObjectEditorController = null;

                if (e.NewValue != null)
                { // the source object has changed or the controller is null, create a new controller.
                    var editor = ControllerFactory.CreateEditor(e.NewValue, ParentEditorController?.Settings);
                    editor.ValueChanged += (s, e) => OnInnerValueChanged(e);
                    editor.ChangesPendingChanged += ObjectEditorController_ChangesPendingChanged;
                    editor.SaveRequiredChanged += ObjectEditorController_SaveRequiredChanged;
                    ObjectEditorController = editor;
                }
            }

            base.OnValueChanged(e);
        }

        public override void Dispose()
        {
            base.Dispose();
            ObjectEditorController?.Dispose();
        }
    }
}
