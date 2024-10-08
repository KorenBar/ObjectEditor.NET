using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.Extensions
{
    public static class LinqExtensions
    {
        public static void Move<T>(this IList<T> list, int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || oldIndex >= list.Count)
                throw new ArgumentOutOfRangeException(nameof(oldIndex));

            if (newIndex < 0 || newIndex >= list.Count)
                throw new ArgumentOutOfRangeException(nameof(newIndex));

            var item = list[oldIndex];
            list.RemoveAt(oldIndex);
            list.Insert(newIndex, item);
        }

        public static void Move<T>(this IList<T> list, T item, int newIndex)
        {
            var oldIndex = list.IndexOf(item);
            list.Move(oldIndex, newIndex);
        }

        /// <summary>
        /// Perform an action on all values and collect exceptions.
        /// Iterates over all values even if an exception is thrown.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="values">The values to iterate over.</param>
        /// <param name="action">The action to perform on each value.</param>
        /// <exception cref="AggregateException">If one or more exceptions were thrown during the iterations.</exception>
        public static void ForEachAll<T>(this IEnumerable<T> values, Action<T> action)
            => values.ForEachAll((v, i) => action(v));

        /// <summary>
        /// Perform an action on all values and collect exceptions.
        /// Iterates over all values even if an exception is thrown.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="values">The values to iterate over.</param>
        /// <param name="action">The action to perform on each value and its index.</param>
        /// <exception cref="AggregateException">If one or more exceptions were thrown during the iterations.</exception>
        public static void ForEachAll<T>(this IEnumerable<T> values, Action<T, int> action)
        {
            // collect exceptions and throw them all at once
            List<Exception> exceptions = new List<Exception>();

            var valuesList = values.ToList(); // copy once to avoid multiple enumerations and index out of range exceptions
            for (int i = 0; i < valuesList.Count; i++)
            {
                try { action(valuesList[i], i); }
                catch (AggregateException ex) { exceptions.AddRange(ex.InnerExceptions); }
                catch (Exception ex) { exceptions.Add(ex); }
            }

            if (exceptions.Count > 0)
                throw new AggregateException(exceptions);
        }
    }
}
