﻿using System;
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
        [EditorPassword]
        public string Password { get; set; } = "secret_password";
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

        public List<User> Users { get; set; } = new List<User>() {
            new User() { Id = 1, Name = "UserAllAccessible", Password = "password1",
                Permissions = null // all fields are accessible
            },
            new User() { Id = 2, Name = "UserOnlyUngrouped", Password = "password2",
                Permissions = new() { } // no permissions
            },
            new User() { Id = 3, Name = "ManagerUser", Password = "password3",
                Permissions = new() {
                    { "Manager", Permissions.ReadWrite },
                    { "Admin", Permissions.Read }
                }
            },
            new User() { Id = 3, Name = "ManagerOnlyUser", Password = "password3",
                Permissions = new() {
                    { "Manager", Permissions.ReadWrite }
                }
            },
            new User() { Id = 3, Name = "AdminUser", Password = "password3",
                Permissions = new() {
                    { "Manager", Permissions.ReadWrite },
                    { "Admin", Permissions.ReadWrite }
                }
            }
        };
        public List<int> IntList { get; set; } = new List<int>() { 1, 2, 3 };
        public Dictionary<string, int> StringIntDictionary { get; set; } = new Dictionary<string, int>() { { "one", 1 }, { "two", 2 }, { "three", 3 } };
        [Info("A null reference property with description.")]
        public MainDataClass NullReference { get; set; }
        //public MainDataClass CircularReference { get; set; } = new MainDataClass(); // StackOverflowException
        public MainDataClass[] NullArrayReference { get; set; }
        public MainDataClass[] EmptyArrayReference { get; set; } = new MainDataClass[0];
        public List<ItemClass> ListOfObjects { get; set; } = new List<ItemClass>() { new ItemClass(), new ItemClass(), new ItemClass() };
        public object ValueAsObject { get; set; } = 1;
        public InheritingClass InheritingClass { get; set; } = new InheritingClass();
        public CustomList CustomList { get; set; } = new CustomList() { new object(), System.DateTime.Now, "string", 1, 2, 3, null, null, 7 };
        public CustomList CustomEmptyList { get; set; } = new CustomList();
        public CustomCollection CustomCollection { get; set; } = new CustomCollection() { new object(), System.DateTime.Now, "string", 1, 2, 3, null, null, 7 };
        public CustomCollection CustomEmptyCollection { get; set; } = new CustomCollection();

        public NullableDataClass NullableData { get; set; } = new NullableDataClass();
        public ReadOnlyDataClass ReadOnlyDataClass { get; set; } = new ReadOnlyDataClass();
        public ReadOnlyNullableDataClass ReadOnlyNullableDataClass { get; set; } = new ReadOnlyNullableDataClass();
    }
}
