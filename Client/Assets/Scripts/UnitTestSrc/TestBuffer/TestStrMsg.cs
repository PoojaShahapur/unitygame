using SDK.Lib;
using Game.Msg;

namespace UnitTestSrc
{
    public class UnitTestStrCmd : stLogonUserCmd
    {
        public uint reserve;
        public uint version;

        public string testStr = "这个是测试数据包加密和解密使用的工具";

        public UnitTestStrCmd()
        {
            byParam = USER_VERIFY_VER_PARA;
            version = (int)ProtoCV.GAME_VERSION;
            reserve = 0;
        }

        public override void serialize(ByteBuffer bu)
        {
            base.serialize(bu);
            bu.writeUnsignedInt32(reserve);
            bu.writeUnsignedInt32(version);

            bu.writeMultiByte(testStr, GkEncode.UTF8, 100);
        }

        public override void derialize(ByteBuffer bu)
        {
            base.derialize(bu);
            bu.readUnsignedInt32(ref reserve);
            bu.readUnsignedInt32(ref version);

            bu.readMultiByte(ref testStr, 100, GkEncode.UTF8);
        }
    }
}