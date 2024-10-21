using ObjectEditor.Data;

namespace ObjectEditor.Tests
{
    [TestClass]
    public class DynamicDictionaryWrapperUnitTest
    {

        [TestMethod]
        public void TestDynamicDictionaryWrapperIndexer()
        {
            var wrapper1 = new DynamicDictionaryWrapper(new Dictionary<string, object>
            {
                { "Name", "John" },
                { "Age", 25 },
                { "IsMarried", false }
            });
            var wrapper2 = new DynamicDictionaryWrapper(new Dictionary<string, int>
            {
                { "Age", 25 },
                { "Height", 180 },
                { "Weight", 75 },
                { "ShoeSize", 42 }
            });
            var wrapper3 = new DynamicDictionaryWrapper(new Dictionary<string, string>
            {
                { "Name", "John" },
                { "Address", "123 Main St." },
                { "City", "Springfield" },
                { "Country", "USA" }
            });

            Assert.AreEqual("John", wrapper1["Name"]);
            Assert.AreEqual(25, wrapper1["Age"]);
            Assert.AreEqual(false, wrapper1["IsMarried"]);

            Assert.AreEqual(25, wrapper2["Age"]);
            Assert.AreEqual(180, wrapper2["Height"]);
            Assert.AreEqual(75, wrapper2["Weight"]);
            Assert.AreEqual(42, wrapper2["ShoeSize"]);

            Assert.AreEqual("John", wrapper3["Name"]);
            Assert.AreEqual("123 Main St.", wrapper3["Address"]);
            Assert.AreEqual("Springfield", wrapper3["City"]);
            Assert.AreEqual("USA", wrapper3["Country"]);

            wrapper1["Name"] = "Jane";
            wrapper1["Age"] = 30;
            wrapper1["IsMarried"] = true;

            wrapper2["Age"] = 30;
            wrapper2["Height"] = 185;
            wrapper2["Weight"] = 80;
            wrapper2["ShoeSize"] = 45;

            wrapper3["Name"] = "Jane";
            wrapper3["Address"] = "456 Elm St.";
            wrapper3["City"] = "Shelbyville";
            wrapper3["Country"] = "USA";

            Assert.AreEqual("Jane", wrapper1["Name"]);
            Assert.AreEqual(30, wrapper1["Age"]);
            Assert.AreEqual(true, wrapper1["IsMarried"]);

            Assert.AreEqual(30, wrapper2["Age"]);
            Assert.AreEqual(185, wrapper2["Height"]);
            Assert.AreEqual(80, wrapper2["Weight"]);
            Assert.AreEqual(45, wrapper2["ShoeSize"]);

            Assert.AreEqual("Jane", wrapper3["Name"]);
            Assert.AreEqual("456 Elm St.", wrapper3["Address"]);
            Assert.AreEqual("Shelbyville", wrapper3["City"]);
            Assert.AreEqual("USA", wrapper3["Country"]);
        }

