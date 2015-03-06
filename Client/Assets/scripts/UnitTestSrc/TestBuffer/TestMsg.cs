using SDK.Common;
using Game.Msg;

namespace UnitTestSrc
{
    public class UnitTestCmd : stLogonUserCmd
    {
        public uint reserve;
        public uint version;

        public string testStr = "这个是测试数据包加密和解密使用的工具";

        public UnitTestCmd()
        {
            byParam = USER_VERIFY_VER_PARA;
            version = (int)CVMsg.GAME_VERSION;
            reserve = 0;
        }

        public override void serialize(IByteArray ba)
        {
            base.serialize(ba);
            ba.writeUnsignedInt(reserve);
            ba.writeUnsignedInt(version);

            ba.writeMultiByte(testStr, GkEncode.UTF8, 100);
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);
            reserve = ba.readUnsignedInt();
            version = ba.readUnsignedInt();

            testStr = ba.readMultiByte(100, GkEncode.UTF8);
        }
    }
}