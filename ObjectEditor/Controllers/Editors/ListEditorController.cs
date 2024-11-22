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
    /// Extends the CollectionEditorController to view and edit a list of items of any type.
    /// </summary>
    public class ListEditorController : CollectionEditorController
    {
        // TODO: after implementing list-specific behavior, override ItemAbilities property to allow editing items if the list is not read-only.
        //public override ItemAbility ItemAbilities
        //    => base.ItemAbilities | (IsReadOnly ? ItemAbility.ReadOnly : ItemAbility.Edit);

        #region Constructors
        /// <param name="sourceList">The collection to view and edit.</param>
        /// <param name="listWrapper">A wrapper for the collection to view and edit (optional).</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ListEditorController(IEnumerable sourceList, DynamicCollectionWrapper listWrapper)
            : this(sourceList, listWrapper, null) { }

        /// <param name="sourceList">The collection to view and edit.</param>
        /// <param name="listWrapper">A wrapper for the collection to view and edit (optional).</param>
        /// <param name="settings">The settings for the editor.</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal ListEditorController(IEnumerable sourceList, DynamicCollectionWrapper listWrapper, IObjectEditorSettings settings)
            : base(sourceList, listWrapper ?? new DynamicCollectionWrapper(sourceList), settings)
        { }
        #endregion

        protected override void ApplyField(ValueFieldController fieldController)
        {
            base.ApplyField(fieldController);
            // TODO: implement list-specific behavior instead of calling base
        }
    }
}