        [TestMethod]
        public void TestDynamicDictionaryWrapperContains()
        {
            var wrapper1 = new DynamicDictionaryWrapper(new Dictionary<string, object>
            {
                { "Name", "John" },
                { "Age", 25 },
                { "IsMarried", false }
            });
            var wrapper2 = new DynamicDictionaryWrapper(new Dictionary<string, int>
            {
                { "Age", 25 },
                { "Height", 180 },
                { "Weight", 75 },
                { "ShoeSize", 42 }
            });
            var wrapper3 = new DynamicDictionaryWrapper(new Dictionary<string, string>
            {
                { "Name", "John" },
                { "Address", "123 Main St." },
                { "City", "Springfield" },
                { "Country", "USA" }
            });

            Assert.IsTrue(wrapper1.ContainsKey("Name"));
            Assert.IsTrue(wrapper1.ContainsKey("Age"));
            Assert.IsTrue(wrapper1.ContainsKey("IsMarried"));
            Assert.IsFalse(wrapper1.ContainsKey("Height"));
            Assert.IsFalse(wrapper1.ContainsKey("Weight"));
            Assert.IsFalse(wrapper1.ContainsKey("ShoeSize"));

            Assert.IsTrue(wrapper2.ContainsKey("Age"));
            Assert.IsTrue(wrapper2.ContainsKey("Height"));
            Assert.IsTrue(wrapper2.ContainsKey("Weight"));
            Assert.IsTrue(wrapper2.ContainsKey("ShoeSize"));
            Assert.IsFalse(wrapper2.ContainsKey("Name"));
            Assert.IsFalse(wrapper2.ContainsKey("IsMarried"));

            Assert.IsTrue(wrapper3.ContainsKey("Name"));
            Assert.IsTrue(wrapper3.ContainsKey("Address"));
            Assert.IsTrue(wrapper3.ContainsKey("City"));
            Assert.IsTrue(wrapper3.ContainsKey("Country"));
            Assert.IsFalse(wrapper3.ContainsKey("Age"));
            Assert.IsFalse(wrapper3.ContainsKey("Height"));
            Assert.IsFalse(wrapper3.ContainsKey("Weight"));
            Assert.IsFalse(wrapper3.ContainsKey("ShoeSize"));

            // Invalid key type
            Assert.IsFalse(wrapper1.ContainsKey(0));
            Assert.IsFalse(wrapper1.ContainsKey(0.0f));
            Assert.IsFalse(wrapper1.ContainsKey(true));
            Assert.IsFalse(wrapper1.ContainsKey(new object()));
            Assert.IsFalse(wrapper1.ContainsKey(new KeyValuePair<object, object>("key", "value")));
            Assert.IsFalse(wrapper2.ContainsKey(0));
            Assert.IsFalse(wrapper2.ContainsKey(0.0f));
            Assert.IsFalse(wrapper2.ContainsKey(true));
            Assert.IsFalse(wrapper2.ContainsKey(new object()));
            Assert.IsFalse(wrapper2.ContainsKey(new KeyValuePair<object, object>("key", "value")));

            Assert.IsTrue(wrapper1.Contains(new KeyValuePair<object, object>("Name", "John")));
            Assert.IsTrue(wrapper1.Contains(new KeyValuePair<object, object>("Age", 25)));
            Assert.IsTrue(wrapper1.Contains(new KeyValuePair<object, object>("IsMarried", false)));
            Assert.IsFalse(wrapper1.Contains(new KeyValuePair<object, object>("Height", 180)));
            Assert.IsFalse(wrapper1.Contains(new KeyValuePair<object, object>("Weight", 75)));
            Assert.IsFalse(wrapper1.Contains(new KeyValuePair<object, object>("ShoeSize", 42)));

            Assert.IsTrue(wrapper2.Contains(new KeyValuePair<object, object>("Age", 25)));
            Assert.IsTrue(wrapper2.Contains(new KeyValuePair<object, object>("Height", 180)));
            Assert.IsTrue(wrapper2.Contains(new KeyValuePair<object, object>("Weight", 75)));
            Assert.IsTrue(wrapper2.Contains(new KeyValuePair<object, object>("ShoeSize", 42)));
            Assert.IsFalse(wrapper2.Contains(new KeyValuePair<object, object>("Name", 0)));
            Assert.IsFalse(wrapper2.Contains(new KeyValuePair<object, object>("IsMarried", 0)));

            Assert.IsTrue(wrapper3.Contains(new KeyValuePair<object, object>("Name", "John")));
            Assert.IsTrue(wrapper3.Contains(new KeyValuePair<object, object>("Address", "123 Main St.")));
            Assert.IsTrue(wrapper3.Contains(new KeyValuePair<object, object>("City", "Springfield")));
            Assert.IsTrue(wrapper3.Contains(new KeyValuePair<object, object>("Country", "USA")));
            Assert.IsFalse(wrapper3.Contains(new KeyValuePair<object, object>("Age", "25")));
            Assert.IsFalse(wrapper3.Contains(new KeyValuePair<object, object>("Height", "180")));
            Assert.IsFalse(wrapper3.Contains(new KeyValuePair<object, object>("Weight", "75")));
            Assert.IsFalse(wrapper3.Contains(new KeyValuePair<object, object>("ShoeSize", "42")));

            // Invalid types
            Assert.IsFalse(wrapper1.Contains(new KeyValuePair<object, object>(0, 0)));
            Assert.IsFalse(wrapper1.Contains(new KeyValuePair<object, object>(0.0f, 0.0f)));
            Assert.IsFalse(wrapper2.Contains(new KeyValuePair<object, object>("Age", true)));
            Assert.IsFalse(wrapper2.Contains(new KeyValuePair<object, object>("Height", "180")));
            Assert.IsFalse(wrapper3.Contains(new KeyValuePair<object, object>("Weight", 75)));
            Assert.IsFalse(wrapper3.Contains(new KeyValuePair<object, object>("Name", false)));
        }

