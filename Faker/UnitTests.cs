using NUnit.Framework;
using NUnit.Framework.Internal;
using NUnit.Framework.Legacy;
using System.Reflection;

namespace Testing
{
    [TestFixture]
    public class UnitTests
    {
        public Faker.IFaker faker;
        [SetUp]
        public void SetUp() { 
            faker = new Faker.Faker();
        }

        public class BasicTypes
        {
            public int int_val;
            public float float_val;
            public double double_val;
            public char char_val;
            public bool bool_val;
            public long long_val;
            public string string_val;
            public DateTime dateTime_val;
        }

        public class Foo
        {
            public BasicTypes temp;
            public string some_val;
            public Foo(BasicTypes temp)
            {
                this.temp = temp;
            }
        }

        public class PrivateConstructor
        {
            private PrivateConstructor(int num)
            {
            }
        }

        public class TestClass
        {
            public List<float> list;
            private long value;
            public List<string> strings;
        }

        public class TestClass2
        {
            public List<TestClass> objs;
        }

        [Test] 
        public void TestOn_BasicTypes_With_Generators() 
        {
            BasicTypes basic = faker.Create<BasicTypes>();
            ClassicAssert.IsTrue(basic.int_val > 0);
            ClassicAssert.IsTrue(basic.double_val > 0);
            ClassicAssert.IsTrue(basic.long_val > 0);
            ClassicAssert.IsTrue(basic.char_val != '\0');
        }

        [Test]
        public void TestOnInnerClassWithConstructor()
        {
            Foo basic = faker.Create<Foo>();
            ClassicAssert.NotNull(basic.temp);
            ClassicAssert.IsTrue(basic.some_val.Length > 2);
        }

        [Test]
        public void TestOnPrivateConstructor()
        {
            PrivateConstructor t = faker.Create<PrivateConstructor>();
            ClassicAssert.IsTrue(t == null);
        }

        [Test]
        public void TestOnListAndPrivateField()
        {
            TestClass t = faker.Create<TestClass>();
            ClassicAssert.IsTrue(t != null);
            ClassicAssert.IsTrue(t.list != null && t.list.Count > 2);
            ClassicAssert.IsTrue(t.strings != null && t.strings[0].Length > 2);
            FieldInfo info = typeof(TestClass).GetField("value", BindingFlags.Instance | BindingFlags.NonPublic);
            ClassicAssert.IsTrue((long)info.GetValue(t) == 0);
        }

        [Test] 
        public void TestOnListOfObjects()
        {
            TestClass2 t = faker.Create<TestClass2>();
            ClassicAssert.IsTrue(t != null);
            ClassicAssert.IsTrue(t.objs.Count > 2);
            ClassicAssert.IsTrue(t.objs[0].strings.Count > 2 && t.objs[0].strings[0].Length > 2);
        }
    }
}
