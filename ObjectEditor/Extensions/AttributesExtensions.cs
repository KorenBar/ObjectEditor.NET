using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ObjectEditor.Extensions;

namespace ObjectEditor
{
    internal static class AttributesExtensions
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

        /// <returns>True if the property is a password, false otherwise.</returns>
        public static bool IsPassword(this PropertyInfo property) => property.GetCustomAttribute<EditorPasswordAttribute>() != null;

        /// <returns>True if the property is ignored, false otherwise.</returns>
        public static bool IsIgnored(this PropertyInfo property) => property.GetCustomAttribute<EditorIgnoreAttribute>() != null;

        /// <returns>True if the property is obsolete, false otherwise.</returns>
        public static bool IsObsolete(this PropertyInfo property) => property.GetCustomAttribute<ObsoleteAttribute>() != null;

        /// <summary>
        /// Get the properties of the type, filtered out if the type has the EditorIgnoreInheritedAttribute defined or if the property is ignored, obsolete or an indexer.
        /// </summary>
        /// <param name="type">The type to get the properties of.</param>
        /// <returns>A filtered enumerable of properties of the type.</returns>
        public static IEnumerable<PropertyInfo> GetPropertiesFiltered(this Type type)
        {
            IEnumerable<PropertyInfo> properties = type.GetProperties();

            IEnumerable<Type> typeInheritance = type.GetInheritanceChain();
            Type stopAtType = typeInheritance.FirstOrDefault(t => t.GetCustomAttribute<EditorIgnoreInheritedAttribute>(false) != null);
            if (stopAtType != null) // only include properties from the target type up to the first class with IgnoreInheritedPropertiesAttribute defined.
                properties = properties.Where(p => p.DeclaringType == stopAtType || p.DeclaringType != null && p.DeclaringType.IsSubclassOf(stopAtType));

            //properties = properties.OrderBy(p => p.Name); // TODO: let the user choose how to sort
            return properties.Where(p => !p.IsIgnored() && !p.IsObsolete() && !p.IsIndexer()); // exclude ignored, obsolete and indexer properties
        }
    }
}
