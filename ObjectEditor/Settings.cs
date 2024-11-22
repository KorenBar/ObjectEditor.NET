using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor
{
    /// <summary>
    /// Settings for the ObjectEditor.
    /// </summary>
    public interface IObjectEditorSettings
    {
        // Suggestions for settings to implement:
        //bool ExcludeReadOnlyFields { get; set; }
        //bool ExcludeStaticFields { get; set; }
        //bool ExcludeReferenceTypes { get; set; }

        /// <summary>
        /// The permissions defined for each group.
        /// If null (default), the permissions are not used and all fields are accessible.
        /// If empty, fields with permission group defined are excluded.
        /// Fields with no permission group defined are accessible anyway.
        /// </summary>
        public IDictionary<string, Permissions> GroupsPermissions { get; }
    }

    public class ObjectEditorSettings : IObjectEditorSettings
    {
        public IDictionary<string, Permissions> GroupsPermissions { get; set; }
    }
}
