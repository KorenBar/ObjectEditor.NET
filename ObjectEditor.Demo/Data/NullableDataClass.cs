using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.Demo.Data
{
    public class NullableDataClass
    {
        public int? Int { get; set; }
        public string String { get; set; }
        public DateTime? DateTime { get; set; }
        public List<int?> IntList { get; set; }
        public Dictionary<string, int?> StringIntDictionary { get; set; }
        public Dictionary<float?, int?> FloatIntDictionary { get; set; }
        public Dictionary<string, float?> StringFloatDictionary { get; set; }
    }
}
