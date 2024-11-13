using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ObjectEditor.Extensions;

namespace ObjectEditor.Data
{
    /// <summary>
    /// Reference any IEnumerable or ICollection<T> as ICollection<object> to access its elements as objects dynamically.
    /// </summary>
    public class DynamicCollectionWrapper : DynamicEnumerableWrapper, ICollection<object>
    {
        Type _genericCollectionType;

        #region Dynamic Members Info
        PropertyInfo CountProperty => _genericCollectionType?.GetProperty("Count");
        PropertyInfo IsReadOnlyProperty => _genericCollectionType?.GetProperty("IsReadOnly");

        MethodInfo AddMethod => _genericCollectionType?.GetMethod("Add");
        MethodInfo ClearMethod => _genericCollectionType?.GetMethod("Clear");
        MethodInfo ContainsMethod => _genericCollectionType?.GetMethod("Contains");
        MethodInfo CopyToMethod => _genericCollectionType?.GetMethod("CopyTo");
        MethodInfo RemoveMethod => _genericCollectionType?.GetMethod("Remove");
        #endregion

        /// <summary>
        /// Wraps a generic collection as a collection of objects.
        /// </summary>
        /// <param name="objects">The source collection to wrap.</param>
        /// <exception cref="ArgumentException"></exception>
        public DynamicCollectionWrapper(IEnumerable objects) : base(objects)
        {
            Type srcType = objects.GetType();
            _genericCollectionType = srcType.GetGenericType(typeof(ICollection<>)); // null if it's not a generic collection

            if (_genericCollectionType == null)
                throw new ArgumentException("The source collection must be a generic collection.", nameof(objects));
        }

        /// <summary>
        /// Adds a new item to the collection with the default value of the item type.
        /// </summary>
        /// <returns>The new default added item.</returns>
        public virtual object AddDefaultValue()
        {
            var value = ItemType?.GetDefaultValue();
            Add(value);
            return value;
        }

        #region ICollection<object> implementation
        /// <summary>
        /// Gets the number of elements contained in the generic collection.
        /// Falls back to LINQ Count if it's an IEnumerable but not a generic ICollection<>.
        /// </summary>
        public int Count => (CountProperty?.GetValue(Source) as int?) ?? Source.Cast<object>().Count();
        public bool IsReadOnly => IsReadOnlyProperty?.GetValue(Source) as bool? == true;

        public void Add(object item) => AddMethod?.Invoke(Source, new object[] { item });
        public bool Remove(object item) => (RemoveMethod?.Invoke(Source, new object[] { item }) as bool?) ?? false;
        public void Clear() => ClearMethod?.Invoke(Source, null);

        public override bool Contains(object item)
        {
            if (ContainsMethod is MethodInfo contains)
                return contains.Invoke(Source, new object[] { item }) as bool? ?? false;
            return base.Contains(item); // fallback
        }
        //public override void CopyTo(object[] array, int arrayIndex)
        //{
        //    if (CopyToMethod is MethodInfo copyTo)
        //        // ArgumentException: Object of type object[] cannot be converted to type T[].
        //        copyTo.Invoke(Source, new object[] { array, arrayIndex });
        //    else base.CopyTo(array, arrayIndex); // fallback
        //}
        #endregion
    }
}