        [TestMethod]
        public void TestDynamicDictionaryWrapperAdd()
        {
            var wrapper1 = new DynamicDictionaryWrapper(new Dictionary<string, object>
            {
                { "Name", "John" },
                { "Age", 25 },
                { "IsMarried", false }
            });
            var wrapper2 = new DynamicDictionaryWrapper(new Dictionary<string, int>
            {
                { "Age", 25 },
                { "Height", 180 },
                { "Weight", 75 },
                { "ShoeSize", 42 }
            });
            var wrapper3 = new DynamicDictionaryWrapper(new Dictionary<string, string>
            {
                { "Name", "John" },
                { "Address", "123 Main St." },
                { "City", "Springfield" },
                { "Country", "USA" }
            });

            wrapper1.Add("Height", 180);
            wrapper1.Add("Weight", 75);
            wrapper1.Add("ShoeSize", 42);
            wrapper1.Add("Object", new object());
            wrapper1.Add("String", "string");
            wrapper1.Add("KPV", new KeyValuePair<object, object>("key", "value"));

            wrapper2.Add("int1", 1);
            //wrapper2.Add("float", 1.0f); // will not be converted to int
            wrapper2.Add("int2", 2);

            // ArgumentException: An item with the same key has already been added. Key: {key}
            //wrapper3.Add("Name", "John");
            //wrapper3.Add("Address", "123 Main St.");
            //wrapper3.Add("City", "Springfield");
            //wrapper3.Add("Country", "USA");

            wrapper3.Add("Address2", "222 St.");

            Assert.AreEqual("John", wrapper1["Name"]);
            Assert.AreEqual(25, wrapper1["Age"]);
            Assert.AreEqual(false, wrapper1["IsMarried"]);
            Assert.AreEqual(180, wrapper1["Height"]);
            Assert.AreEqual(75, wrapper1["Weight"]);
            Assert.AreEqual(42, wrapper1["ShoeSize"]);
            Assert.IsTrue(wrapper1["Object"] != null && wrapper1["Object"].GetType().Equals(typeof(object)));
            Assert.AreEqual("string", wrapper1["String"]);
            Assert.AreEqual(new KeyValuePair<object, object>("key", "value"), wrapper1["KPV"]);

            Assert.AreEqual(25, wrapper2["Age"]);
            Assert.AreEqual(1, wrapper2["int1"]);
            Assert.AreEqual(2, wrapper2["int2"]);

            Assert.AreEqual("John", wrapper3["Name"]);
            Assert.AreEqual("123 Main St.", wrapper3["Address"]);
            Assert.AreEqual("Springfield", wrapper3["City"]);
            Assert.AreEqual("USA", wrapper3["Country"]);
            Assert.AreEqual("222 St.", wrapper3["Address2"]);
        }

