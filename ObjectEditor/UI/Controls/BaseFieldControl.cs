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
using System.Diagnostics;
using TechnosoCommons.Extensions;

namespace TechnosoCommons.Configuration.UI.Controls
{
    /// <summary>
    /// Base class for all types of fields in an object editor.
    /// </summary>
    public abstract partial class BaseFieldControl : UserControl
    {
        #region Properties
        private bool _settingValue;

        /// <summary>
        /// Gets whether the field value is set to null.
        /// Can be true for nullable or reference types only.
        /// </summary>
        protected bool IsNull
        {
            get => false;//nullCheckbox.Checked;
            private set
            {
                if (value == IsNull) return; // no change
                // TODO: Create a checkbox and set it to rise the OnValueChanged event.
                //nullCheckbox.Checked = value; // will rise its CheckedChanged event
                // TODO: ControlContainer.Enabled = !value; // DON'T set the visibility of the control itself, it may be enabled/disabled by the field info.
                // En\Disable the control in the 2nd ([1]) column of tableLayoutPanel1
            }
        }

        /// <summary>
        /// Gets or sets the field value.
        /// ValueChanged event will be ignored when setting the value.
        /// </summary>
        public object Value
        {
            get => !IsNull ? GetValue() : null;
            set
            {
                try
                {
                    _settingValue = true; // suspend the ValueChanged event

                    if (value == null)
                    {
                        if (!FieldInfo.IsNullable) // it's not a reference or nullable type
                            throw new InvalidOperationException($"Can't set value of type {FieldInfo.Type.Name} to null.");
                        IsNull = true;
                    }
                    else
                    {
                        IsNull = false;
                        SetValue(value);
                    }
                }
                finally { _settingValue = false; }
            }
        }

        /// <summary>
        /// Gets the field information.
        /// </summary>
        public BaseFieldInfo FieldInfo { get; }

        /// <summary>
        /// Gets or sets whether the remove button is visible.
        /// </summary>
        public bool ShowRemoveButton
        {
            get => this.tableLayoutPanel1.ColumnStyles[2].Width > 0;
            protected set => this.tableLayoutPanel1.ColumnStyles[2].Width = value ? 27 : 0;
        }

        /// <summary>
        /// Gets or sets whether the null checkbox is visible.
        /// </summary>
        public bool ShowNullCheckbox
        {
            get => this.tableLayoutPanel1.ColumnStyles[3].Width > 0;
            protected set => this.tableLayoutPanel1.ColumnStyles[3].Width = value ? 27 : 0;
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
        /// Occurs when the field value changes by the user on the UI.
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

            //nullCheckbox.Enabled = !fieldInfo.IsReadOnly;
            //ShowNullCheckbox = fieldInfo.IsNullable;

            if (fieldInfo is ItemFieldInfo itemFieldInfo)
            {
                ShowRemoveButton = true;
                btnRemove.Enabled = itemFieldInfo.IsRemovable;
            }

            if (fieldInfo.IsReadOnly)
                label1.ForeColor = Color.Gray;

            if (fieldInfo.Description != null) // underline described fields
                label1.Font = new Font(label1.Font, FontStyle.Underline);

            Initialize();
            Value = value;
            UpdateName(); // set the name after the value is set
            UpdateTip();
        }
        #endregion

        #region Abstract Methods
        protected abstract void Initialize(); // Initialize the control, the value is not set yet.
        protected abstract object GetValue();
        protected abstract void SetValue(object value);
        #endregion

        #region Field Actions
        /// <summary>
        /// Sets the control to the field, which is used to view and edit the value.
        /// </summary>
        /// <param name="control"></param>
        protected void SetControl(Control control)
        {
            this.tableLayoutPanel1.Controls.Add(control, 1, 0);
        }

        /// <summary>
        /// Applies the changes made.
        /// </summary>
        public virtual void Apply()
        {
            Status = FieldStatus.Synced;
        }

        /// <summary>
        /// Resets the field value to an initial value.
        /// </summary>
        /// <param name="value">Initial value</param>
        public virtual void Reset(object value)
        {
            Value = value;
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
        /// Occurs when the field value changes by the user on the UI.
        /// </summary>
        /// <param name="value">The new value of the field.</param>
        protected void OnValueChanged(object value) => OnValueChanged(new FieldValueChangedEventArgs(this, value, !_settingValue));
        protected virtual void OnValueChanged(FieldValueChangedEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (!e.ByUser) return; // ignore the event if it's not by the user
            if (IsNull && e.Value != null) return; // when the base class desides the value is null, ignore changes

            bool isThis = e.Sender == this;

            if (!isThis) // add this field to the path before invoking the event.
                e.AddParentField(this);

            //BackColor = isThis ? Color.Orange : Color.Yellow;
            Status |= isThis ? FieldStatus.ValueChanged : FieldStatus.InnerValueChanged;
            ValueChanged?.Invoke(this, e);
        }

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
