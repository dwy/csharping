using System;
using System.Globalization;
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
            var generic = new GenericWithNewConstraint<Int32>();
            // compile error: string has no public parameterless constructor 
            // var generic2 = new GenericWithNewConstraint<string>();

            Int32 instance = generic.CreateInstance();

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
            var generic = new GenericWithStructConstraint<DateTime>();
            // compile error: string is no struct
            // var generic2 = new GenericWithStructConstraint<string>();

            DateTime instance = generic.CreateStruct();

            Assert.IsNotNull(instance);
        }

        class GenericWithStructConstraint<T> where T : struct
        {
            public T CreateStruct()
            {
                return new T();
            }
        }

        [Test]
        public void TypeParameterConstraint_BaseClassOrInterface()
        {
            var matchingInstanceToPass = new ComparableStringInfo();
            var generic = new GenericWithBaseClassOrInterface<ComparableStringInfo>(matchingInstanceToPass);
            // compile error: string does not have base class nor implements interface
            // var generic2 = new GenericWithBaseClassOrInterface<string>("");

            ComparableStringInfo instance = generic.GetInstance();

            Assert.IsNotNull(instance);
        }

        class GenericWithBaseClassOrInterface<T> where T : StringInfo, IComparable
        {
            private readonly T _instance;

            public GenericWithBaseClassOrInterface(T instance)
            {
                _instance = instance;
            }

            public T GetInstance()
            {
                return _instance;
            }
        }

        class ComparableStringInfo: StringInfo, IComparable
        {
            public int CompareTo(object obj)
            {
                return 0;
            }
        }

        [Test]
        public void TypeParameterConstraint_AnotherTypeParameter()
        {
            var generic = new GenericWithAnotherTypeParameter<ArgumentException, Exception>();
            // compile error: ArgumentException does not implement IList
            // var generic2 = new GenericWithAnotherTypeParameter<ArgumentException, IList>();

            var instance = generic.GetInstance();

            Assert.IsNotNull(instance);
        }

        class GenericWithAnotherTypeParameter<TDerived, TBase> where TDerived : TBase, new()
        {
            public TDerived GetInstance()
            {
                return new TDerived();
            }
        }

        [Test]
        public void TypeParameterConstraint_ConstrainMultipleParameters()
        {
            var generic = new GenericWithConstraintsOnMultipleParameters<Exception, DateTime>();
            // compile error: class string has no public parameterless constructor
            // var generic2 = new GenericWithConstraintsOnMultipleParameters<string, Exception>();
            // compile error: DateTime is not a reference type
            // var generic3 = new GenericWithConstraintsOnMultipleParameters<DateTime, DateTime>();

            Exception firstInstance = generic.GetFirstInstance();
            DateTime secondInstance = generic.GetSecondInstance();

            Assert.IsInstanceOf<Exception>(firstInstance);
            Assert.IsInstanceOf<DateTime>(secondInstance);
        }


        class GenericWithConstraintsOnMultipleParameters<T1, T2> 
            where T1 : class, new() 
            where T2 : new()
        {
            public T1 GetFirstInstance()
            {
                return new T1();
            }

            public T2 GetSecondInstance()
            {
                return new T2();
            }
        }
    }
}