        [TestMethod]
        public void TestDynamicDictionaryWrapperRemove()
        {
            var wrapper1 = new DynamicDictionaryWrapper(new Dictionary<string, object>
            {
                { "Name", "John" },
                { "Age", 25 },
                { "IsMarried", false }
            });
            var wrapper2 = new DynamicDictionaryWrapper(new Dictionary<string, int>
            {
                { "Age", 25 },
                { "Height", 180 },
                { "Weight", 75 },
                { "ShoeSize", 42 },
                { "f", 185 }
            });
            var wrapper3 = new DynamicDictionaryWrapper(new Dictionary<string, string>
            {
                { "Name", "John" },
                { "Address", "123 Main St." },
                { "City", "Springfield" },
                { "Country", "USA" },
                { "Address2", "222 St." }
            });

            Assert.IsTrue(wrapper1.Remove("Name"));
            Assert.IsTrue(wrapper1.Remove("Age"));
            Assert.IsTrue(wrapper1.Remove("IsMarried"));

            Assert.IsTrue(wrapper2.Remove("Age"));
            Assert.IsTrue(wrapper2.Remove("Weight"));
            Assert.IsTrue(wrapper2.Remove("ShoeSize"));

            Assert.IsTrue(wrapper3.Remove("Name"));
            Assert.IsTrue(wrapper3.Remove("Address"));
            Assert.IsTrue(wrapper3.Remove("Country"));

            Assert.IsFalse(wrapper1.Remove("Name"));
            Assert.IsFalse(wrapper1.Remove("Age"));
            Assert.IsFalse(wrapper1.Remove("NotExists"));
            Assert.IsFalse(wrapper1.Remove("NotExists"));
            Assert.IsFalse(wrapper2.Remove("NotExists"));
            Assert.IsFalse(wrapper2.Remove("NotExists"));
            Assert.IsFalse(wrapper3.Remove("NotExists"));
            Assert.IsFalse(wrapper3.Remove("NotExists"));

            // Invalid types
            Assert.IsFalse(wrapper1.Remove(false));
            Assert.IsFalse(wrapper2.Remove('f')); // it's a char, not a string
            Assert.IsFalse(wrapper3.Remove(false));
            Assert.IsFalse(wrapper3.Remove(0));

            var keys1 = wrapper1.Keys;
            var keys2 = wrapper2.Keys;
            var keys3 = wrapper3.Keys;

            Assert.IsFalse(keys1.Contains("Name"));
            Assert.IsFalse(keys1.Contains("Age"));
            Assert.IsFalse(keys1.Contains("IsMarried"));

            Assert.IsFalse(keys2.Contains("Age"));
            Assert.IsTrue(keys2.Contains("Height"));
            Assert.IsFalse(keys2.Contains("Weight"));
            Assert.IsFalse(keys2.Contains("ShoeSize"));
            Assert.IsTrue(keys2.Contains("f"));

            Assert.IsFalse(keys3.Contains("Name"));
            Assert.IsFalse(keys3.Contains("Address"));
            Assert.IsTrue(keys3.Contains("City"));
            Assert.IsFalse(keys3.Contains("Country"));
            Assert.IsTrue(keys3.Contains("Address2"));

            Assert.IsFalse(wrapper1.Remove(new KeyValuePair<object, object>("Name", "John")));
            Assert.IsFalse(wrapper1.Remove(new KeyValuePair<object, object>("Age", 25)));
            Assert.IsFalse(wrapper1.Remove(new KeyValuePair<object, object>("IsMarried", false)));

            Assert.IsFalse(wrapper2.Remove(new KeyValuePair<object, object>("Age", 25)));
            Assert.IsTrue(wrapper2.Remove(new KeyValuePair<object, object>("Height", 180)));
            Assert.IsFalse(wrapper2.Remove(new KeyValuePair<object, object>("Weight", 75)));
            Assert.IsFalse(wrapper2.Remove(new KeyValuePair<object, object>("ShoeSize", 42)));
            Assert.IsFalse(wrapper2.Remove(new KeyValuePair<object, object>("f", 10))); // not the same value
            Assert.IsFalse(wrapper2.Remove(new KeyValuePair<object, object>(false, 185))); // invalid key type
            Assert.IsFalse(wrapper2.Remove(new KeyValuePair<object, object>('f', 185))); // char instead of string
            Assert.IsTrue(wrapper2.Remove(new KeyValuePair<object, object>("f", 185)));

        }

