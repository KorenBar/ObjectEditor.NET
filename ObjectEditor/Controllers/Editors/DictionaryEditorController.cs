using ObjectEditor.Controllers.Fields;
using ObjectEditor.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.Controllers.Editors
{
    /// <summary>
    /// Extends the CollectionEditorController to view and edit a collection of items of any type.
    /// </summary>
    public class DictionaryEditorController : CollectionEditorController
    {
        /// <summary>
        /// A wrapper for the dictionary to view and edit (must be set in the constructor).
        /// </summary>
        protected DynamicDictionaryWrapper SourceDictionaryWrapper => (DynamicDictionaryWrapper)SourceCollectionWrapper;

        public override ItemAbility ItemAbilities
            => base.ItemAbilities | (IsReadOnly ? ItemAbility.ReadOnly : ItemAbility.Edit);

        #region Constructors
        /// <param name="sourceDictionary">The dictionary to view and edit.</param>
        public DictionaryEditorController(IEnumerable sourceDictionary) : this(sourceDictionary, null) { }

        /// <param name="sourceDictionary">The dictionary to view and edit.</param>
        /// <param name="dictionaryWrapper">A wrapper for the dictionary to view and edit (optional).</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal DictionaryEditorController(IEnumerable sourceDictionary, DynamicDictionaryWrapper dictionaryWrapper)
            : base(sourceDictionary, dictionaryWrapper ?? new DynamicDictionaryWrapper(sourceDictionary))
        { }
        #endregion

        protected override void ResetField(ValueFieldController fieldController)
        {
            if (fieldController is not KeyValuePairFieldController kvpFieldController)
            { // fallback to default behavior
                base.ResetField(fieldController);
                return;
            }

            var sourceKey = kvpFieldController.SourceKeyValuePair?.Key;
            if (sourceKey == null)
                return; // it's a new item, no need to reset

            if (SourceDictionaryWrapper.TryGetValue(sourceKey, out var sourceValue))
                kvpFieldController.Value = new KeyValuePair<object, object>(sourceKey, sourceValue);
            // else: the key was removed, no need to reset (will be re-added if applied)
        }

        protected override void ApplyField(ValueFieldController fieldController)
        {
            if (fieldController is not KeyValuePairFieldController kvpFieldController)
            { // fallback to default behavior
                base.ApplyField(fieldController);
                return;
            }

            var kvpToRemove = kvpFieldController.SourceKeyValuePair;
            var kvpToAdd = kvpFieldController.KeyValuePair;

            // remove the old key-value pair
            if (kvpToRemove.HasValue)
                SourceDictionaryWrapper.Remove(kvpToRemove.Value.Key); // shouldn't throw if the key is not found

            // add the new key-value pair using the indexer, if the key is new it will be added
            if (kvpToAdd.HasValue)
                SourceDictionaryWrapper[kvpToAdd.Value.Key] = kvpToAdd.Value.Value;
        }

        protected override void RemoveItem(ValueFieldController fieldController)
        {
            if (fieldController is not KeyValuePairFieldController kvpFieldController)
            { // fallback to default behavior
                base.RemoveItem(fieldController);
                return;
            }

            var kvp = kvpFieldController.SourceKeyValuePair;
            if (kvp.HasValue)
                SourceDictionaryWrapper.Remove(kvp.Value.Key);
        }
    }
}
