using SDK.Lib;
using SDK.Lib;

namespace Game.Msg
{
    public class stLogonUserCmd : stNullUserCmd
	{
        public const byte USER_REQUEST_LOGIN_PARA = 2;
        public const byte SERVER_RETURN_LOGIN_FAILED = 3;
        public const byte SERVER_RETURN_LOGIN_OK = 4;
        public const byte PASSWD_LOGON_USERCMD_PARA = 5;
        public const byte PASSWORD_LOGON_USERCMD_PARA = 9;
        public const byte REQUEST_CLIENT_IP_PARA = 15;
        public const byte RETURN_CLIENT_IP_PARA = 16;
        public const byte USER_VERIFY_VER_PARA = 120;

		public stLogonUserCmd ()
		{
            byCmd = LOGON_USERCMD;
		}
	}
}


//struct stLogonUserCmd : public stNullUserCmd
//{
//    stLogonUserCmd()
//    {
//        byCmd = LOGON_USERCMD;
//    }
//};