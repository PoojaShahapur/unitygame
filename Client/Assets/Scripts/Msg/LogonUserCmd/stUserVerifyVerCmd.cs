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
            version = (int)ProtoCV.GAME_VERSION;
            reserve = 0;
            cmd = new stPasswdLogonUserCmd();
        }

        public override void serialize(ByteBuffer bu)
        {
            base.serialize(bu);
            bu.writeUnsignedInt32(reserve);
            bu.writeUnsignedInt32(version);

            cmd.serialize(bu);
        }

        public override void derialize(ByteBuffer bu)
        {
            base.derialize(bu);
            bu.readUnsignedInt32(ref reserve);
            bu.readUnsignedInt32(ref version);

            cmd.derialize(bu);
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