using SDK.Common;
using SDK.Lib;

namespace Game.Msg
{
    public class stUserVerifyVerCmd : stLogonUserCmd
	{
        public uint reserve;
        public uint version;

        public stUserVerifyVerCmd()
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
        }

        public override void derialize(ByteBuffer ba)
        {
            base.derialize(ba);
            ba.readUnsignedInt32(ref reserve);
            ba.readUnsignedInt32(ref version);
        }
	}
}

/// 客户端验证版本
//const BYTE USER_VERIFY_VER_PARA = 120;
//const DWORD GAME_VERSION = 1999;
//struct stUserVerifyVerCmd  : public stLogonUserCmd
//{
//    stUserVerifyVerCmd()
//    {
//        byParam = USER_VERIFY_VER_PARA;
//        version = GAME_VERSION;
//        reserve = 0;
//    }
//    DWORD reserve;
//    DWORD version;
//};