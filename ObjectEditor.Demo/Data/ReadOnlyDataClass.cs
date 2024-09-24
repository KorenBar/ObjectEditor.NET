using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.Demo.Data
{
    /// <summary>
    /// A class that contains properties of any type, value, reference, collection, list, dictionary, nullable, etc.
    /// </summary>
    public class ReadOnlyDataClass
    {
        public int Int { get; } = 1;
        public string String { get; } = "string";
        public DateTime DateTime { get; } = System.DateTime.Now;
        public List<int> IntList{ get; } = new List<int>() { 1, 2, 3 };
        public Dictionary<string, int> StringIntDictionary{ get; } = new Dictionary<string, int>() { { "one", 1 }, { "two", 2 }, { "three", 3 } };
        public CustomList CustomList { get; set; } = new CustomList(true) { new object(), System.DateTime.Now, "string", 1, 2, 3, null, null, 7 };
        public CustomList CustomEmptyList { get; set; } = new CustomList(true);

        public MainDataClass NullReference { get; }
        public MainDataClass[] NullArrayReference { get; }
        public MainDataClass[] EmptyArrayReference { get; } = new MainDataClass[0];
        public int? NullableInt { get; }
        public string NullableString { get; }
        public DateTime? NullableDateTime { get; }
        public List<int?> NullableIntList { get; }
        public Dictionary<string, int?> NullableStringIntDictionary { get; }
        public Dictionary<float?, int?> NullableFloatIntDictionary { get; }

    }
}
