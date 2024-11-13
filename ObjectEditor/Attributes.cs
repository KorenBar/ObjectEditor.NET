using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ObjectEditor
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InfoAttribute : Attribute
    {
        /// <summary>
        /// The name of the property field.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The description of the property field.
        /// </summary>
        public string Description { get; set; }

        public InfoAttribute() { }
        public InfoAttribute(string description) => Description = description;
    }

    /// <summary>
    /// Associates a property or class with a permission group.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public class PermissionGroupAttribute : Attribute
    {
        /// <summary>
        /// The name of the permission group.
        /// </summary>
        public string GroupName { get; }

        /// <param name="groupName">The name of the permission group. Set to null to remove the permission group.</param>
        public PermissionGroupAttribute(string groupName) => GroupName = groupName;
    }

    /// <summary>
    /// Attribute to use the property value as a display name of an item in a collection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EditorDisplayNameAttribute : Attribute { }

    /// <summary>
    /// Attribute to mask the property value in the object editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EditorPasswordAttribute : Attribute { }

    /// <summary>
    /// Attribute to ignore the inherited properties in the object editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class EditorIgnoreInheritedAttribute : Attribute { }

    /// <summary>
    /// Attribute to ignore the property in the object editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EditorIgnoreAttribute : Attribute { }
}
