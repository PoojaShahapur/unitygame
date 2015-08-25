using SDK.Lib;
using SDK.Lib;

namespace Game.Msg
{
    public class stPasswordLogonUserCmd : stLogonUserCmd
    {
        public string strName;
        public string strPassword;
        public string strNewPassword;

        public stPasswordLogonUserCmd()
        {
            byParam = PASSWORD_LOGON_USERCMD_PARA;
        }

        public override void serialize(ByteBuffer ba)
        {
            base.serialize(ba);
            ba.writeMultiByte(strName, GkEncode.UTF8, CVMsg.MAX_ACCNAMESIZE);
            ba.writeMultiByte(strPassword, GkEncode.UTF8, CVMsg.MAX_PASSWORD);
            ba.writeMultiByte(strNewPassword, GkEncode.UTF8, CVMsg.MAX_PASSWORD);
        }

        //public override void derialize(ByteBuffer ba)
        //{
        //    base.derialize(ba);
        //    strName = ba.readMultiByte(CVMsg.MAX_ACCNAMESIZE, GkEncode.UTF8);
        //    strPassword = ba.readMultiByte(CVMsg.MAX_PASSWORD, GkEncode.UTF8);
        //    strNewPassword = ba.readMultiByte(CVMsg.MAX_PASSWORD, GkEncode.UTF8);
        //}
    }
}


/// 请求更改密码
//const BYTE PASSWORD_LOGON_USERCMD_PARA = 9;
//  struct stPasswordLogonUserCmd : public stLogonUserCmd {
//    stPasswordLogonUserCmd()
//    {
//      byParam = PASSWORD_LOGON_USERCMD_PARA;
//    }

//    char strName[MAX_ACCNAMESIZE];
//    char strPassword[MAX_PASSWORD];
//    char strNewPassword[MAX_PASSWORD];
//  };