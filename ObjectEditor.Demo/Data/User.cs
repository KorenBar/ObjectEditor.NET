using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.Demo.Data
{
    [PermissionGroup("Manager")]
    public class User
    {
        [EditorDisplayName]
        public string Name { get; set; }
        [EditorDisplayName]
        public int Id { get; set; }
        public string Email { get; set; }
        [PermissionGroup("Admin")] // only admins can see this field, managers can't
        public string Password { get; set; }

        [EditorIgnore] // this field is not shown in the editor
        public Dictionary<string, Permissions> Permissions { get; set; }

        [PermissionGroup("Admin")]
        public IEnumerable<KeyValuePair<string, Permissions>> PermissionsToReadOnly
            => Permissions.Select(p => p); // as enumerable of key-value pairs (read-only)
    }
}
