using SDK.Common;
using Game.Msg;
using SDK.Lib;

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
            version = (int)CVMsg.GAME_VERSION;
            reserve = 0;
        }

        public override void serialize(ByteArray ba)
        {
            base.serialize(ba);
            ba.writeUnsignedInt(reserve);
            ba.writeUnsignedInt(version);

            ba.writeMultiByte(testStr, GkEncode.UTF8, 100);
        }

        public override void derialize(ByteArray ba)
        {
            base.derialize(ba);
            reserve = ba.readUnsignedInt();
            version = ba.readUnsignedInt();

            testStr = ba.readMultiByte(100, GkEncode.UTF8);
        }
    }
}