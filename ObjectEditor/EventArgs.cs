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
        public string GetPath()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var field in _fieldPath)
                sb.Append($"{field?.Text}.");
            sb.Append($"{Sender.Text} = {Value}");
            return sb.ToString();
        }
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
