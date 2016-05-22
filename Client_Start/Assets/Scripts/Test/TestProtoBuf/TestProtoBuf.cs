using msg;
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
            string content = ProtobufUtil.SerializeTString<Person>(pSource);

            Person pResult = ProtobufUtil.DeSerializeFString<Person>(content);
        }

        public void testProtoBufBytes()
        {
            Person pSource = new Person();
            pSource.name = "asdf";
            pSource.id = 123;
            pSource.email = "qwer";

            byte[] bytes = ProtobufUtil.SerializeTBytes<Person>(pSource);
            Person pFBytes = ProtobufUtil.DeSerializeFBytes<Person>(bytes);
        }

        public void testNet()
        {
            MSG_ReqTest pSource = new MSG_ReqTest();
            pSource.requid = 123;
            pSource.reqguid = 456;
            pSource.reqaccount = "qwer";

            byte[] bytes = ProtobufUtil.SerializeTBytes<MSG_ReqTest>(pSource);
            Ctx.m_instance.m_luaSystem.receiveToLua(bytes);
        }
    }
}