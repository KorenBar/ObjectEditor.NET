using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using TechnosoCommons;
using TechnosoCommons.Configuration.UI.Controls;
using TechnosoCommons.Configuration.UI.Forms;
using TechnosoCommons.Data;
using TechnosoCommons.Extensions;
using TechnosoUI.Configuration;
using TechnosoUI.Configuration.UI;

namespace TechnosoCommons.Configuration.UI.Forms
{
    /// <summary>
    /// Extends the ObjectEditorForm to view and edit an enumerable of any type.
    /// </summary>
    public partial class EnumerableEditorForm : ObjectEditorForm
    {
        // A demo source for the designer.
        protected static readonly IEnumerable DEMO_SOURCE = new List<object> { 1, 2, new int[] { 3, 4 }, null };

        /// <summary>
        /// A wrapper for the enumerable to view and edit (must be set in the constructor).
        /// </summary>
        protected DynamicEnumerableWrapper SourceEnumerableWrapper { get; }

        ///// <summary>
        ///// Gets a value indicating whether the source is read-only and can't be edited.
        ///// </summary>
        //public virtual bool IsReadOnly => true;

        /// <summary>
        /// Gets the abilities of the enumerable on its items.
        /// </summary>
        public virtual ItemAbility ItemAbilities => ItemAbility.ReadOnly;

        protected IEnumerable<BaseFieldControl> ItemFieldControls => FieldControls.Where(f => f.FieldInfo is ItemFieldInfo);

        #region Constructors
        /// <summary>
        /// Default constructor for the designer.
        /// </summary>
        public EnumerableEditorForm() : this(DEMO_SOURCE) { }

        /// <param name="sourceEnumerable">The enumerable to view and edit.</param>
        public EnumerableEditorForm(IEnumerable sourceEnumerable) : this(sourceEnumerable, null) { }

        /// <param name="sourceEnumerable">The enumerable to view and edit.</param>
        /// <param name="parent">Parent form to inherit settings from (optional).</param>
        internal EnumerableEditorForm(IEnumerable sourceEnumerable, ObjectEditorForm parent)
            : this(sourceEnumerable, null, parent) { }

        /// <param name="sourceEnumerable">The enumerable to view and edit.</param>
        /// <param name="enumerableWrapper">A wrapper for the enumerable to view and edit (optional).</param>
        /// <param name="parent">Parent form to inherit settings from (optional).</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal EnumerableEditorForm(IEnumerable sourceEnumerable, DynamicEnumerableWrapper enumerableWrapper, ObjectEditorForm parent)
            // pass the source object to the base constructor as an object, properties will be extracted from it
            : base(sourceEnumerable, parent)
        {
            if (sourceEnumerable == null) throw new ArgumentNullException(nameof(sourceEnumerable));
            if (enumerableWrapper != null && enumerableWrapper.Source != sourceEnumerable)
                throw new ArgumentException("The dynamic wrapper must be for the same given source.", nameof(enumerableWrapper));

            InitializeComponent();

            SourceEnumerableWrapper = enumerableWrapper ?? new DynamicEnumerableWrapper(sourceEnumerable);
        }
        #endregion

        #region Form Actions
        /// <summary>
        /// Add a field for an item of an enumerable.
        /// </summary>
        /// <param name="itemValue">The item to create a field for.</param>
        /// <param name="index">The index of the item in the enumerable.</param>
        protected virtual BaseFieldControl AddItemField(object itemValue, int index)
        {
            var fieldType = itemValue?.GetType() ?? SourceEnumerableWrapper.ItemType;
            var fieldInfo = new ItemFieldInfo(fieldType, null, null, index, ItemAbilities);
            var field = CreateFieldControl(fieldInfo, itemValue);
            AddField(field);
            return field;
        }

        /// <summary>
        /// Clear the item field controls from the form.
        /// </summary>
        protected virtual void ClearItemFields()
        {
            ItemFieldControls.ForEachAll(f => f.Dispose());
        }

        /// <summary>
        /// Clear and reload the item field controls from the source enumerable.
        /// </summary>
        protected virtual void ReloadItemControls()
        {
            ClearItemFields();
            SourceEnumerableWrapper.ForEachAll((p, i) => AddItemField(p, i));
        }
        #endregion

        #region Overrides
        public override void ReloadControls()
        {
            base.ReloadControls();
            ReloadItemControls();
        }

        //public override void ApplyChanges()
        //{
        //    // TODO: parse the items as well
        //    // if there is a mismatch between the items and the fields, show a message box to reload the form and lose the changes

        //    FieldControls.Where(f => f.FieldInfo is ItemFieldInfo).ForEachAll(f => {
        //        // Actually, it's not possible to set the items directly to the source here.
        //        // TODO: create a ListEditorForm class to edit a list items directly (support dictionary too, with a new KeyValuePairFieldControl class).
        //        //SourceEnumerableWrapper[((ItemFieldInfo)f.FieldInfo).Index] = f.Value;
        //        f.Apply();
        //    });
        //    base.ApplyChanges(); // end with the base method, so the event will be raised at the end
        //}

        /// <summary>
        /// Reload values from the source object to the existing item fields.
        /// </summary>
        protected virtual void ResetItems()
        {
            // the field control type is by the item type, what if the item at the same index was changed to another type?
            // TODO: detect the type change and reload the item field controls if needed.
            FieldControls.Where(f => f.FieldInfo is ItemFieldInfo).ForEachAll(f => {
                f.Value = SourceEnumerableWrapper.GetAt(((ItemFieldInfo)f.FieldInfo).Index);
            });
        }
        public override void Reset()
        {
            ResetItems(); // reset the items as well
            base.Reset(); // end with the base method, so the event will be raised at the end
        }
        #endregion
    }
}
