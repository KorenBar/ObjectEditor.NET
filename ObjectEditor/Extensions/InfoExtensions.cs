using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ObjectEditor
{
    public static class InfoExtensions
    {
        public static InfoAttribute GetInfo(this PropertyInfo property) => property.GetCustomAttribute<InfoAttribute>();

        /// <summary>
        /// Get the display name of the object using the properties marked with the InfoAttribute.UseAsDisplayName attribute.
        /// </summary>
        /// <param name="item">The object to get the display name of.</param>
        /// <returns>The display name of the object as a '_' separated string.</returns>
        public static string GetDisplayName(this object item)
        {
            if (item == null) return null;
            var displayProperties = item.GetType().GetProperties().Where(p => p.CanRead && (p.GetCustomAttribute<EditorDisplayNameAttribute>() != null));
            if (displayProperties.Count() > 0)
                return string.Join("_", displayProperties.Select(p => p.GetValue(item)));
            return null;
        }
    }
}
