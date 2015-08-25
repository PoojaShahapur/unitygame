using SDK.Lib;
using SDK.Lib;

namespace Game.Msg
{
    public class stUserVerifyVerCmd : stLogonUserCmd
	{
        public uint reserve;
        public uint version;
        public stPasswdLogonUserCmd cmd;

        public stUserVerifyVerCmd()
        {
            byParam = USER_VERIFY_VER_PARA;
            version = (int)CVMsg.GAME_VERSION;
            reserve = 0;
            cmd = new stPasswdLogonUserCmd();
        }

        public override void serialize(ByteBuffer ba)
        {
            base.serialize(ba);
            ba.writeUnsignedInt32(reserve);
            ba.writeUnsignedInt32(version);

            cmd.serialize(ba);
        }

        public override void derialize(ByteBuffer ba)
        {
            base.derialize(ba);
            ba.readUnsignedInt32(ref reserve);
            ba.readUnsignedInt32(ref version);

            cmd.derialize(ba);
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