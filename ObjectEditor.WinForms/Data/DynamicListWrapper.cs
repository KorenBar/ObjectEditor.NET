using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ObjectEditor.Extensions;

namespace ObjectEditor.Data
{
    /// <summary>
    /// Reference any IList<T> as IList<object> to access its items as objects dynamically.
    /// </summary>
    public class DynamicListWrapper : DynamicCollectionWrapper //, IList<object> // TODO
    {
        /// <summary>
        /// The generic type definition of the source list.
        /// </summary>
        Type _genericTypeDefinition;

        /// <summary>
        /// Wraps a generic list as a list of objects.
        /// </summary>
        /// <param name="objects">The source list to wrap.</param>
        /// <exception cref="ArgumentException"></exception>
        public DynamicListWrapper(IEnumerable objects)
            : base(objects)
        {
            _genericTypeDefinition = objects.GetType().GetGenericType(typeof(IList<>));

            if (_genericTypeDefinition == null)
                throw new ArgumentException("The source enumerable must be a generic list.", nameof(objects));
        }

        // TODO: implement IList<object> members
    }
}
