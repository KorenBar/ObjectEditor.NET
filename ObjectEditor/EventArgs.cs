using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;
using TechnosoCommons.Configuration.UI.Controls;

namespace TechnosoCommons.Configuration
{
    public class FieldValueChangedEventArgs : EventArgs
    {
        // A parent field can only be an object, since a value field has no properties
        private List<BaseFieldControl> _fieldPath = new();
        internal IEnumerable<BaseFieldControl> FieldPath => _fieldPath.AsReadOnly();
        /// <summary>
        /// The first sender who has changed
        /// </summary>
        internal BaseFieldControl Sender { get; }
        public object Value { get; }
        public bool ByUser { get; }

        internal FieldValueChangedEventArgs(BaseFieldControl sender, object value, bool byUser = false)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            Sender = sender;
            Value = value;
            ByUser = byUser;
        }

        internal void AddParentField(BaseFieldControl field) => _fieldPath.Insert(0, field);

        /// <summary>
        /// Returns the path of the field that has changed.
        /// </summary>
        /// <returns>A string representing the path of the field that has changed.</returns>
        public string GetPath(string separator = ".")
        {
            StringBuilder sb = new();
            foreach (var field in _fieldPath)
                sb.Append($"{field?.Text}{separator}");
            sb.Append($"{Sender.Text}");
            return sb.ToString();
        }

        public override string ToString() => $"{GetPath()}" + (Value != null ? $" = {Value}" : " is null");
    }

    public class SaveRequiredChangedEventArgs : EventArgs
    {
        public bool SaveRequired { get; }
        public SaveRequiredChangedEventArgs(bool saveRequired) => SaveRequired = saveRequired;
    }

    public class ChangesPendingChangedEventArgs : EventArgs
    {
        public bool ChangesPending { get; }
        public ChangesPendingChangedEventArgs(bool changesPending) => ChangesPending = changesPending;
    }

    public class ChangesAppliedEventArgs : EventArgs { }
}
