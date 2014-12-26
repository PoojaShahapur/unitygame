using SDK.Common;

namespace Game.Msg
{
    public class stPropertyUserCmd : stNullUserCmd
    {
        public const byte REMOVEUSEROBJECT_PROPERTY_USERCMD_PARAMETER = 2;
        public const byte REFCOUNTOBJECT_PROPERTY_USERCMD_PARAMETER = 6;
        public const byte USEUSEROBJECT_PROPERTY_USERCMD_PARAMETER = 7;
        public const byte ADDUSER_MOBJECT_LIST_PROPERTY_USERCMD_PARAMETER = 42;
        public const byte ADDUSER_MOBJECT_PROPERTY_USERCMD_PARAMETER = 43;
        public const byte REQ_BUY_MARKET_MOBILE_OBJECT_CMD = 44;

        public stPropertyUserCmd()
        {
            byCmd = PROPERTY_USERCMD;
        }
    }

    //struct stPropertyUserCmd : public stNullUserCmd
    //{
    //    stPropertyUserCmd()
    //    {
    //        byCmd = PROPERTY_USERCMD;
    //    }
    //};
}