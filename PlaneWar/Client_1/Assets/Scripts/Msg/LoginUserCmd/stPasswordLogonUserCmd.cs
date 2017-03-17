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

        public override void serialize(ByteBuffer bu)
        {
            base.serialize(bu);
            bu.writeMultiByte(strName, GkEncode.eUTF8, ProtoCV.MAX_ACCNAMESIZE);
            bu.writeMultiByte(strPassword, GkEncode.eUTF8, ProtoCV.MAX_PASSWORD);
            bu.writeMultiByte(strNewPassword, GkEncode.eUTF8, ProtoCV.MAX_PASSWORD);
        }

        //public override void derialize(ByteBuffer bu)
        //{
        //    base.derialize(bu);
        //    strName = bu.readMultiByte(CVMsg.MAX_ACCNAMESIZE, GkEncode.UTF8);
        //    strPassword = bu.readMultiByte(CVMsg.MAX_PASSWORD, GkEncode.UTF8);
        //    strNewPassword = bu.readMultiByte(CVMsg.MAX_PASSWORD, GkEncode.UTF8);
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