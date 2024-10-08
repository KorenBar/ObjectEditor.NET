using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectEditor;

namespace ObjectEditor.Demo.Data
{
    public enum TestEnum { Zero, One, Two, Three }

    /// <summary>
    /// A class that contains properties of any type, value, reference, collection, list, dictionary, nullable, etc.
    /// For testing the ObjectEditor.
    /// </summary>
    public class MainDataClass
    {
        public MainDataClass GenerativeProperty => new MainDataClass();

        [Info("An integer property with description.")]
        public int Int { get; set; }
        [Info("A string property with description.")]
        public string String { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public bool Bool { get; set; }
        public double Double { get; set; }
        public float Float { get; set; }
        public decimal Decimal { get; set; }
        public char Char { get; set; }
        public byte Byte { get; set; }
        public sbyte SByte { get; set; }
        public short Short { get; set; }
        public ushort UShort { get; set; }
        public uint UInt { get; set; }
        public long Long { get; set; }
        public TestEnum Enum { get; set; }

        public List<int> IntList { get; set; } = new List<int>() { 1, 2, 3 };
        public Dictionary<string, int> StringIntDictionary { get; set; } = new Dictionary<string, int>() { { "one", 1 }, { "two", 2 }, { "three", 3 } };
        [Info("A null reference property with description.")]
        public MainDataClass NullReference { get; set; }
        //public MainDataClass CircularReference { get; set; } = new MainDataClass(); // StackOverflowException
        public MainDataClass[] NullArrayReference { get; set; }
        public MainDataClass[] EmptyArrayReference { get; set; } = new MainDataClass[0];
        public object ValueAsObject { get; set; } = 1;
        public CustomList CustomList { get; set; } = new CustomList() { new object(), System.DateTime.Now, "string", 1, 2, 3, null, null, 7 };
        public CustomList CustomEmptyList { get; set; } = new CustomList();
        public CustomCollection CustomCollection { get; set; } = new CustomCollection() { new object(), System.DateTime.Now, "string", 1, 2, 3, null, null, 7 };
        public CustomCollection CustomEmptyCollection { get; set; } = new CustomCollection();

        public NullableDataClass NullableData { get; set; } = new NullableDataClass();
        public ReadOnlyDataClass ReadOnlyDataClass { get; set; } = new ReadOnlyDataClass();
        public ReadOnlyNullableDataClass ReadOnlyNullableDataClass { get; set; } = new ReadOnlyNullableDataClass();
    }
}
