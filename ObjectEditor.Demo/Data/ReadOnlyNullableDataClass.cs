using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.Demo.Data
{
    public class ReadOnlyNullableDataClass
    {
        public int? NullInt { get; }
        public string NullString { get; }
        public DateTime? NullDateTime { get; }
        public List<int?> NullIntList { get; }
        public Dictionary<string, int?> NullStringIntDictionary { get; }
        public Dictionary<float?, int?> NullFloatIntDictionary { get; }
        public Dictionary<string, float?> NullStringFloatDictionary { get; }

        public int? Int { get; } = 1;
        public string String { get; } = "string";
        public DateTime? DateTime { get; } = System.DateTime.Now;
        public List<int?> IntList { get; } = new List<int?>() { 1, 2, 3, null, null, 7 };
        public Dictionary<string, int?> StringIntDictionary { get; } = new Dictionary<string, int?>() { { "one", 1 }, { "two", 2 }, { "three", 3 }, { "null", null }, { "null2", null }, { "seven", 7 } };
        public Dictionary<float?, int?> FloatIntDictionary { get; } = new Dictionary<float?, int?>() { { 1.1f, 1 }, { 2.2f, 2 }, { 3.3f, 3 }, { 4.4f, null }, { 5.5f, null }, { 7.7f, 7 } };
        public Dictionary<string, float?> StringFloatDictionary { get; } = new Dictionary<string, float?>() { { "one", 1.1f }, { "two", 2.2f }, { "three", 3.3f }, { "null", null }, { "null2", null }, { "seven", 7.7f } };
    }
}
