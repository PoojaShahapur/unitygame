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
        public const byte NOFITY_MARKET_ALL_OBJECT_CMD = 45;
        public const byte REQ_MARKET_OBJECT_INFO_CMD = 46;
        public const byte REQ_USER_BASE_DATA_INFO_CMD = 47;

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

    public class stHeroCardCmd : stNullUserCmd
    {
        public const byte NOFITY_ALL_CARD_TUJIAN_INFO_CMD = 1;
        public const byte NOFITY_ONE_CARD_TUJIAN_INFO_CMD = 2;
        public const byte RET_GIFTBAG_CARDS_DATA_CMD = 3;
        public const byte REQ_ALL_CARD_TUJIAN_DATA_CMD = 4;

        public const byte REQ_CARD_GROUP_LIST_INFO_CMD = 5;
        public const byte RET_CARD_GROUP_LIST_INFO_CMD = 6;
        public const byte REQ_ONE_CARD_GROUP_INFO_CMD = 7;
        public const byte RET_ONE_CARD_GROUP_INFO_CMD = 8;
        public const byte REQ_CREATE_ONE_CARD_GROUP_CMD = 9;
        public const byte REQ_SAVE_ONE_CARD_GROUP_CMD = 10;
        public const byte RET_CREATE_ONE_CARD_GROUP_CMD = 11;
        public const byte REQ_DELETE_ONE_CARD_GROUP_CMD = 12;
        public const byte RET_DELETE_ONE_CARD_GROUP_CMD = 13;
        public const byte RET_SAVE_ONE_CARD_GROUP_CMD = 14;

        public const byte REQ_ALL_HERO_INFO_CMD = 15;
        public const byte RET_ALL_HERO_INFO_CMD = 16;
        public const byte RET_ONE_HERO_INFO_CMD = 17;
        public const byte REQ_HERO_FIGHT_MATCH_CMD = 18;
        public const byte RET_HERO_FIGHT_MATCH_CMD = 19;

        public const byte RET_LEFT_CARDLIB_NUM_CMD = 20;
        public const byte RET_MAGIC_POINT_INFO_CMD = 21;
        public const byte REQ_END_MY_ROUND_CMD = 22;

        public stHeroCardCmd()
        {
            byCmd = HERO_CARD_USERCMD;
        }
    }

    //const BYTE HERO_CARD_USERCMD    = 162;
    //struct stHeroCardCmd : public stNullUserCmd
    //{
    //    stHeroCardCmd()
    //    {
    //        byCmd = HERO_CARD_USERCMD;
    //    }
    //};
}