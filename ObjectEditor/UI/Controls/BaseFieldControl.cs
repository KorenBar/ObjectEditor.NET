using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;
using TechnosoUI.Configuration;
using TechnosoUI.Configuration.UI;
using TechnosoCommons.Extensions;

namespace TechnosoCommons.Configuration.UI.Controls
{
    /// <summary>
    /// Base class for all types of fields in an object editor.
    /// </summary>
    public abstract partial class BaseFieldControl : UserControl
    {
        /// <summary>
        /// The value copied by the user to paste or link to another field.
        /// </summary>
        private static object _copyValue;

        #region Properties
        private object _value;
        /// <summary>
        /// Gets or sets the field value.
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
        /// Gets the field information.
        /// </summary>
        public BaseFieldInfo FieldInfo { get; }

        /// <summary>
        /// Gets the control that displays the value.
        /// Created by the inherited class.
        /// </summary>
        protected Control ValueControl { get; }

        /// <summary>
        /// Gets or sets whether the remove button is visible.
        /// </summary>
        public bool ShowRemoveButton
        {
            get => tableLayoutPanel1.ColumnStyles[2].Width > 0;
            protected set => tableLayoutPanel1.ColumnStyles[2].Width = value ? tableLayoutPanel1.Height : 0;
        }

        private FieldStatus _status;
        /// <summary>
        /// Gets the status of the field.
        /// </summary>
        public FieldStatus Status
        {
            get => _status;
            protected set
            {
                if (_status == value) return;
                _status = value;
                OnStatusChanged(value);
            }
        }
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
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new field control.
        /// </summary>
        /// <param name="value">Initial field value</param>
        /// <param name="fieldInfo">Field information</param>
        /// <exception cref="ArgumentNullException"></exception>
        public BaseFieldControl(object value, BaseFieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException(nameof(fieldInfo));

            FieldInfo = fieldInfo;

            InitializeComponent();

            if (fieldInfo is ItemFieldInfo itemFieldInfo)
            {
                ShowRemoveButton = true;
                btnRemove.Enabled = itemFieldInfo.IsRemovable;
            }

            if (fieldInfo.IsReadOnly)
                label1.ForeColor = Color.Gray;

            if (fieldInfo.Description != null) // underline described fields
                label1.Font = new Font(label1.Font, FontStyle.Underline);

            ValueControl = CreateValueControl(fieldInfo);
            if (ValueControl == null)
                throw new InvalidOperationException("The value control is not created.");

            ValueControl.Dock = DockStyle.Fill;
            valueControlPanel.Controls.Add(ValueControl);

            Value = value;
            UpdateName(); // set the name after the value is set
            UpdateTip();
        }
        #endregion

        #region Abstract Methods
        /// <summary>
        /// Initializes the field value control before the value is set.
        /// </summary>
        /// <param name="fieldInfo">Field information.</param>
        /// <returns>The created control to display and edit the value.</returns>
        protected abstract Control CreateValueControl(BaseFieldInfo fieldInfo);
        /// <summary>
        /// Sets the value of the value control, depending on the field type.
        /// </summary>
        /// <param name="value">The value to set.</param>
        protected abstract void UpdateControlValue(object value);
        #endregion

        #region Field Actions
        /// <summary>
        /// Sets the value of the field.
        /// </summary>
        /// <param name="byUser">Indicates whether the value is set by the user.</param>
        protected void SetValue(object value, bool byUser)
        {
            if (_value == value) return;

            value = value?.ChangeType(FieldInfo.Type); // convert first, may will throw an exception

            if (_value != null && value != null && _value.Equals(value))
                return; // in some cases the value is the same, but the reference is different, like with strings

            if (byUser && FieldInfo.IsReadOnly) // the user can't change the value of a read-only field, but the program can update it.
                throw new InvalidOperationException("The field is read-only.");

            if (value == null && !FieldInfo.IsNullable) // it's not a reference or nullable type
                throw new InvalidOperationException($"Can't set value of type {FieldInfo.Type.Name} to null.");

            _value = value;
            OnValueChanged(new FieldValueChangedEventArgs(this, value, byUser));
        }

        /// <summary>
        /// Applies the changes made.
        /// </summary>
        public virtual void Apply()
        {
            Status = FieldStatus.Synced;
        }

        /// <summary>
        /// Takes the name from the field info or from the display name properties values on the source object itself.
        /// </summary>
        protected virtual void UpdateName() => this.Text = FieldInfo.Name ?? this.Value.GetDisplayName()
            ?? (FieldInfo is ItemFieldInfo info ? $"[{info.Index}]" : null);