        [TestMethod]
        public void TestDynamicDictionaryWrapperAddDefaultValue()
        {
            var wrapper1 = new DynamicDictionaryWrapper(new Dictionary<string, object>
            {
                { "Name", "John" },
                { "Age", 25 },
                { "IsMarried", false }
            });
            var wrapper2 = new DynamicDictionaryWrapper(new Dictionary<string, int>
            {
                { "Age", 25 },
                { "Height", 180 },
                { "Weight", 75 },
                { "ShoeSize", 42 }
            });
            var wrapper3 = new DynamicDictionaryWrapper(new Dictionary<string, string>
            {
                { "Name", "John" },
                { "Address", "123 Main St." },
                { "City", "Springfield" },
                { "Country", "USA" }
            });

            var entry1 = wrapper1.AddDefaultValue();
            var entry2 = wrapper2.AddDefaultValue();
            var entry3 = wrapper3.AddDefaultValue();

            var val1 = wrapper1[""];
            Assert.IsTrue(val1 != null);
            Assert.IsTrue(val1.GetType().Equals(typeof(object)));
            Assert.AreEqual(0, wrapper2[""]);
            Assert.AreEqual("", wrapper3[""]);
        }

        [TestMethod]
        public void TestDynamicDictionaryWrapperKeys()
        {
            var wrapper1 = new DynamicDictionaryWrapper(new Dictionary<string, object>
            {
                { "Name", "John" },
                { "Age", 25 },
                { "IsMarried", false }
            });
            var wrapper2 = new DynamicDictionaryWrapper(new Dictionary<string, int>
            {
                { "Age", 25 },
                { "Height", 180 },
                { "Weight", 75 },
                { "ShoeSize", 42 }
            });
            var wrapper3 = new DynamicDictionaryWrapper(new Dictionary<string, string>
            {
                { "Name", "John" },
                { "Address", "123 Main St." },
                { "City", "Springfield" },
                { "Country", "USA" }
            });

            var keys1 = wrapper1.Keys;
            var keys2 = wrapper2.Keys;
            var keys3 = wrapper3.Keys;

            Assert.AreEqual(3, keys1.Count);
            Assert.AreEqual(4, keys2.Count);
            Assert.AreEqual(4, keys3.Count);

            Assert.IsTrue(keys1.Contains("Name"));
            Assert.IsTrue(keys1.Contains("Age"));
            Assert.IsTrue(keys1.Contains("IsMarried"));
            Assert.IsFalse(keys1.Contains("Height"));
            Assert.IsFalse(keys1.Contains("Weight"));
            Assert.IsFalse(keys1.Contains("ShoeSize"));

            Assert.IsTrue(keys2.Contains("Age"));
            Assert.IsTrue(keys2.Contains("Height"));
            Assert.IsTrue(keys2.Contains("Weight"));
            Assert.IsTrue(keys2.Contains("ShoeSize"));
            Assert.IsFalse(keys2.Contains("Name"));
            Assert.IsFalse(keys2.Contains("IsMarried"));

            Assert.IsTrue(keys3.Contains("Name"));
            Assert.IsTrue(keys3.Contains("Address"));
            Assert.IsTrue(keys3.Contains("City"));
            Assert.IsTrue(keys3.Contains("Country"));
            Assert.IsFalse(keys3.Contains("Age"));
            Assert.IsFalse(keys3.Contains("Height"));
            Assert.IsFalse(keys3.Contains("Weight"));
            Assert.IsFalse(keys3.Contains("ShoeSize"));
        }

