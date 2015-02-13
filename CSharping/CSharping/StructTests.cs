using System;
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
            var data = new StructWithReferenceType(1, "Bob");

            Assert.AreEqual(1, data.Id);
            Assert.AreEqual("Bob", data.Value);
        }

        struct StructWithReferenceType
        {
            public int Id;
            public string Value;
            
            // compile error: structs cannot have an explicit parameterless constructor
            /* public StructWithReferenceType()
            {
                
            }*/

            public StructWithReferenceType(int id, string value)
            {
                Id = id;
                Value = value;
            }
        }

        [Test]
        public void Instantiate_StructWithValueTypes_WithoutNewKeyword_NeedToInitialiseAllFields()
        {
            StructWithValueTypes structWithValueTypes;
            structWithValueTypes.Id = 1;
            structWithValueTypes.Value = 12.34m;


            Assert.AreEqual(1, structWithValueTypes.Id);
            Assert.AreEqual(12.34m, structWithValueTypes.Value);
        }

        [Test]
        public void Instantiate_StructWithReferenceType_WithoutNewKeyword_NeedToInitialiseAllFields()
        {
            StructWithReferenceType structWithReferenceType;
            structWithReferenceType.Id = 2;
            structWithReferenceType.Value = "Alice";

            Assert.AreEqual(2, structWithReferenceType.Id);
            Assert.AreEqual("Alice", structWithReferenceType.Value);
        }

        struct StructWithValueTypes
        {
            public int Id;
            public decimal Value;

            public StructWithValueTypes(int id, decimal value)
            {
                Id = id;
                Value = value;
            }
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
            var s = new StructWithValueTypes(1, 100);

            Assert.IsInstanceOf<ValueType>(s);
        }

        [Test]
        public void AssignmnentToAVariable_StructIsCopied()
        {
            StructWithReferenceType s1 = new StructWithReferenceType(1, "Alice");
            StructWithReferenceType s2 = s1;

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

        struct StructImplementingInterface : IFormattable
        {
            public string ToString(string format, IFormatProvider formatProvider)
            {
                return string.Empty;
            }
        }
    }
}
