using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.Demo.Data
{
    /// <summary>
    /// A class that contains properties of any type, value, reference, collection, list, dictionary, nullable, etc.
    /// For testing the ObjectEditor.
    /// </summary>
    public class MainDataClass
    {
        public int Int { get; set; }
        public string String { get; set; }
        public DateTime DateTime { get; set; }
        public List<int> IntList { get; set; } = new List<int>() { 1, 2, 3 };
        public Dictionary<string, int> StringIntDictionary { get; set; } = new Dictionary<string, int>() { { "one", 1 }, { "two", 2 }, { "three", 3 } };
        public MainDataClass NullReference { get; set; }
        //public MainDataClass CircularReference { get; set; } = new MainDataClass(); // StackOverflowException
        public MainDataClass GenerativeProperty => new MainDataClass();
        public MainDataClass[] NullArrayReference { get; set; }
        public MainDataClass[] EmptyArrayReference { get; set; } = new MainDataClass[0];
        public CustomList CustomList { get; set; } = new CustomList() { new object(), System.DateTime.Now, "string", 1, 2, 3, null, null, 7 };
        public CustomList CustomEmptyList { get; set; } = new CustomList();
        public CustomCollection CustomCollection { get; set; } = new CustomCollection() { new object(), System.DateTime.Now, "string", 1, 2, 3, null, null, 7 };
        public CustomCollection CustomEmptyCollection { get; set; } = new CustomCollection();

        public NullableDataClass NullableData { get; set; } = new NullableDataClass();
        public ReadOnlyDataClass ReadOnlyDataClass { get; set; } = new ReadOnlyDataClass();
        public ReadOnlyNullableDataClass ReadOnlyNullableDataClass { get; set; } = new ReadOnlyNullableDataClass();
    }
}
