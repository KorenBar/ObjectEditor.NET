using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.Demo.Data
{
    public class ItemClass
    {
        [EditorDisplayName]
        public string Name { get; set; } = "ItemName";
        [EditorDisplayName]
        public int Value { get; set; } = Random.Shared.Next();
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
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
        [EditorDisplayName]
        public TestEnum Enum { get; set; }
    }
}
