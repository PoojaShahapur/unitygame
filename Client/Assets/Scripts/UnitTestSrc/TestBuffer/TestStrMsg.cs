using SDK.Common;
using Game.Msg;
using SDK.Lib;

namespace UnitTestSrc
{
    public class UnitTestStrCmd : stLogonUserCmd
    {
        public uint reserve;
        public uint version;

        public string testStr = "����ǲ������ݰ����ܺͽ���ʹ�õĹ���";

        public UnitTestStrCmd()
        {
            byParam = USER_VERIFY_VER_PARA;
            version = (int)CVMsg.GAME_VERSION;
            reserve = 0;
        }

        public override void serialize(ByteBuffer ba)
        {
            base.serialize(ba);
            ba.writeUnsignedInt32(reserve);
            ba.writeUnsignedInt32(version);

            ba.writeMultiByte(testStr, GkEncode.UTF8, 100);
        }

        public override void derialize(ByteBuffer ba)
        {
            base.derialize(ba);
            ba.readUnsignedInt32(ref reserve);
            ba.readUnsignedInt32(ref version);

            ba.readMultiByte(ref testStr, 100, GkEncode.UTF8);
        }
    }
}