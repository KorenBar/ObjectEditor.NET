using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.Demo.Data
{
    [EditorIgnoreInherited]
    public class BaseInheritingClass1 : ItemClass
    {
        public string TheOnlySingleProperty { get; set; }
    }

    public class BaseInheritingClass2 : BaseInheritingClass1
    {
        public bool ShouldNotBeSeen { get; set; }
    }

    [EditorIgnoreInherited]
    public class BaseInheritingClass3 : BaseInheritingClass2
    {
        public bool TheOnlySecondProperty { get; set; } // the only property that should be seen
    }

    public class InheritingClass : BaseInheritingClass3
    {
        // Only TheOnlySecondProperty should be seen
    }
}
