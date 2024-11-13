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
    /// Reference any IEnumerable as IEnumerable<object> to access its elements as objects dynamically.
    /// </summary>
    public class DynamicEnumerableWrapper : IEnumerable<object>
    {
        /// <summary>
        /// The source wrapped enumerable.
        /// </summary>
        public IEnumerable Source { get; }
        /// <summary>
        /// The generic argument type of the source enumerable items.
        /// </summary>
        public Type ItemType { get; }

        /// <summary>
        /// Wraps an enumerable as an enumerable of objects.
        /// </summary>
        /// <param name="objects">The source enumerable to wrap.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DynamicEnumerableWrapper(IEnumerable objects)
        {
            if (objects == null)
                throw new ArgumentNullException(nameof(objects));
            Source = objects;
            ItemType = objects.GetType().GetGenericType(typeof(IEnumerable<>))?.GetGenericArguments()?[0] ?? typeof(object);
        }

        public virtual bool Contains(object item)
        {
            foreach (var obj in Source)
                if (obj == item)
                    return true;
            return false;
        }

        public virtual void CopyTo(object[] array, int arrayIndex)
        {
            foreach (var obj in Source)
                array[arrayIndex++] = obj;
        }

        public IEnumerator<object> GetEnumerator()
        {
            foreach (var obj in Source)
                yield return obj;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
