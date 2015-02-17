using System;
using NUnit.Framework;

namespace CSharping.Types
{
    // https://msdn.microsoft.com/en-us/library/sz6zd40f.aspx
    // https://msdn.microsoft.com/en-us/library/d5x73970.aspx

    [TestFixture]
    public class GenericTests
    {
        [Test]
        public void Simple()
        {
            var g = new Generic<string>();

            Type type = g.GetParameterType();

            Assert.AreEqual(typeof (string), type);
        }

        class Generic<T>
        {
            public Type GetParameterType()
            {
                return typeof(T);
            }
        }

        [Test]
        public void TypeParameterConstraint_New_ParameterlessConstructor()
        {
            var newConstraint = new GenericWithNewConstraint<Int32>();
            // compile error: string has no public parameterless constructor 
            // var newConstraint2 = new GenericWithNewConstraint<string>();

            Int32 instance = newConstraint.CreateInstance();

            Assert.IsNotNull(instance);
        }

        class GenericWithNewConstraint<T> where T : new()
        {
            public T CreateInstance()
            {
                return new T();
            }
        }

        [Test]
        public void TypeParameterConstraint_Struct()
        {
            var newConstraint = new GenericWithStructConstraint<DateTime>();
            // compile error: string is no struct
            // var newConstraint2 = new GenericWithStructConstraint<string>();

            DateTime instance = newConstraint.CreateStruct();

            Assert.IsNotNull(instance);
        }

        class GenericWithStructConstraint<T> where T : struct
        {
            public T CreateStruct()
            {
                return new T();
            }
        }
    }
}
