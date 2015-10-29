using MyProtoBuf;
using SDK.Lib;

namespace UnitTest
{
    public class TestProtoBuf
    {
        public void run()
        {
            testProtoBuf();
        }

        public void testProtoBuf()
        {
            Person pSource = new Person();
            pSource.name = "asdf";
            pSource.id = 123;
            pSource.email = "qwer";
            string content = ProtobufHelper.Serialize<Person>(pSource);

            Person pResult = ProtobufHelper.DeSerialize<Person>(content);
        }
    }
}