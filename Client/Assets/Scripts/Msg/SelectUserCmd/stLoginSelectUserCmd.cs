using SDK.Lib;

namespace Game.Msg
{
    public class stLoginSelectUserCmd : stSelectUserCmd
    {
        public uint charNo;

        public stLoginSelectUserCmd()
        {
            byParam = LOGIN_SELECT_USERCMD_PARA;
        }

        public override void serialize(ByteBuffer bu)
        {
            base.serialize(bu);
            charNo = 0;
            bu.writeUnsignedInt32(charNo);
        }
    }
}


/// 进入游戏
//const BYTE LOGIN_SELECT_USERCMD_PARA = 3;
//struct stLoginSelectUserCmd : public stSelectUserCmd
//{
//  stLoginSelectUserCmd()
//  {
//    byParam = LOGIN_SELECT_USERCMD_PARA;
//  }

//  DWORD charNo;
//};