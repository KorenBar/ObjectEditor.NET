using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ObjectEditor
{
    public class InfoAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Hide the text from the user
        /// </summary>
        public bool IsPassword { get; set; }
        /// <summary>
        /// Use the property's value as a display name of an item in collection.
        /// </summary>
        public bool UseAsDisplayName { get; set; }

        public InfoAttribute() { }
        public InfoAttribute(string description) => Description = description;
    }

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
