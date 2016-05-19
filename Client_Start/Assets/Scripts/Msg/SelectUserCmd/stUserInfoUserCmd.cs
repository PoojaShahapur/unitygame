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

        public override void derialize(ByteBuffer bu)
        {
            base.derialize(bu);

            charInfo = new SelectUserInfo[ProtoCV.MAX_CHARINFO];
            int idx = 0;
            while(idx < ProtoCV.MAX_CHARINFO)
            {
                charInfo[idx].derialize(bu);
                ++idx;
            }

            bu.readUnsignedInt16(ref size);
            if(size > 0)
            {
                bu.readMultiByte(ref data, size, GkEncode.UTF8);
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