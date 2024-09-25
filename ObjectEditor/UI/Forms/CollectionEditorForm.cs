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
    /// Extends the ObjectEditorForm to view and edit a collection of items of any type.
    /// </summary>
    public partial class CollectionEditorForm : EnumerableEditorForm
    {
        /// <summary>
        /// A wrapper for the collection to view and edit (must be set in the constructor).
        /// </summary>
        protected DynamicCollectionWrapper SourceCollectionWrapper => (DynamicCollectionWrapper)SourceEnumerableWrapper;

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// When false, items can be added or removed from the collection.
        /// </summary>
        public bool IsReadOnlyCollection => SourceCollectionWrapper.IsReadOnly;

        public override ItemAbility ItemAbilities
            => base.ItemAbilities | (IsReadOnlyCollection ? ItemAbility.ReadOnly : ItemAbility.Add | ItemAbility.Remove);

        #region Constructors
        public CollectionEditorForm() : this(DEMO_SOURCE) { }

        /// <param name="sourceCollection">The collection to view and edit.</param>
        public CollectionEditorForm(IEnumerable sourceCollection) : this(sourceCollection, null) { }

        /// <param name="sourceCollection">The collection to view and edit.</param>
        /// <param name="parent">Parent form to inherit settings from (optional).</param>
        internal CollectionEditorForm(IEnumerable sourceCollection, ObjectEditorForm parent)
            : this(sourceCollection, null, parent) { }

        /// <param name="sourceCollection">The collection to view and edit.</param>
        /// <param name="collectionWrapper">A wrapper for the collection to view and edit (optional).</param>
        /// <param name="parent">Parent form to inherit settings from (optional).</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal CollectionEditorForm(IEnumerable sourceCollection, DynamicCollectionWrapper collectionWrapper, ObjectEditorForm parent)
            : base(sourceCollection, collectionWrapper ?? new DynamicCollectionWrapper(sourceCollection), parent)
        {
            InitializeComponent();
            ShowCustomPanel = true;
            btnAdd.Enabled = !IsReadOnlyCollection; // Enable the Add button if the collection is editable
        }
        #endregion

        #region Form Actions
        /// <summary>
        /// Create a new item and add it to the collection.
        /// This method will not add a field control for the new item.
        /// </summary>
        /// <returns>The new item created or null if failed.</returns>
        protected object CreateNewItem()
        {
            var item = SourceCollectionWrapper.AddDefaultValue();

            this.SaveRequired = true; // there are no pending changes, but the collection was modified
            return item;
        }

        /// <summary>
        /// Add a new item to the collection and create a field control for it.
        /// </summary>
        /// <returns></returns>
        protected Task AddNewItemAsync()
        {
            // source object can't be changed after initialization, no need to copy it
            if (SourceCollectionWrapper == null || SourceObject == null)
                return Task.CompletedTask;
            return Task.Run(() => {
                lock (SourceObject) // lock the source object to prevent concurrent modifications as possible
                {
                    int index = SourceCollectionWrapper.Count;
                    var item = CreateNewItem(); // the item value can be null
                    AddItemField(item, index);
                }
            });
        }

        /// <summary>
        /// Remove an item from the source collection.
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected virtual void RemoveItem(ItemFieldInfo fieldInfo)
        {
            if (fieldInfo == null) throw new ArgumentNullException(nameof(fieldInfo));

            //SourceCollectionWrapper.RemoveAt(fieldInfo.Index); // remove the item from the collection
            var itemToRemove = SourceCollectionWrapper.GetAt(fieldInfo.Index);
            SourceCollectionWrapper.Remove(itemToRemove); // in case of duplicates, the first one will be removed!

            ReloadItemControls(); // Reload anyway to update the indexes

            this.SaveRequired = true;
        }
        #endregion

        #region UI Events
        private async void Add_Click(object sender, EventArgs e)
        {
            if (IsReadOnlyCollection)
            {   // Can't edit
                MessageBox.Show($"It's a read only collection.", "Can't add an item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try { await AddNewItemAsync(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Failed to add a new item", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            ScrollDown();
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Add a field for an item of a collection.
        /// </summary>
        /// <param name="itemValue">The item to create a field for.</param>
        /// <param name="index">The index of the item in the collection.</param>
        protected override BaseFieldControl AddItemField(object itemValue, int index)
        {
            var fieldControl = base.AddItemField(itemValue, index);
            fieldControl.Removing += (s, e) => RemoveItem(((BaseFieldControl)s)?.FieldInfo as ItemFieldInfo);
            return fieldControl;
        }
        #endregion
    }
}