        [TestMethod]
        public void TestDynamicDictionaryWrapperValues()
        {
            var wrapper1 = new DynamicDictionaryWrapper(new Dictionary<string, object>
            {
                { "Name", "John" },
                { "Age", 25 },
                { "IsMarried", false }
            });
            var wrapper2 = new DynamicDictionaryWrapper(new Dictionary<string, int>
            {
                { "Age", 25 },
                { "Height", 180 },
                { "Weight", 75 },
                { "ShoeSize", 42 }
            });
            var wrapper3 = new DynamicDictionaryWrapper(new Dictionary<string, string>
            {
                { "Name", "John" },
                { "Address", "123 Main St." },
                { "City", "Springfield" },
                { "Country", "USA" }
            });

            var values1 = wrapper1.Values;
            var values2 = wrapper2.Values;
            var values3 = wrapper3.Values;

            Assert.AreEqual(3, values1.Count);
            Assert.AreEqual(4, values2.Count);
            Assert.AreEqual(4, values3.Count);

            Assert.IsTrue(values1.Contains("John"));
            Assert.IsTrue(values1.Contains(25));
            Assert.IsTrue(values1.Contains(false));
            Assert.IsFalse(values1.Contains(180));
            Assert.IsFalse(values1.Contains(75));
            Assert.IsFalse(values1.Contains(42));

            Assert.IsTrue(values2.Contains(25));
            Assert.IsTrue(values2.Contains(180));
            Assert.IsTrue(values2.Contains(75));
            Assert.IsTrue(values2.Contains(42));
            Assert.IsFalse(values2.Contains("John"));
            Assert.IsFalse(values2.Contains(false));

            Assert.IsTrue(values3.Contains("John"));
            Assert.IsTrue(values3.Contains("123 Main St."));
            Assert.IsTrue(values3.Contains("Springfield"));
            Assert.IsTrue(values3.Contains("USA"));
            Assert.IsFalse(values3.Contains(25));
            Assert.IsFalse(values3.Contains(180));
            Assert.IsFalse(values3.Contains(75));
            Assert.IsFalse(values3.Contains(42));
        }

        [TestMethod]
        public void TestDynamicDictionaryWrapperCopyTo()
        {
            var wrapper1 = new DynamicDictionaryWrapper(new Dictionary<string, object>
            {
                { "Name", "John" },
                { "Age", 25 },
                { "IsMarried", false }
            });
            var wrapper2 = new DynamicDictionaryWrapper(new Dictionary<string, int>
            {
                { "Age", 25 },
                { "Height", 180 },
                { "Weight", 75 },
                { "ShoeSize", 42 }
            });
            var wrapper3 = new DynamicDictionaryWrapper(new Dictionary<string, string>
            {
                { "Name", "John" },
                { "Address", "123 Main St." },
                { "City", "Springfield" },
                { "Country", "USA" }
            });

            var array1 = new KeyValuePair<object, object>[3];
            var array2 = new KeyValuePair<object, object>[4];
            var array3 = new KeyValuePair<object, object>[4];

            wrapper1.CopyTo(array1, 0);
            wrapper2.CopyTo(array2, 0);
            wrapper3.CopyTo(array3, 0);

            Assert.AreEqual("Name", array1[0].Key);
            Assert.AreEqual("John", array1[0].Value);
            Assert.AreEqual("Age", array1[1].Key);
            Assert.AreEqual(25, array1[1].Value);
            Assert.AreEqual("IsMarried", array1[2].Key);
            Assert.AreEqual(false, array1[2].Value);

            Assert.AreEqual("Age", array2[0].Key);
            Assert.AreEqual(25, array2[0].Value);
            Assert.AreEqual("Height", array2[1].Key);
            Assert.AreEqual(180, array2[1].Value);
            Assert.AreEqual("Weight", array2[2].Key);
            Assert.AreEqual(75, array2[2].Value);
            Assert.AreEqual("ShoeSize", array2[3].Key);
            Assert.AreEqual(42, array2[3].Value);

            Assert.AreEqual("Name", array3[0].Key);
            Assert.AreEqual("John", array3[0].Value);
            Assert.AreEqual("Address", array3[1].Key);
            Assert.AreEqual("123 Main St.", array3[1].Value);
            Assert.AreEqual("City", array3[2].Key);
            Assert.AreEqual("Springfield", array3[2].Value);
            Assert.AreEqual("Country", array3[3].Key);
            Assert.AreEqual("USA", array3[3].Value);
        }

