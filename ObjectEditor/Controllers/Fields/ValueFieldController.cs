using ObjectEditor.Controllers.Editors;
using ObjectEditor.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.Controllers.Fields
{
    /// <summary>
    /// A controller for a field in an object editor.
    /// </summary>
    public class ValueFieldController
    {
        /// <summary>
        /// The value copied by the user to paste or link to another field.
        /// </summary>
        private static object _copyValue;

        #region Properties
        private object _value;
        /// <summary>
        /// Get or set the field value.
        /// </summary>
        public object Value
        {
            get => _value;
            set
            {
                SetValue(value, false);
                Status = FieldStatus.Synced;
            }
        }

        /// <summary>
        /// Get the field information.
        /// </summary>
        public FieldMetadata FieldInfo { get; }

        /// <summary>
        /// Get the containing controller of this field.
        /// </summary>
        public ObjectEditorController ParentEditorController { get; }

        private FieldStatus _status;
        /// <summary>
        /// Get the status of the field.
        /// </summary>
        public FieldStatus Status
        {
            get => _status;
            protected set
            {
                if (_status == value) return;
                var oldValue = _status;
                _status = value;
                OnStatusChanged(new ValueChangedEventArgs<FieldStatus>(oldValue, value));
            }
        }

        /// <summary>
        /// Get the name from the field info, or from the display name properties values on the source object itself, or the index if it's an item field.
        /// The first non-null value is returned.
        /// </summary>
        public string Name => FieldInfo.Name ?? Value?.GetDisplayName() ?? (FieldInfo is ItemFieldMetadata info ? $"[{info.Index}]" : null);

        // Abilities Properties
        public bool CanRemove => FieldInfo is ItemFieldMetadata itemInfo && itemInfo.Abilities.HasFlag(ItemAbility.Remove);
        public bool CanCopy => Value != null;
        public bool CanPaste => _copyValue != null && !FieldInfo.IsReadOnly && FieldInfo.Type.IsAssignableFrom(_copyValue.GetType()) && FieldInfo.Type.IsSimpleType();
        public bool CanLink => _copyValue != null && !FieldInfo.Type.IsSimpleType() && _copyValue.GetType().IsAssignableTo(FieldInfo.Type) && !FieldInfo.IsReadOnly;
        public bool CanSetNull => FieldInfo.IsNullable && !FieldInfo.IsReadOnly;
        public bool CanSetDefault => FieldInfo.Type.IsValueType || FieldInfo.Type.Equals(typeof(string)) || FieldInfo.Type.GetConstructor(Type.EmptyTypes) != null && !FieldInfo.IsReadOnly;
        #endregion

        #region Events
        /// <summary>
        /// Occurs after the field value changes.
        /// </summary>
        public event EventHandler<FieldValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// Occurs when the user requires to remove this item from the collection.
        /// </summary>
        public event EventHandler<EventArgs> Removing;

        /// <summary>
        /// Occurs when the status of the field changes.
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<FieldStatus>> StatusChanged;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new field controller.
        /// </summary>
        /// <param name="value">Initial field value</param>
        /// <param name="fieldInfo">Field information</param>
        /// <param name="parentController">The controller that contains this field.</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal ValueFieldController(object value, FieldMetadata fieldInfo, ObjectEditorController parentController)
        {
            if (fieldInfo == null) throw new ArgumentNullException(nameof(fieldInfo));
            if (parentController == null) throw new ArgumentNullException(nameof(parentController));

            FieldInfo = fieldInfo;
            ParentEditorController = parentController;
            Value = value;
        }
        #endregion

        #region Field Actions
        /// <summary>
        /// Sets the value of the field.
        /// </summary>
        /// <param name="byUser">Indicates whether the value is set by the user.</param>
        internal void SetValue(object value, bool byUser)
        {
            if (_value == value) return;

            value = value?.ChangeType(FieldInfo.Type); // convert first, may will throw an exception

            if (_value != null && value != null && _value.Equals(value))
                return; // in some cases the value is the same, but the reference is different, like with strings

            if (byUser && FieldInfo.IsReadOnly) // the user can't change the value of a read-only field, but the program can update it.
                throw new InvalidOperationException("The field is read-only.");

            if (value == null && !FieldInfo.IsNullable) // it's not a reference or nullable type
                throw new InvalidOperationException($"Can't set value of type {FieldInfo.Type.Name} to null.");

            var oldValue = _value;
            _value = value;
            OnValueChanged(new FieldValueChangedEventArgs(this, oldValue, value, byUser));
        }

        /// <summary>
        /// Applies the changes made.
        /// </summary>
        public virtual void Apply()
        {
            Status = FieldStatus.Synced;
        }

        /// <summary>
        /// Tells the parent controller to remove this field.
        /// </summary>
        /// <exception cref="InvalidOperationException">if the field is not an item field or doesn't have the remove ability.</exception>
        public virtual void Remove()
        {
            if (FieldInfo is ItemFieldMetadata itemInfo)
            {
                if (!itemInfo.Abilities.HasFlag(ItemAbility.Remove))
                    throw new InvalidOperationException("The item can't be removed, no remove ability.");

                OnRemoving();
            }
            else
                throw new InvalidOperationException("The field is not an item field.");
        }

        /// <summary>
        /// Copies the value of this field to paste or link to another field.
        /// </summary>
        public void CopyValue()
        {
            _copyValue = Value;
        }

        /// <summary>
        /// Links the copied value to this field.
        /// </summary>
        /// <exception cref="InvalidOperationException">if the copied value is null or the types are not compatible.</exception>
        public void LinkValue()
        {
            var value = _copyValue;
            if (value == null)
                throw new InvalidOperationException("No value is copied.");
            LinkValue(value);
        }
        /// <summary>
        /// Links the specified value to this field.
        /// </summary>
        /// <param name="value">The value to link.</param>
        /// <exception cref="ArgumentNullException">if the value is null.</exception>
        /// <exception cref="InvalidOperationException">if the types are not compatible.</exception>
        public void LinkValue(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (!value.GetType().IsAssignableTo(FieldInfo.Type))
                throw new InvalidOperationException($"Can't link value of type {value.GetType().Name} to {FieldInfo.Type.Name}.");
            SetValue(value, true);
        }

        /// <summary>
        /// Pastes the copied value to this field.
        /// </summary>
        /// <exception cref="InvalidOperationException">if the copied value is null, the types are not compatible, or the field is not a simple type.</exception>
        public void PasteValue()
        {
            if (_copyValue == null)
                throw new InvalidOperationException("No value is copied.");
            if (!FieldInfo.Type.IsAssignableFrom(_copyValue.GetType()))
                throw new InvalidOperationException($"Can't paste value of type {_copyValue.GetType().Name} to {FieldInfo.Type.Name}.");
            if (!FieldInfo.Type.IsSimpleType())
                throw new InvalidOperationException("Can't paste a class type.");
            SetValue(_copyValue, true);
        }

        /// <summary>
        /// Sets the field value to null.
        /// </summary>
        /// <exception cref="InvalidOperationException">if the field is not nullable.</exception>
        public void SetNull()
        {
            if (!FieldInfo.IsNullable)
                throw new InvalidOperationException("The field is not nullable.");
            SetValue(null, true);
        }

        /// <summary>
        /// Sets the field value to the default value of the type.
        /// </summary>
        public void SetDefault()
        {
            SetValue(FieldInfo.Type.GetDefaultValue(), true);
        }
        #endregion

        #region Event Handlers
        protected void OnRemoving() => OnRemoving(new EventArgs());
        /// <summary>
        /// Occurs when the user requires to remove this item from the collection.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRemoving(EventArgs e)
        {
            var itemInfo = FieldInfo as ItemFieldMetadata;
            if (itemInfo == null)
                throw new InvalidOperationException("The field is not an item field.");
            if (!itemInfo.Abilities.HasFlag(ItemAbility.Remove))
                throw new InvalidOperationException("The item can't be removed, no remove ability.");

            Removing?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when the field value changes.
        /// </summary>
        /// <param name="value">The new value of the field.</param>
        protected virtual void OnValueChanged(FieldValueChangedEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            if (e.ByUser) // the value changed by the user, mark the field as changed
                Status |= FieldStatus.ValueChanged;

            ValueChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when the a child field value changes.
        /// </summary>
        protected virtual void OnInnerValueChanged(FieldValueChangedEventArgs e)
        { // located here instead of the ObjectFieldController, since the chained event can be raised from here only.
            e.AddParentField(this); // creates a path to the changed field
            ValueChanged?.Invoke(this, e); // chain the event
        }

        /// <summary>
        /// Occurs when the status of the field changes.
        /// </summary>
        protected virtual void OnStatusChanged(ValueChangedEventArgs<FieldStatus> e)
        {
            StatusChanged?.Invoke(this, e);
        }
        #endregion
    }
}
