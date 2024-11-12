using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using ObjectEditor.Controllers.Fields;

namespace ObjectEditor
{
    /// <summary>
    /// Event arguments for when a value has changed.
    /// </summary>
    /// <typeparam name="T">The type of the value that has changed.</typeparam>
    public class ValueChangedEventArgs<T> : EventArgs
    {
        public T OldValue { get; }
        public T NewValue { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ValueChangedEventArgs{T}"/>.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public ValueChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    /// <summary>
    /// Event arguments for when a field has changed.
    /// </summary>
    public class FieldValueChangedEventArgs : ValueChangedEventArgs<object>
    {
        // A parent field can only be an object, since a value field has no properties
        private readonly List<ValueFieldController> _fieldPath = new();
        internal IEnumerable<ValueFieldController> FieldPath => _fieldPath.AsReadOnly();
        /// <summary>
        /// The first sender who has changed
        /// </summary>
        internal ValueFieldController Sender { get; }
        public bool ByUser { get; }

        internal FieldValueChangedEventArgs(ValueFieldController sender, object oldValue, object newValue, bool byUser = false)
            : base(oldValue, newValue)
        {
            Sender = sender ?? throw new ArgumentNullException(nameof(sender));
            ByUser = byUser;
        }

        internal void AddParentField(ValueFieldController field) => _fieldPath.Insert(0, field);

        /// <summary>
        /// Returns the path of the field that has changed.
        /// </summary>
        /// <returns>A string representing the path of the field that has changed.</returns>
        public string GetPath(string separator = ".")
        {
            StringBuilder sb = new();
            foreach (var field in _fieldPath)
                sb.Append($"{field?.Name}{separator}");
            sb.Append($"{Sender.Name}");
            return sb.ToString();
        }

        public override string ToString() => $"{GetPath()}" + (NewValue != null ? $" = {NewValue}" : " is null");
    }

    public class SaveRequiredChangedEventArgs : EventArgs
    {
        public bool SaveRequired { get; }
        public bool Saveable { get; set; }
        public SaveRequiredChangedEventArgs(bool saveRequired) => SaveRequired = saveRequired;
    }

    public class ChangesPendingChangedEventArgs : EventArgs
    {
        public bool ChangesPending { get; }
        public ChangesPendingChangedEventArgs(bool changesPending) => ChangesPending = changesPending;
    }

    public class ChangesAppliedEventArgs : EventArgs { }

    public class FieldEventArgs : EventArgs
    {
        public ValueFieldController Field { get; }
        public FieldEventArgs(ValueFieldController field) => Field = field;
    }
}