        [TestMethod]
        public void TestDynamicDictionaryWrapperContainsKey()
        {
            var wrapper1 = new DynamicDictionaryWrapper(new Dictionary<string, object>
            {
                { "Name", "John" },
                { "Age", 25 },
                { "IsMarried", false }
            });
            var wrapper2 = new DynamicDictionaryWrapper(new Dictionary<string, int>
            {
                { "Age", 25 },
                { "Height", 180 },
                { "Weight", 75 },
                { "ShoeSize", 42 }
            });
            var wrapper3 = new DynamicDictionaryWrapper(new Dictionary<string, string>
            {
                { "Name", "John" },
                { "Address", "123 Main St." },
                { "City", "Springfield" },
                { "Country", "USA" }
            });

            Assert.IsTrue(wrapper1.ContainsKey("Name"));
            Assert.IsTrue(wrapper1.ContainsKey("Age"));
            Assert.IsTrue(wrapper1.ContainsKey("IsMarried"));
            Assert.IsFalse(wrapper1.ContainsKey("Height"));
            Assert.IsFalse(wrapper1.ContainsKey("Weight"));
            Assert.IsFalse(wrapper1.ContainsKey("ShoeSize"));

            Assert.IsTrue(wrapper2.ContainsKey("Age"));
            Assert.IsTrue(wrapper2.ContainsKey("Height"));
            Assert.IsTrue(wrapper2.ContainsKey("Weight"));
            Assert.IsTrue(wrapper2.ContainsKey("ShoeSize"));
            Assert.IsFalse(wrapper2.ContainsKey("Name"));
            Assert.IsFalse(wrapper2.ContainsKey("IsMarried"));

            Assert.IsTrue(wrapper3.ContainsKey("Name"));
            Assert.IsTrue(wrapper3.ContainsKey("Address"));
            Assert.IsTrue(wrapper3.ContainsKey("City"));
            Assert.IsTrue(wrapper3.ContainsKey("Country"));
            Assert.IsFalse(wrapper3.ContainsKey("Age"));
            Assert.IsFalse(wrapper3.ContainsKey("Height"));
            Assert.IsFalse(wrapper3.ContainsKey("Weight"));
            Assert.IsFalse(wrapper3.ContainsKey("ShoeSize"));
        }

        [TestMethod]
        public void TestDynamicDictionaryWrapperAddKeyValuePair()
        {
            var wrapper1 = new DynamicDictionaryWrapper(new Dictionary<string, object>
            {
                { "Name", "John" },
                { "Age", 25 },
                { "IsMarried", false }
            });
            var wrapper2 = new DynamicDictionaryWrapper(new Dictionary<string, int>
            {
                { "Age", 25 },
                { "Height", 180 },
                { "Weight", 75 },
                { "ShoeSize", 42 }
            });
            var wrapper3 = new DynamicDictionaryWrapper(new Dictionary<string, string>
            {
                { "Name", "John" },
                { "Address", "123 Main St." },
                { "City", "Springfield" },
                { "Country", "USA" }
            });

            wrapper1.Add(new KeyValuePair<object, object>("Height", 180));
            wrapper1.Add(new KeyValuePair<object, object>("Weight", 75));
            wrapper1.Add(new KeyValuePair<object, object>("ShoeSize", 42));
            wrapper1.Add(new KeyValuePair<object, object>("Address", "123 Main St."));
            wrapper1.Add(new KeyValuePair<object, object>("City", "Springfield"));
            wrapper1.Add(new KeyValuePair<object, object>("Country", "USA"));

            wrapper2.Add(new KeyValuePair<object, object>("int1", 1));
            //wrapper2.Add(new KeyValuePair<object, object>("float", 1.0f)); // will not be converted to int
            wrapper2.Add(new KeyValuePair<object, object>("int2", 2));

            // ArgumentException: An item with the same key has already been added. Key: {key}
            //wrapper3.Add(new KeyValuePair<object, object>("Name", "John"));

            Assert.AreEqual("John", wrapper1["Name"]);
            Assert.AreEqual(25, wrapper1["Age"]);
            Assert.AreEqual(false, wrapper1["IsMarried"]);
            Assert.AreEqual(180, wrapper1["Height"]);
            Assert.AreEqual(75, wrapper1["Weight"]);
            Assert.AreEqual(42, wrapper1["ShoeSize"]);
            Assert.AreEqual("123 Main St.", wrapper1["Address"]);
            Assert.AreEqual("Springfield", wrapper1["City"]);
            Assert.AreEqual("USA", wrapper1["Country"]);

            Assert.AreEqual(25, wrapper2["Age"]);
            Assert.AreEqual(1, wrapper2["int1"]);
            Assert.AreEqual(2, wrapper2["int2"]);

            Assert.AreEqual("John", wrapper3["Name"]);
            Assert.AreEqual("123 Main St.", wrapper3["Address"]);
            Assert.AreEqual("Springfield", wrapper3["City"]);
            Assert.AreEqual("USA", wrapper3["Country"]);
        }

