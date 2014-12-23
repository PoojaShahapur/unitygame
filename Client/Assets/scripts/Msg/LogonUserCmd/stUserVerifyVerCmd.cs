using SDK.Common;

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

        public override void serialize(IByteArray ba)
        {
            base.serialize(ba);
            ba.writeUnsignedInt(reserve);
            ba.writeUnsignedInt(version);
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);
            reserve = ba.readUnsignedInt();
            version = ba.readUnsignedInt();
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