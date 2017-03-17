using SDK.Lib;

namespace Game.Msg
{
    public class stDataUserCmd : stNullUserCmd
    {
        public const byte MAIN_USER_DATA_USERCMD_PARA = 2;
        public const byte MERGE_VERSION_CHECK_USERCMD_PARA = 53;

        public stDataUserCmd()
        {
            byCmd = DATA_USERCMD;
        }
    }
}


//struct stDataUserCmd : public stNullUserCmd
//{
//  stDataUserCmd()
//  {
//    byCmd = DATA_USERCMD;
//  }
//};