using SDK.Common;
using SDK.Lib;
namespace Game.Msg
{
    public class stUserInfoUserCmd : stSelectUserCmd
    {
        public SelectUserInfo[] charInfo;
        public ushort size;
        public string data;

        public stUserInfoUserCmd()
        {
            byParam = USERINFO_SELECT_USERCMD_PARA;
        }

        public override void derialize(ByteBuffer ba)
        {
            base.derialize(ba);

            charInfo = new SelectUserInfo[CVMsg.MAX_CHARINFO];
            int idx = 0;
            while(idx < CVMsg.MAX_CHARINFO)
            {
                charInfo[idx].derialize(ba);
                ++idx;
            }

            size = ba.readUnsignedInt16();
            if(size > 0)
            {
                data = ba.readMultiByte(size, GkEncode.UTF8);
            }
        }
    }
}


//const BYTE USERINFO_SELECT_USERCMD_PARA = 1;
//struct stUserInfoUserCmd : public stSelectUserCmd
//{
//  stUserInfoUserCmd()
//  {
//    byParam = USERINFO_SELECT_USERCMD_PARA;
//    bzero(charInfo,sizeof(charInfo));
//    size = 0;
//  }
//  SelectUserInfo charInfo[MAX_CHARINFO];
//  WORD size;
//  BYTE data[0];
//};