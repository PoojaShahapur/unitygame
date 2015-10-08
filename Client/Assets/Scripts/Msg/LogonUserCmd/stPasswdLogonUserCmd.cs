using SDK.Lib;

namespace Game.Msg
{
    public class stPasswdLogonUserCmd : stLogonUserCmd
    {
        public uint loginTempID;
        public uint dwUserID;
        public string pstrName;
        public string pstrPassword;
        public uint reserve;
        public uint version;

        public stPasswdLogonUserCmd()
        {
            byParam = PASSWD_LOGON_USERCMD_PARA;
        }

        public override void serialize(ByteBuffer bu)
        {
            base.serialize(bu);

            bu.writeUnsignedInt32(loginTempID);
            bu.writeUnsignedInt32(dwUserID);
            bu.writeMultiByte(pstrName, GkEncode.UTF8, ProtoCV.MAX_ACCNAMESIZE);
            bu.writeMultiByte(pstrPassword, GkEncode.UTF8, ProtoCV.MAX_PASSWORD);

            bu.writeUnsignedInt32(reserve);
            bu.writeUnsignedInt32(version);
        }
    }
}

/// 客户登陆网关服务器发送账号和密码
//const BYTE PASSWD_LOGON_USERCMD_PARA = 5; 
//struct stPasswdLogonUserCmd : public stLogonUserCmd
//{
//  stPasswdLogonUserCmd()
//  {
//    byParam = PASSWD_LOGON_USERCMD_PARA;
//  }
//  DWORD loginTempID;
//  DWORD dwUserID;
//  char pstrName[MAX_ACCNAMESIZE];    /**< 甯愬彿 */
//  char pstrPassword[MAX_PASSWORD];
//  DWORD reserve;
//  DWORD version;
//};