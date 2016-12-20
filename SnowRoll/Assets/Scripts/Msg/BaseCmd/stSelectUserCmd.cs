using SDK.Lib;

namespace Game.Msg
{
    public class stSelectUserCmd : stNullUserCmd
    {
        public const byte USERINFO_SELECT_USERCMD_PARA = 1;
        public const byte CREATE_SELECT_USERCMD_PARA = 2;
        public const byte LOGIN_SELECT_USERCMD_PARA = 3;
        public const byte LOGIN_SELECT_SUCCESS_USERCMD_PARA = 14;

        public stSelectUserCmd()
        {
            byCmd = SELECT_USERCMD;
        }
    }
}

//struct stSelectUserCmd : public stNullUserCmd
//{
//  stSelectUserCmd()
//  {
//    byCmd = SELECT_USERCMD;
//  }
//};