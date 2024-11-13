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
    /// Extends the ObjectEditorController to view and edit an enumerable of any type.
    /// </summary>
    public class EnumerableEditorController : ObjectEditorController
    {
        /// <summary>
        /// A wrapper for the enumerable to view and edit (must be set in the constructor).
        /// </summary>
        protected DynamicEnumerableWrapper SourceEnumerableWrapper { get; }

        /// <summary>
        /// Gets the abilities of the enumerable on its items.
        /// </summary>
        public virtual ItemAbility ItemAbilities => ItemAbility.ReadOnly;

        protected IEnumerable<ValueFieldController> ItemFields => Fields.Where(f => f.FieldInfo is ItemFieldMetadata);

        #region Constructors
        /// <param name="sourceEnumerable">The enumerable to view and edit.</param>
        public EnumerableEditorController(IEnumerable sourceEnumerable) : this(sourceEnumerable, null) { }

        /// <param name="sourceEnumerable">The enumerable to view and edit.</param>
        /// <param name="enumerableWrapper">A wrapper for the enumerable to view and edit (optional).</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal EnumerableEditorController(IEnumerable sourceEnumerable, DynamicEnumerableWrapper enumerableWrapper)
            // pass the source object to the base constructor as an object, properties will be extracted from it
            : base(sourceEnumerable)
        {
            if (sourceEnumerable == null) throw new ArgumentNullException(nameof(sourceEnumerable));
            if (enumerableWrapper != null && enumerableWrapper.Source != sourceEnumerable)
                throw new ArgumentException("The dynamic wrapper must be for the same given source.", nameof(enumerableWrapper));

            SourceEnumerableWrapper = enumerableWrapper ?? new DynamicEnumerableWrapper(sourceEnumerable);
        }
        #endregion

        #region Actions
        /// <summary>
        /// Add a field for an item of an enumerable.
        /// </summary>
        /// <param name="fieldInfo">The field info for the item.</param>
        /// <param name="itemValue">The value of the item.</param>
        /// <returns>The new created field controller.</returns>
        protected virtual ValueFieldController AddItemField(ItemFieldMetadata fieldInfo, object itemValue)
        {
            var field = CreateFieldController(fieldInfo, itemValue);
            AddField(field);
            return field;
        }

        /// <summary>
        /// Create a field info for an item of an enumerable.
        /// </summary>
        /// <param name="itemValue">The value of the item.</param>
        /// <param name="index">The index of the item in the enumerable.</param>
        /// <returns>The new created field info.</returns>
        protected virtual ItemFieldMetadata CreateItemFieldInfo(object itemValue, int index)
        {
            // get the type of the item or the default type of the enumerable if null
            var fieldType = itemValue?.GetType() ?? SourceEnumerableWrapper.ItemType;
            return new ItemFieldMetadata(fieldType, null, null, index, ItemAbilities);
        }

        /// <summary>
        /// Clear the item field from this controller.
        /// </summary>
        protected virtual void ClearItemFields()
        {
            var itemFields = ItemFields.ToArray(); // copy the fields to prevent concurrent modification
            foreach (var field in itemFields)
                RemoveField(field);
        }

        /// <summary>
        /// Clear and reload the item fields from the source enumerable.
        /// </summary>
        protected virtual void ReloadItemFields()
        {
            ClearItemFields();
            SourceEnumerableWrapper.ForEachAll((p, i) => AddItemField(CreateItemFieldInfo(p, i), p));
        }
        #endregion

        #region Overrides
        public override void ReloadFields()
        {
            base.ReloadFields();
            ReloadItemFields();
        }

        protected override void ResetField(ValueFieldController fieldController)
        {
            if (fieldController.FieldInfo is ItemFieldMetadata)
                fieldController.Value = SourceEnumerableWrapper.GetAt(((ItemFieldMetadata)fieldController.FieldInfo).Index);
            else base.ResetField(fieldController); // fallback to the base method
        }
        #endregion
    }
}
