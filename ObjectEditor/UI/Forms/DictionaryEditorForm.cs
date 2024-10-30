using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using ObjectEditor;
using ObjectEditor.UI.Controls;
using ObjectEditor.UI.Forms;
using ObjectEditor.Data;
using ObjectEditor.Extensions;

namespace ObjectEditor.UI.Forms
{
    /// <summary>
    /// Extends the ObjectEditorForm to view and edit a collection of items of any type.
    /// </summary>
    public partial class DictionaryEditorForm : CollectionEditorForm
    {
        protected static readonly IEnumerable DEMO_DICT_SOURCE = new Dictionary<string, int>() { { "1", 1 }, { "2", 2 } };

        /// <summary>
        /// A wrapper for the dictionary to view and edit (must be set in the constructor).
        /// </summary>
        protected DynamicDictionaryWrapper SourceDictionaryWrapper => (DynamicDictionaryWrapper)SourceCollectionWrapper;

        public override ItemAbility ItemAbilities
            => base.ItemAbilities | (IsReadOnly ? ItemAbility.ReadOnly : ItemAbility.Edit);

        #region Constructors
        public DictionaryEditorForm() : this(DEMO_DICT_SOURCE) { }

        /// <param name="sourceDictionary">The dictionary to view and edit.</param>
        public DictionaryEditorForm(IEnumerable sourceDictionary) : this(sourceDictionary, null) { }

        /// <param name="sourceDictionary">The dictionary to view and edit.</param>
        /// <param name="parent">Parent form to inherit settings from (optional).</param>
        internal DictionaryEditorForm(IEnumerable sourceDictionary, ObjectEditorForm parent)
            : this(sourceDictionary, null, parent) { }

        /// <param name="sourceDictionary">The dictionary to view and edit.</param>
        /// <param name="dictionaryWrapper">A wrapper for the dictionary to view and edit (optional).</param>
        /// <param name="parent">Parent form to inherit settings from (optional).</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal DictionaryEditorForm(IEnumerable sourceDictionary, DynamicDictionaryWrapper dictionaryWrapper, ObjectEditorForm parent)
            : base(sourceDictionary, dictionaryWrapper ?? new DynamicDictionaryWrapper(sourceDictionary), parent)
        {
            InitializeComponent();
        }
        #endregion

        protected override void ResetField(BaseFieldControl fieldControl)
        {
            if (fieldControl is not KeyValuePairFieldControl kvpFieldControl)
            { // fallback to default behavior
                base.ResetField(fieldControl);
                return;
            }

            var sourceKey = kvpFieldControl.SourceKeyValuePair?.Key;
            if (sourceKey == null)
                return; // it's a new item, no need to reset

            if (SourceDictionaryWrapper.TryGetValue(sourceKey, out var sourceValue))
                kvpFieldControl.Value = new KeyValuePair<object, object>(sourceKey, sourceValue);
            // else: the key was removed, no need to reset (will be re-added if applied)
        }

        protected override void ApplyField(BaseFieldControl fieldControl)
        {
            if (fieldControl is not KeyValuePairFieldControl kvpFieldControl)
            { // fallback to default behavior
                base.ApplyField(fieldControl);
                return;
            }
            
            var kvpToRemove = kvpFieldControl.SourceKeyValuePair;
            var kvpToAdd = kvpFieldControl.KeyValuePair;

            // remove the old key-value pair
            if (kvpToRemove.HasValue)
                SourceDictionaryWrapper.Remove(kvpToRemove.Value.Key); // shouldn't throw if the key is not found

            // add the new key-value pair using the indexer, if the key is new it will be added
            if (kvpToAdd.HasValue)
                SourceDictionaryWrapper[kvpToAdd.Value.Key] = kvpToAdd.Value.Value;
        }

        protected override void RemoveItem(BaseFieldControl fieldControl)
        {
            if (fieldControl is not KeyValuePairFieldControl kvpFieldControl)
            { // fallback to default behavior
                base.RemoveItem(fieldControl);
                return;
            }

            var kvp = kvpFieldControl.SourceKeyValuePair;
            if (kvp.HasValue)
                SourceDictionaryWrapper.Remove(kvp.Value.Key);
        }
    }
}
