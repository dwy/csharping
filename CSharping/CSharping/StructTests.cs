using System;
using System.Globalization;
using NUnit.Framework;

namespace CSharping
{
    // https://msdn.microsoft.com/en-us/library/saxz13w4.aspx

    [TestFixture]
    public class StructTests
    {
        [Test]
        public void Instantiate_WithNewKeyword()
        {
            var data = new DataStruct(1, "Bob");

            Assert.AreEqual(1, data.Id);
            Assert.AreEqual("Bob", data.Value);
        }

        struct DataStruct
        {
            public int Id;
            public string Value;
            
            // compile error: structs cannot have an explicit parameterless constructor
            /* public DataStruct()
            {
                
            }*/

            public DataStruct(int id, string value)
            {
                Id = id;
                Value = value;
            }
        }

        [Test]
        public void Instantiate_WithoutNewKeyword_NeedToInitialiseAllFields()
        {
            DataStruct dataStruct;
            dataStruct.Id = 2;
            dataStruct.Value = "Alice";

            Assert.AreEqual(2, dataStruct.Id);
            Assert.AreEqual("Alice", dataStruct.Value);
        }

        [Test]
        public void StaticInitialisers_OnlyConstAndStaticFields()
        {
            Assert.AreEqual("Struct", StructWithFieldInitialisers.Name);
            Assert.AreEqual(100, StructWithFieldInitialisers.Value);
        }

        struct StructWithFieldInitialisers
        {
            public const string Name = "Struct";
            public static int Value = 100;
        }

        [Test]
        public void StructsAreValueTypes()
        {
            var s = new DataStruct(1, "Calvin");

            Assert.IsInstanceOf<ValueType>(s);
        }

        [Test]
        public void AssignmnentToAVariable_StructIsCopied()
        {
            DataStruct s1 = new DataStruct(1, "Alice");
            DataStruct s2 = s1;

            s2.Value = "Bob";

            Assert.AreEqual(1, s1.Id);
            Assert.AreEqual("Alice", s1.Value);

            Assert.AreEqual(1, s2.Id);
            Assert.AreEqual("Bob", s2.Value);
        }

        /*
         * structs cannot inherit from a struct or a class.
         * 
         * compile errors:
         *
         * struct DerivedFromStruct : StructWithValueTypes { }
         * struct DerivedFromClass : String { }
        */

        [Test]
        public void StructsCanImplementInterfaces()
        {
            var s = new StructImplementingInterface();

            string text = s.ToString("format", CultureInfo.InvariantCulture);

            Assert.AreEqual("format", text);
        }

        struct StructImplementingInterface : IFormattable
        {
            public string ToString(string format, IFormatProvider formatProvider)
            {
                return format;
            }
        }
    }
}
