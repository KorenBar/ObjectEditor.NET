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

        // TODO: Override the remove item method to remove by key.
    }
}
