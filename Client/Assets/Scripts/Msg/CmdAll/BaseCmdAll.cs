using SDK.Lib;
using SDK.Lib;

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
        public const byte RET_REFRESH_BATTLE_STATE_CMD = 23;

        public const byte RET_REFRESH_BATTLE_PRIVILEGE_CMD = 24;
        public const byte REQ_GIVEUP_ONE_BATTLE_CMD = 25;
        public const byte ADD_BATTLE_CARD_PROPERTY_CMD = 26;
        public const byte NOTIFY_FIGHT_ENEMY_INFO_CMD = 27;
        public const byte REQ_FIGHT_PREPARE_OVER_CMD = 28;
        public const byte RENAME_CARD_GROUP_USERCMD_PARAMETER = 29;
        public const byte RET_FIRST_HAND_CARD_CMD = 30;

        public const byte RET_MOVE_CARD_USERCMD_PARAMETER = 31;
        public const byte RET_NOTIFY_HAND_IS_FULL_CMD = 32;
        public const byte ADD_ENEMY_HAND_CARD_PROPERTY_CMD = 33;
        public const byte RET_NOTIFY_UNFINISHED_GAME_CMD = 34;
        public const byte REQ_ENTER_UNFINISHED_GAME_CMD = 35;
        public const byte ADD_BATTLE_CARD_LIST_PROPERTY_CMD = 36;
        public const byte RET_ENEMY_HAND_CARD_NUM_CMD = 37;
        public const byte REQ_CARD_MAGIC_USERCMD_PARA = 38;

        public const byte RET_REMOVE_BATTLE_CARD_USERCMD = 39;
        public const byte DEL_ENEMY_HAND_CARD_PROPERTY_CMD = 40;
        public const byte RET_REFRESH_CARD_ALL_STATE_CMD = 41;
        public const byte RET_CLEAR_CARD_ONE_STATE_CMD = 42;
        public const byte RET_SET_CARD_ONE_STATE_CMD = 43;

        public const byte RET_BATTLE_GAME_RESULT_CMD = 44;
        public const byte RET_HERO_INTO_BATTLE_SCENE_CMD = 45;
        public const byte RET_CARD_ATTACK_FAIL_USERCMD_PARA = 46;
        public const byte REQ_CARD_MOVE_AND_MAGIC_USERCMD_PARA = 47;
        public const byte RET_BATTLE_HISTORY_INFO_CMD = 48;
        public const byte NOTIFY_BATTLE_CARD_PROPERTY_CMD = 49;
        public const byte NOTIFY_BATTLE_FLOW_START_CMD = 50;
        public const byte NOTIFY_BATTLE_FLOW_END_CMD = 51;
        public const byte NOTIFY_RESET_ATTACKTIMES_CMD = 52;
        public const byte NOTIFY_OUT_CARD_INFO_CMD = 53;

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

    public class stChatUserCmd : stNullUserCmd
    {
        public const byte CHAT_USERCMD_PARAMETER = 2;

        public stChatUserCmd()
        {
            byCmd = CHAT_USERCMD;
        }
    }

    //const BYTE CHAT_USERCMD      = 14;
    //struct stChatUserCmd : public stNullUserCmd
    //{       
    //  stChatUserCmd()
    //  { 
    //    byCmd = CHAT_USERCMD;
    //  } 
    //}; 
}