        private void UpdateTip()
        {
            toolTip1.RemoveAll();
            toolTip1.SetToolTip(label1, FieldInfo.Tip ?? this.Text);
        }
        #endregion

        #region UI Events
        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("The value will be removed with no option to be returned. \nDo you want to continue?", $"Removing {this.Text}", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            ((Action)(() => OnRemoving())).InvokeUserAction("Remove");
        }

        private void fieldMenu_Opening(object sender, CancelEventArgs e)
        {
            var fieldType = FieldInfo.Type;
            copyToolStripMenuItem.Enabled = Value != null;
            pasteToolStripMenuItem.Enabled = _copyValue != null && !FieldInfo.IsReadOnly && fieldType.IsAssignableFrom(_copyValue.GetType()) && fieldType.IsSimpleType();
            linkToolStripMenuItem.Enabled = _copyValue != null && !fieldType.IsSimpleType() && _copyValue.GetType().IsAssignableTo(FieldInfo.Type) && !FieldInfo.IsReadOnly;
            setNullToolStripMenuItem.Enabled = FieldInfo.IsNullable && !FieldInfo.IsReadOnly;
            createDefaultToolStripMenuItem.Enabled = fieldType.IsValueType || fieldType.Equals(typeof(string)) || fieldType.GetConstructor(Type.EmptyTypes) != null && !FieldInfo.IsReadOnly;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _copyValue = Value;
        }

        private void linkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Action)(() =>
            {
                var value = _copyValue;
                if (value == null)
                    throw new InvalidOperationException("The value to link is null.");
                if (!value.GetType().IsAssignableTo(FieldInfo.Type))
                    throw new InvalidOperationException($"Can't link value of type {value.GetType().Name} to {FieldInfo.Type.Name}.");
                SetValue(value, true);
            }))
            .InvokeUserAction("Link Value");
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Action)(() =>
            {
                if (_copyValue == null)
                    throw new InvalidOperationException("The value to paste is null.");
                if (!FieldInfo.Type.IsAssignableFrom(_copyValue.GetType()))
                    throw new InvalidOperationException($"Can't paste value of type {_copyValue.GetType().Name} to {FieldInfo.Type.Name}.");
                if (!FieldInfo.Type.IsSimpleType())
                    throw new InvalidOperationException("Can't paste a class type.");
                SetValue(_copyValue, true);
            }))
            .InvokeUserAction("Paste Value");
        }

        private void setNullToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Action)(() =>
            {
                if (!FieldInfo.IsNullable)
                    throw new InvalidOperationException("The field is not nullable.");
                SetValue(null, true);
            }))
            .InvokeUserAction("Set Null");
        }

        private void createDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Action)(() =>
            {
                SetValue(FieldInfo.Type.GetDefaultValue(), true);
            }))
            .InvokeUserAction("Create Default");
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
            var itemInfo = FieldInfo as ItemFieldInfo;
            if (itemInfo == null)
                throw new InvalidOperationException("The field is not an item field.");
            if (!itemInfo.Abilities.HasFlag(ItemAbility.Remove))
                throw new InvalidOperationException("The item can't be removed, no remove ability.");

            Removing?.Invoke(this, e);

            if (!IsDisposed && !Disposing)
                Dispose();
        }

        /// <summary>
        /// Occurs when the user changes the value of the field.
        /// The inherited class should call this method when the user changes the value.
        /// </summary>
        /// <param name="value">The new value set by the user.</param>
        /// <exception cref="InvalidOperationException">The field is read-only.</exception>
        protected virtual void OnUserChangedValue(object value)
        {
            Action action = () => SetValue(value, true);
            if (!action.InvokeUserAction("Value Change")) // failed
                UpdateControlValue(Value); // revert the value
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

            UpdateControlValue(e.Value);
            nullLabel.Visible = e.Value == null;

            ValueChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when the a child field value changes.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnInnerValueChanged(FieldValueChangedEventArgs e)
        {
            e.AddParentField(this); // creates a path to the changed field
            ValueChanged?.Invoke(this, e); // chain the event
        }

        /// <summary>
        /// Occurs when the status of the field changes.
        /// </summary>
        /// <param name="status"></param>
        protected virtual void OnStatusChanged(FieldStatus status)
        {
            BackColor = _status.HasFlag(FieldStatus.ValueChanged) ? Color.Orange
                : _status.HasFlag(FieldStatus.InnerValueChanged) ? Color.Yellow
                : Color.Empty;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            label1.Text = this.Text;
            UpdateTip();
            base.OnTextChanged(e);
        }
        #endregion
    }
}
