using MyProtoBuf;
using SDK.Lib;

namespace UnitTest
{
    public class TestProtoBuf
    {
        public void run()
        {
            //testProtoBufStr();
            //testProtoBufBytes();
            testNet();
        }

        public void testProtoBufStr()
        {
            Person pSource = new Person();
            pSource.name = "asdf";
            pSource.id = 123;
            pSource.email = "qwer";
            string content = ProtobufHelper.SerializeTString<Person>(pSource);

            Person pResult = ProtobufHelper.DeSerializeFString<Person>(content);
        }

        public void testProtoBufBytes()
        {
            Person pSource = new Person();
            pSource.name = "asdf";
            pSource.id = 123;
            pSource.email = "qwer";

            byte[] bytes = ProtobufHelper.SerializeTBytes<Person>(pSource);
            Person pFBytes = ProtobufHelper.DeSerializeFBytes<Person>(bytes);
        }

        public void testNet()
        {
            Person pSource = new Person();
            pSource.name = "asdf";
            pSource.id = 123;
            pSource.email = "qwer";

            byte[] bytes = ProtobufHelper.SerializeTBytes<Person>(pSource);
            Ctx.m_instance.m_luaSystem.receiveToLua(bytes);
        }
    }
}