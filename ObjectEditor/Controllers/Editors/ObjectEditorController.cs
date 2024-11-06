﻿using ObjectEditor.Controllers.Fields;
using ObjectEditor.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.Controllers.Editors
{
    /// <summary>
    /// View and edit the properties of an object and its sub-objects recursively, of any type.
    /// </summary>
    public class ObjectEditorController
    {
        #region Properties
        private List<ValueFieldController> _fields = new();
        protected IEnumerable<ValueFieldController> Fields => _fields.AsReadOnly();

        private bool _saveRequired;
        /// <summary>
        /// Data has changed and applied but was not saved to file yet.
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
        /// Data has changed and was not applied to the source object yet.
        /// </summary>
        public bool ChangesPending
        {
            get => _changesPending;
            protected set
            {
                if (_changesPending == value) return;
                _changesPending = value;
                OnChangesPendingChanged(_changesPending);
            }
        }

        /// <summary>
        /// Whether the changes on the source object can be saved to file.
        /// </summary>
        public bool IsSaveable { get; protected set; }
        #endregion

        //public IObjectEditorSettings Settings { get; }

        public object SourceObject { get; }

        #region Events
        /// <summary>
        /// Occurs when one of the values was changed and there is no child controller that can save the changes to a file.
        /// </summary>
        public event EventHandler<FieldValueChangedEventArgs> ValueChanged;
        /// <summary>
        /// The source object was updated (true) or the data was saved (false).
        /// Will not be arise when the changes are on a controller that able to save changes to a file by itself.
        /// </summary>
        public event EventHandler<SaveRequiredChangedEventArgs> SaveRequiredChanged;
        /// <summary>
        /// Changes on this controller was applied to the source object.
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
        public ObjectEditorController() : this(new object()) { }

        /// <param name="sourceObject">The object to view or edit the data of.</param>
        public ObjectEditorController(object sourceObject)
        {
            SourceObject = sourceObject;

            IsSaveable = false; // TODO: Determine if it's possible to save this specific source object.
        }
        #endregion

        #region Action Methods
        /// <summary>
        /// Reload the fields from the source object.
        /// </summary>
        public virtual void ReloadFields()
        {
            Clear();
            if (SourceObject == null) return;
            SourceObject.GetType().GetPropertiesFiltered().ForEachAll(p => AddPropertyField(p));
        }

        /// <summary>
        /// Create a controller for a child field.
        /// </summary>
        /// <param name="fieldInfo">The field information to create a controller for.</param>
        /// <returns>The new created controller.</returns>
        protected virtual ValueFieldController CreateFieldController(FieldMetadata fieldInfo, object value)
        {
            if (fieldInfo == null) return null;
            return fieldInfo.CreateFieldController(value, this);
        }

        /// <summary>
        /// Add a field for a property.
        /// </summary>
        /// <param name="propertyInfo">The property to create a field for.</param>
        /// <returns></returns>
        protected bool AddPropertyField(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) return false;
            if (!propertyInfo.CanRead) return false; // unreadable property

            var fieldInfo = new PropertyFieldMetadata(propertyInfo);
            var value = fieldInfo.GetValue(SourceObject);
            var fieldController = CreateFieldController(fieldInfo, value);
            if (fieldController == null) return false;

            AddField(fieldController);
            return true;
        }

        /// <summary>
        /// Add a child field controller and register its events.
        /// </summary>
        /// <param name="fieldController"></param>
        protected void AddField(ValueFieldController fieldController)
        {
            if (fieldController == null) return;
            _fields.Add(fieldController);
            fieldController.ValueChanged += (s, e) => OnValueChanged(e);
        }

        /// <summary>
        /// Remove a child field controller and unregister its events.
        /// </summary>
        /// <param name="fieldController"></param>
        protected void RemoveField(ValueFieldController fieldController)
        {
            if (fieldController == null) return;
            if (_fields.Remove(fieldController))
                fieldController.ValueChanged -= (s, e) => OnValueChanged(e);
        }

        /// <summary>
        /// Clear all fields.
        /// </summary>
        protected virtual void Clear()
        {
            _fields.Clear();
        }

        /// <summary>
        /// Update the source object from the values of the fields controllers.
        /// </summary>
        public void ApplyChanges()
        {
            if (!ChangesPending) return; // no changes to apply, avoid unnecessary events arise.

            _fields.ForEachAll(f =>
            {
                ApplyField(f); // may throw an exception
                if (f.Status != FieldStatus.Synced) // otherwise, canceled changes may will be saved!
                    f.Apply();
            });
            OnChangesApplied();
        }

        /// <summary>
        /// Apply a field value to the source object.
        /// </summary>
        /// <param name="fieldController"></param>
        protected virtual void ApplyField(ValueFieldController fieldController)
        {
            if (fieldController.FieldInfo is PropertyFieldMetadata p
                && fieldController.Status.HasFlag(FieldStatus.ValueChanged) // otherwise, nullable fields will be set to a value
                && !p.IsReadOnly)
                p.PropertyInfo.SetValue(SourceObject, fieldController.Value?.ChangeType(p.Type));
        }

        /// <summary>
        /// Reload values from source,
        /// will not affect if fields have not yet loaded.
        /// </summary>
        public void Reset()
        {
            _fields.ForEachAll(f => ResetField(f));
            ChangesPending = false;
        }

        /// <summary>
        /// Reset a field to its original value from the source object.
        /// </summary>
        /// <param name="fieldController"></param>
        protected virtual void ResetField(ValueFieldController fieldController)
        {
            // handle only property fields here
            if (fieldController.FieldInfo is PropertyFieldMetadata p)
                fieldController.Value = p.PropertyInfo.GetValue(SourceObject);
        }

        protected virtual void Save()
        {
            ApplyChanges();
            // TODO: if it's possible to save a specific item to its same place on the source file, do it here.
            OnDataSaved();
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
            ChangesPendingChanged?.Invoke(this, e);
        }

        protected void OnSaveRequiredChanged(bool saveRequired) => OnSaveRequiredChanged(new SaveRequiredChangedEventArgs(saveRequired));
        protected virtual void OnSaveRequiredChanged(SaveRequiredChangedEventArgs e)
        {
            e.Saveable |= IsSaveable; // can be saved here or on a child controller.
            SaveRequiredChanged?.Invoke(this, e);
        }

        protected virtual void OnDataSaved() => OnDataSaved(new EventArgs());
        protected virtual void OnDataSaved(EventArgs e)
        {
            SaveRequired = false;
            DataSaved?.Invoke(this, e);
        }
        #endregion
    }
}