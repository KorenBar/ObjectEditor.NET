using ObjectEditor.Controllers.Fields;
using ObjectEditor.Data;
using ObjectEditor.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.Controllers.Editors
{
    /// <summary>
    /// Extends the ObjectEditorController to view and edit a collection of items of any type.
    /// </summary>
    internal class CollectionEditorController : EnumerableEditorController
    {
        /// <summary>
        /// A wrapper for the collection to view and edit (must be set in the constructor).
        /// </summary>
        protected DynamicCollectionWrapper SourceCollectionWrapper => (DynamicCollectionWrapper)SourceEnumerableWrapper;

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// When false, items can be added or removed from the collection.
        /// </summary>
        public bool IsReadOnly => SourceCollectionWrapper.IsReadOnly;

        public override ItemAbility ItemAbilities
            => base.ItemAbilities | (IsReadOnly ? ItemAbility.ReadOnly : ItemAbility.Add | ItemAbility.Remove);

        #region Constructors
        /// <param name="sourceCollection">The collection to view and edit.</param>
        public CollectionEditorController(IEnumerable sourceCollection) : this(sourceCollection, null) { }

        /// <param name="sourceCollection">The collection to view and edit.</param>
        /// <param name="collectionWrapper">A wrapper for the collection to view and edit (optional).</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal CollectionEditorController(IEnumerable sourceCollection, DynamicCollectionWrapper collectionWrapper)
            : base(sourceCollection, collectionWrapper ?? new DynamicCollectionWrapper(sourceCollection))
        { }
        #endregion

        #region Actions
        /// <summary>
        /// Create a new item and add it to the collection.
        /// This method will not add a field controller for the new item.
        /// </summary>
        /// <returns>The new item created or null if failed.</returns>
        protected object CreateNewItem()
        {
            var item = SourceCollectionWrapper.AddDefaultValue();

            SaveRequired = true; // there are no pending changes, but the collection was modified
            return item;
        }

        /// <summary>
        /// Add a new item to the collection and create a field controller for it.
        /// </summary>
        public void AddNewItem()
        {
            // source object can't be changed after initialization, no need to copy it
            if (SourceCollectionWrapper == null || SourceObject == null)
                return;

            lock (SourceObject) // lock the source object to prevent concurrent modifications as possible
            {
                int index = SourceCollectionWrapper.Count;
                var itemValue = CreateNewItem(); // the item value can be null
                ItemFieldMetadata fieldInfo = CreateItemFieldInfo(itemValue, index);
                AddItemField(fieldInfo, itemValue);
            }
        }

        /// <summary>
        /// Remove an item from the source collection.
        /// </summary>
        /// <param name="fieldController">The field controller of the item to remove, must be an item field and not null.</param>
        protected virtual void RemoveItem(ValueFieldController fieldController)
        {
            var fieldInfo = fieldController.FieldInfo as ItemFieldMetadata;

            //SourceCollectionWrapper.RemoveAt(fieldInfo.Index); // remove the item from the collection
            var itemToRemove = SourceCollectionWrapper.GetAt(fieldInfo.Index);
            SourceCollectionWrapper.Remove(itemToRemove); // in case of duplicates, the first one will be removed!

            ReloadItemFields(); // Reload anyway to update the indexes
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Add a field for an item of a collection.
        /// </summary>
        /// <param name="fieldInfo">The field info for the item.</param>
        /// <param name="itemValue">The item to create a field for.</param>
        /// <returns>The new created field controller.</returns>
        protected override ValueFieldController AddItemField(ItemFieldMetadata fieldInfo, object itemValue)
        {
            var fieldController = base.AddItemField(fieldInfo, itemValue);
            if (fieldController == null) return null;
            fieldController.Removing += (s, e) =>
            {
                if (fieldController.FieldInfo is not ItemFieldMetadata) // check before
                    throw new InvalidOperationException("The field is not an item field.");
                RemoveItem(fieldController);
                SaveRequired = true;
            };
            return fieldController;
        }
        #endregion
    }
}
