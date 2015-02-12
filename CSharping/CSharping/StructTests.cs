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
            var data = new Data(1, "Bob");

            Assert.AreEqual(1, data.Id);
            Assert.AreEqual("Bob", data.Value);
        }

        struct Data
        {
            private readonly int _id;
            private string _value;
            public const string Name = "DataStruct"; // can only have const or static field initialisers
            
            // cannot have an explicit parameterless constructor

            public Data(int id, string value)
            {
                _id = id;
                _value = value;
            }

            public int Id
            {
                get { return _id; }
            }

            public string Value
            {
                get { return _value; }
                set { _value = value; }
            }
        }
    }
}