        [TestMethod]
        public void TestDynamicDictionaryWrapperTryGetValue()
        {
            var wrapper1 = new DynamicDictionaryWrapper(new Dictionary<string, object>
            {
                { "Name", "John" },
                { "Age", 25 },
                { "IsMarried", false }
            });
            var wrapper2 = new DynamicDictionaryWrapper(new Dictionary<string, int>
            {
                { "Age", 25 },
                { "Height", 180 },
                { "Weight", 75 },
                { "ShoeSize", 42 }
            });
            var wrapper3 = new DynamicDictionaryWrapper(new Dictionary<string, string>
            {
                { "Name", "John" },
                { "Address", "123 Main St." },
                { "City", "Springfield" },
                { "Country", "USA" }
            });

            object val1;
            object val2;
            object val3;

            Assert.IsTrue(wrapper1.TryGetValue("Name", out val1));
            Assert.IsTrue(wrapper1.TryGetValue("Age", out val2));
            Assert.IsTrue(wrapper1.TryGetValue("IsMarried", out val3));
            Assert.AreEqual("John", val1);
            Assert.AreEqual(25, val2);
            Assert.AreEqual(false, val3);

            Assert.IsTrue(wrapper2.TryGetValue("Age", out val1));
            Assert.IsTrue(wrapper2.TryGetValue("Height", out val2));
            Assert.IsTrue(wrapper2.TryGetValue("Weight", out val3));
            Assert.AreEqual(25, val1);
            Assert.AreEqual(180, val2);
            Assert.AreEqual(75, val3);

            Assert.IsTrue(wrapper3.TryGetValue("Name", out val1));
            Assert.IsTrue(wrapper3.TryGetValue("Address", out val2));
            Assert.IsTrue(wrapper3.TryGetValue("City", out val3));
            Assert.AreEqual("John", val1);
            Assert.AreEqual("123 Main St.", val2);
            Assert.AreEqual("Springfield", val3);

            Assert.IsFalse(wrapper1.TryGetValue("Height", out val1));
            Assert.IsFalse(wrapper1.TryGetValue("Weight", out val2));
            Assert.IsFalse(wrapper1.TryGetValue("ShoeSize", out val3));
            Assert.IsFalse(wrapper1.TryGetValue("Address", out val1));
            Assert.IsFalse(wrapper1.TryGetValue("City", out val2));
            Assert.IsFalse(wrapper1.TryGetValue("Country", out val3));
            Assert.AreEqual(null, val1);
            Assert.AreEqual(null, val2);
            Assert.AreEqual(null, val3);

            Assert.IsFalse(wrapper2.TryGetValue("Name", out val1));
            Assert.IsFalse(wrapper2.TryGetValue("IsMarried", out val2));
            Assert.IsFalse(wrapper2.TryGetValue("Address", out val3));
            // the values should be null even when the source dictionary value type is not nullable, the wrapper should return null
            Assert.AreEqual(null, val1);
            Assert.AreEqual(null, val2);
            Assert.AreEqual(null, val3);
        }
    }
}