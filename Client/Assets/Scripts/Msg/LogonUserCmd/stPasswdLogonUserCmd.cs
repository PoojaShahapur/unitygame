using SDK.Common;
using SDK.Lib;

namespace Game.Msg
{
    public class stPasswdLogonUserCmd : stLogonUserCmd
    {
        public uint loginTempID;
        public uint dwUserID;
        public string pstrName;
        public string pstrPassword;

        public stPasswdLogonUserCmd()
        {
            byParam = PASSWD_LOGON_USERCMD_PARA;
        }

        public override void serialize(ByteArray ba)
        {
            base.serialize(ba);

            ba.writeUnsignedInt(loginTempID);
            ba.writeUnsignedInt(dwUserID);
            ba.writeMultiByte(pstrName, GkEncode.UTF8, CVMsg.MAX_ACCNAMESIZE);
            ba.writeMultiByte(pstrPassword, GkEncode.UTF8, CVMsg.MAX_PASSWORD);
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
//  char pstrName[MAX_ACCNAMESIZE];    /**< 帐号 */
//  char pstrPassword[MAX_PASSWORD];
//};