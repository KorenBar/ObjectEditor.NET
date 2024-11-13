using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectEditor.Extensions;

namespace ObjectEditor.Data
{
    public static class DynamicWrapperExtensions
    {
        /// <summary>
        /// Wraps any IEnumerable as IEnumerable<object> to access its elements as objects dynamically.
        /// </summary>
        public static DynamicEnumerableWrapper AsDynamic(this IEnumerable objects)
        {
            var type = objects.GetType();
            if (type.GetGenericType(typeof(IDictionary<,>)) != null)
                return new DynamicDictionaryWrapper(objects);
            if (type.GetGenericType(typeof(IList<>)) != null)
                return new DynamicListWrapper(objects);
            if (type.GetGenericType(typeof(ICollection<>)) != null)
                return new DynamicCollectionWrapper(objects);
            return new DynamicEnumerableWrapper(objects);
        }

        /// <summary>
        /// Gets the item at the specified index in the enumerable.
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static object GetAt(this IEnumerable objects, int index)
        {
            var enumerator = objects.GetEnumerator();
            for (int i = 0; i <= index; i++)
                enumerator.MoveNext();
            return enumerator.Current;
        }

        ///// <summary>
        ///// Removes the item at the specified index in the collection.
        ///// </summary>
        ///// <typeparam name="T">The type of the collection items.</typeparam>
        ///// <param name="values">The collection to remove from.</param>
        ///// <param name="index">The index of the item to remove.</param>
        ///// <exception cref="ArgumentOutOfRangeException"></exception>
        //public static void RemoveAt<T>(this ICollection<T> values, int index)
        //{
        //    if (values is IList<T> list)
        //    {
        //        list.RemoveAt(index);
        //        return;
        //    }

        //    var enumerator = values.GetEnumerator();
        //    for (int i = 0; i <= index; i++)
        //        if (!enumerator.MoveNext()) // move to i
        //            throw new ArgumentOutOfRangeException(nameof(index), index, "The index is out of range.");

        //    values.Remove(enumerator.Current);
        //}
    }
}
