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
        public static IEnumerable<PropertyInfo> GetPropertiesFiltered(this Type type, Type ignoreInherited = null)
        {
            IEnumerable<PropertyInfo> properties = type.GetProperties();

            IEnumerable<Type> typeInheritance = type.GetInheritanceChain();
            Type stopAtType = typeInheritance.FirstOrDefault(t => t.GetCustomAttribute<EditorIgnoreInheritedAttribute>(false) != null);
            if (stopAtType != null) // only include properties from the target type up to the first class with IgnoreInheritedPropertiesAttribute defined.
                properties = properties.Where(p => p.DeclaringType == stopAtType || p.DeclaringType != null && p.DeclaringType.IsSubclassOf(stopAtType));

            if (ignoreInherited != null)
                properties = properties.Where(p => p.DeclaringType.IsAssignableTo(ignoreInherited));

            //properties = properties.OrderBy(p => p.Name); // TODO: let the user choose how to sort
            return properties.Where(p => !p.IsIgnored() && !p.IsObsolete() && !p.IsIndexer()); // exclude ignored, obsolete and indexer properties
        }

        /// <summary>
        /// Get the permissions for each property based on the permission groups defined in the properties and the permissions defined for each group.
        /// </summary>
        /// <param name="properties">The properties to get the permissions of.</param>
        /// <param name="groupsPermissions">The permissions defined for each group.</param>
        /// <returns>A dictionary of properties and their permissions.</returns>
        public static Dictionary<PropertyInfo, Permissions> CheckPermissions(this IEnumerable<PropertyInfo> properties, IDictionary<string, Permissions> groupsPermissions)
        {
            var result = new Dictionary<PropertyInfo, Permissions>();
            foreach (var property in properties)
                result[property] = property.GetPermissions(groupsPermissions);
            return result;
        }

        /// <summary>
        /// Get the permissions for the property based on the group the property belongs to and the permissions defined for that group.
        /// </summary>
        /// <param name="property">The property to get the permissions of.</param>
        /// <param name="groupsPermissions">The permissions defined for each group.</param>
        /// <returns>The permissions for the property.</returns>
        public static Permissions GetPermissions(this PropertyInfo property, IDictionary<string, Permissions> groupsPermissions)
        {
            if (property == null)
                return Permissions.None;
            if (groupsPermissions == null)
                return Permissions.All; // no permissions defined, allow all by default

            var permissionGroup = property.GetPermissionGroupName();

            Permissions permissions;
            if (permissionGroup == null)
                permissions = Permissions.All; // no permission group defined for that property, allow all by default
            else if (!groupsPermissions.TryGetValue(permissionGroup, out permissions))
                permissions = Permissions.None; // the property permission group not found, deny all by default
            // else: the property permission group was found, use the permissions defined for that group

            return permissions;
        }

        /// <summary>
        /// Get the last defined permission group attribute of the property or its declaring type.
        /// </summary>
        /// <param name="property">The property to get the permission group attribute of.</param>
        /// <returns>The permission group attribute of the property or its declaring type.</returns>
        public static PermissionGroupAttribute GetPermissionGroupAttribute(this PropertyInfo property)
        {
            if (property == null)
                return null;

            // If class A has group attribute "A" and class B has group attribute "B" and B inherits from A, then B's properties will have group "B" and A's properties will have group "A".
            // If B has no group attribute, then B's properties will have group "A".
            // This is because the inheritance chain is ordered from the most derived class to the base class.
            // If a property has a group attribute, it will be used instead of the class group attribute anyway.
            // Both class and property group attributes are optional.

            var attribute = property.GetCustomAttribute<PermissionGroupAttribute>();
            if (attribute != null)
                return attribute;

            var typeChain = property.DeclaringType.GetInheritanceChain();
            foreach (var type in typeChain)
            {
                attribute = type.GetCustomAttribute<PermissionGroupAttribute>();
                if (attribute != null)
                    return attribute;
            }

            return null;
        }

        /// <summary>
        /// Get the permission group name of the property or its declaring type.
        /// </summary>
        /// <param name="property">The property to get the permission group name of.</param>
        /// <returns>The permission group name of the property or its declaring type.</returns>
        public static string GetPermissionGroupName(this PropertyInfo property)
            => property.GetPermissionGroupAttribute()?.GroupName;
    }
}
