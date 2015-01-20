using SDK.Common;
using System.Collections.Generic;

namespace Game.Msg
{
    public class stNotifyAllCardTujianInfoCmd : stHeroCardCmd
    {
        public ushort count;
        public List<t_Tujian> info;

        public stNotifyAllCardTujianInfoCmd()
        {
            byParam = NOFITY_ALL_CARD_TUJIAN_INFO_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);
            count = ba.readUnsignedShort();

            info = new List<t_Tujian>();
            t_Tujian item;
            int idx = 0;
            while(idx < count)
            {
                item = new t_Tujian();
                item.derialize(ba);
                info.Add(item);
                ++idx;
            }
        }
    }

    //const BYTE NOFITY_ALL_CARD_TUJIAN_INFO_CMD = 1;
    //struct stNotifyAllCardTujianInfoCmd : public stHeroCardCmd
    //{
    //stNotifyAllCardTujianInfoCmd()
    //{
    //    byParam = NOFITY_ALL_CARD_TUJIAN_INFO_CMD;
    //    count = 0;
    //}
    //WORD count;
    //t_Tujian info[0];
    //};

    public class stNotifyOneCardTujianInfoCmd : stHeroCardCmd
    {
        public uint id;
        public byte num;

        public stNotifyOneCardTujianInfoCmd()
        {
            byParam = NOFITY_ONE_CARD_TUJIAN_INFO_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);

            id = ba.readUnsignedInt();
            num = ba.readUnsignedByte();
        }
    }

    //const BYTE NOFITY_ONE_CARD_TUJIAN_INFO_CMD = 2;
    //struct stNotifyOneCardTujianInfoCmd : public stHeroCardCmd
    //{
    //stNotifyOneCardTujianInfoCmd()
    //{
    //    byParam = NOFITY_ONE_CARD_TUJIAN_INFO_CMD;
    //    id = 0;
    //    num = 0;
    //}
    //DWORD id;
    //BYTE num;
    //};

    public class stRetGiftBagCardsDataUserCmd : stHeroCardCmd
    {
        public uint[] id;

        public stRetGiftBagCardsDataUserCmd()
        {
            byParam = RET_GIFTBAG_CARDS_DATA_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);

            id = new uint[5];
            int idx = 0;
            while(idx < 5)
            {
                id[idx] = ba.readUnsignedInt();
                ++idx;
            }
        }
    }

    //const BYTE RET_GIFTBAG_CARDS_DATA_CMD = 3;
    //struct stRetGiftBagCardsDataUserCmd : public stHeroCardCmd
    //{
    //stRetGiftBagCardsDataUserCmd()
    //{
    //    byParam = RET_GIFTBAG_CARDS_DATA_CMD;
    //}
    //DWORD id[5];		    //一个礼包中的5张卡
    //};

    public class stReqAllCardTujianDataUserCmd : stHeroCardCmd
    {
        public stReqAllCardTujianDataUserCmd()
        {
            byParam = REQ_ALL_CARD_TUJIAN_DATA_CMD;
        }
    }
	
    //const BYTE REQ_ALL_CARD_TUJIAN_DATA_CMD = 4;
    //struct stReqAllCardTujianDataUserCmd : public stHeroCardCmd
    //{   
    //    stReqAllCardTujianDataUserCmd()
    //    {   
    //        byParam = REQ_ALL_CARD_TUJIAN_DATA_CMD;
    //    }   
    //};

    public class stReqCardGroupListInfoUserCmd : stHeroCardCmd
    {
        public stReqCardGroupListInfoUserCmd()
        {
            byParam = REQ_CARD_GROUP_LIST_INFO_CMD;
        }
    }

    //const BYTE REQ_CARD_GROUP_LIST_INFO_CMD = 5;
    //struct stReqCardGroupListInfoUserCmd : public stHeroCardCmd
    //{
    //stReqCardGroupListInfoUserCmd()
    //{
    //    byParam = REQ_CARD_GROUP_LIST_INFO_CMD;
    //}
    //};

    public class stRetCardGroupListInfoUserCmd : stHeroCardCmd
    {
        public ushort count;
        public List<t_group_list> info;

        public stRetCardGroupListInfoUserCmd()
        {
            byParam = RET_CARD_GROUP_LIST_INFO_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);
            count = ba.readUnsignedShort();

            info = new List<t_group_list>();
            t_group_list item;
            int idx = 0;
            while (idx < count)
            {
                item = new t_group_list();
                item.derialize(ba);
                info.Add(item);
                ++idx;
            }
        }
    }

    //const BYTE RET_CARD_GROUP_LIST_INFO_CMD = 6;
    //struct stRetCardGroupListInfoUserCmd : public stHeroCardCmd
    //{
    //stRetCardGroupListInfoUserCmd()
    //{
    //    byParam = RET_CARD_GROUP_LIST_INFO_CMD;
    //    count = 0;
    //}
    //WORD count;
    //t_group_list info[0];
    //};

    public class stReqOneCardGroupInfoUserCmd : stHeroCardCmd
    {
        public uint index;

        public stReqOneCardGroupInfoUserCmd()
        {
            byParam = REQ_ONE_CARD_GROUP_INFO_CMD;
        }

        public override void serialize(IByteArray ba)
        {
            base.serialize(ba);

            ba.writeUnsignedInt(index);
        }
    }

    //const BYTE REQ_ONE_CARD_GROUP_INFO_CMD = 7;
    //struct stReqOneCardGroupInfoUserCmd : public stHeroCardCmd
    //{
    //DWORD index;
    //stReqOneCardGroupInfoUserCmd()
    //{
    //    byParam = REQ_ONE_CARD_GROUP_INFO_CMD;
    //}
    //};

    public class stRetOneCardGroupInfoUserCmd : stHeroCardCmd
    {
        public uint index;
        public ushort count;
        public List<uint> id;

        public stRetOneCardGroupInfoUserCmd()
        {
            byParam = RET_ONE_CARD_GROUP_INFO_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);
            index = ba.readUnsignedInt();
            count = ba.readUnsignedShort();

            id = new List<uint>();
            int idx = 0;
            while (idx < count)
            {
                id.Add(ba.readUnsignedInt());
                ++idx;
            }
        }
    }

    //const BYTE RET_ONE_CARD_GROUP_INFO_CMD = 8;
    //struct stRetOneCardGroupInfoUserCmd : public stHeroCardCmd
    //{
    //stRetOneCardGroupInfoUserCmd()
    //{
    //    byParam = RET_ONE_CARD_GROUP_INFO_CMD;
    //    count = 0;
    //}
    //DWORD index;
    //WORD count;
    //DWORD id[0];
    //};

    public class stReqCreateOneCardGroupUserCmd : stHeroCardCmd
    {
        public uint occupation;

        public stReqCreateOneCardGroupUserCmd()
        {
            byParam = REQ_CREATE_ONE_CARD_GROUP_CMD;
        }

        public override void serialize(IByteArray ba)
        {
            base.serialize(ba);
            ba.writeUnsignedInt(occupation);
        }
    }

    //const BYTE REQ_CREATE_ONE_CARD_GROUP_CMD = 9;
    //struct stReqCreateOneCardGroupUserCmd : public stHeroCardCmd
    //{
    //stReqCreateOneCardGroupUserCmd()
    //{
    //    byParam = REQ_CREATE_ONE_CARD_GROUP_CMD;
    //    occupation = 0;
    //}
    //DWORD occupation;	//ְҵ
    //};

    public class stReqSaveOneCardGroupUserCmd : stHeroCardCmd
    {
        public uint index;
        public ushort count;
        public List<uint> id;

        public stReqSaveOneCardGroupUserCmd()
        {
            byParam = REQ_SAVE_ONE_CARD_GROUP_CMD;
        }

        public override void serialize(IByteArray ba)
        {
            base.serialize(ba);
            ba.writeUnsignedInt(index);
            ba.writeUnsignedShort(count);

            if (count > 0)
            {
                int idx = 0;
                while (idx < count)
                {
                    ba.writeUnsignedInt(id[idx]);
                    ++idx;
                }
            }
        }
    }

    //const BYTE REQ_SAVE_ONE_CARD_GROUP_CMD = 10;
    //struct stReqSaveOneCardGroupUserCmd : public stHeroCardCmd
    //{
    //stReqSaveOneCardGroupUserCmd()
    //{
    //    byParam = REQ_SAVE_ONE_CARD_GROUP_CMD;
    //    index = 0;
    //    count = 0;
    //}
    //DWORD index;	//ְҵ
    //WORD count;
    //DWORD id[0];
    //};

    public class stRetCreateOneCardGroupUserCmd : stHeroCardCmd
    {
        public uint occupation;       //职业
        public uint index;
        public string name;

        public stRetCreateOneCardGroupUserCmd()
        {
            byParam = RET_CREATE_ONE_CARD_GROUP_CMD;
        }

         public override void derialize(IByteArray ba)
        {
            base.derialize(ba);
            occupation = ba.readUnsignedInt();
            index = ba.readUnsignedInt();
            name = ba.readMultiByte(CVMsg.MAX_NAMESIZE + 1, GkEncode.GB2312);
        }
    }

    //const BYTE RET_CREATE_ONE_CARD_GROUP_CMD = 11; 
    //struct stRetCreateOneCardGroupUserCmd : public stHeroCardCmd
    //{   
    //    stRetCreateOneCardGroupUserCmd()
    //    {   
    //        byParam = RET_CREATE_ONE_CARD_GROUP_CMD;
    //        occupation = 0;
    //        index = 0;
    //        bzero(name, sizeof(name));
    //    }   
    //    DWORD occupation;       //职业
    //    DWORD index;
    //    char name[MAX_NAMESIZE+1];
    //};

    public class stReqDeleteOneCardGroupUserCmd : stHeroCardCmd
    {
        public uint index;

        public stReqDeleteOneCardGroupUserCmd()
        {
            byParam = REQ_DELETE_ONE_CARD_GROUP_CMD;
        }

        public override void serialize(IByteArray ba)
        {
            base.serialize(ba);
            ba.writeUnsignedInt(index);
        }
    }

    //const BYTE REQ_DELETE_ONE_CARD_GROUP_CMD = 12; 
    //struct stReqDeleteOneCardGroupUserCmd : public stHeroCardCmd
    //{   
    //    stReqDeleteOneCardGroupUserCmd()
    //    {   
    //        byParam = REQ_DELETE_ONE_CARD_GROUP_CMD;
    //        index = 0;
    //    }   
    //    DWORD index;    
    //};  

    public class stRetDeleteOneCardGroupUserCmd : stHeroCardCmd
    {
        public uint index;
        public byte success;

        public stRetDeleteOneCardGroupUserCmd()
        {
            byParam = RET_DELETE_ONE_CARD_GROUP_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);

            index = ba.readUnsignedInt();
            success = ba.readUnsignedByte();
        }
    }

    //const BYTE RET_DELETE_ONE_CARD_GROUP_CMD = 13; 
    //struct stRetDeleteOneCardGroupUserCmd : public stHeroCardCmd
    //{   
    //    stRetDeleteOneCardGroupUserCmd()
    //    {   
    //        byParam = RET_DELETE_ONE_CARD_GROUP_CMD;
    //        index = 0;
    //        success = 0;
    //    }   
    //    DWORD index;    
    //    BYTE success;       //1成功 0失败
    //};  

    public class stRetSaveOneCardGroupUserCmd : stHeroCardCmd
    {
        public uint index;
        public byte success;

        public stRetSaveOneCardGroupUserCmd()
        {
            byParam = RET_SAVE_ONE_CARD_GROUP_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);

            index = ba.readUnsignedInt();
            success = ba.readUnsignedByte();
        }
    }

    //const BYTE RET_SAVE_ONE_CARD_GROUP_CMD = 14; 
    //struct stRetSaveOneCardGroupUserCmd : public stHeroCardCmd
    //{   
    //    stRetSaveOneCardGroupUserCmd()
    //    {   
    //        byParam = RET_SAVE_ONE_CARD_GROUP_CMD;
    //        index = 0;
    //        success = 0;
    //    }   
    //    DWORD index;    
    //    BYTE success;       //1成功 0失败
    //};

    public class stReqAllHeroInfoUserCmd : stHeroCardCmd
    {
        public stReqAllHeroInfoUserCmd()
        {
            byParam = REQ_ALL_HERO_INFO_CMD;
        }
    }
    
    //const BYTE REQ_ALL_HERO_INFO_CMD = 15; 
    //struct stReqAllHeroInfoUserCmd : public stHeroCardCmd
    //{   
    //    stReqAllHeroInfoUserCmd()
    //    {   
    //        byParam = REQ_ALL_HERO_INFO_CMD;
    //    }   
    //};  

    public class stRetAllHeroInfoUserCmd : stHeroCardCmd
    {
        public ushort count;
        public List<t_hero> info;

        public stRetAllHeroInfoUserCmd()
        {
            byParam = RET_ALL_HERO_INFO_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);

            count = ba.readUnsignedShort();
            if(count > 0)
            {
                int idx = 0;
                info = new List<t_hero>();
                t_hero item;
                while(idx < count)
                {
                    item = new t_hero();
                    info.Add(item);
                    item.derialize(ba);
                    ++idx;
                }
            }
        }
    }

    //const BYTE RET_ALL_HERO_INFO_CMD = 16; 
    //struct stRetAllHeroInfoUserCmd : public stHeroCardCmd
    //{   
    //    stRetAllHeroInfoUserCmd()
    //    {   
    //        byParam = RET_ALL_HERO_INFO_CMD;
    //        count = 0;
    //    }   
    //    WORD count;
    //    t_hero info[0];
    //};

    public class stRetOneHeroInfoUserCmd : stHeroCardCmd
    {
        public t_hero info;

        public stRetOneHeroInfoUserCmd()
        {
            byParam = RET_ONE_HERO_INFO_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);

            info.derialize(ba);
        }
    }

    //const BYTE RET_ONE_HERO_INFO_CMD = 17;
    //struct stRetOneHeroInfoUserCmd : public stHeroCardCmd
    //{
    //    stRetOneHeroInfoUserCmd()
    //    {
    //        byParam = RET_ONE_HERO_INFO_CMD;
    //    }
    //    t_hero info;
    //};

    public class stReqHeroFightMatchUserCmd : stHeroCardCmd
    {
        public uint index;
        public byte fightType;      // 当前使用 CHALLENGE_GAME_RELAX_TYPE

        public stReqHeroFightMatchUserCmd()
        {
            byParam = REQ_HERO_FIGHT_MATCH_CMD;
            fightType = (byte)ChallengeGameType.CHALLENGE_GAME_RELAX_TYPE;
        }

        public override void serialize(IByteArray ba)
        {
            base.serialize(ba);

            ba.writeUnsignedInt(index);
            ba.writeByte(fightType);
        }
    }

    //const BYTE REQ_HERO_FIGHT_MATCH_CMD = 18; 
    //struct stReqHeroFightMatchUserCmd : public stHeroCardCmd
    //{   
    //    stReqHeroFightMatchUserCmd()
    //    {   
    //        byParam = REQ_HERO_FIGHT_MATCH_CMD;
    //        index = 0;
    //        fightType = 0;
    //    }   
    //    DWORD index;        //套牌索引
    //    BYTE fightType;     //对战类型
    //};

    public class stRetHeroFightMatchUserCmd : stHeroCardCmd
    {
        public byte fightType;
        public byte success;

        public stRetHeroFightMatchUserCmd()
        {
            byParam = RET_HERO_FIGHT_MATCH_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);

            fightType = ba.readUnsignedByte();
            success = ba.readUnsignedByte();
        }
    }

    //const BYTE RET_HERO_FIGHT_MATCH_CMD = 19; 
    //struct stRetHeroFightMatchUserCmd : public stHeroCardCmd
    //{   
    //    stRetHeroFightMatchUserCmd()
    //    {   
    //        byParam = RET_HERO_FIGHT_MATCH_CMD;
    //        fightType = 0;
    //        success = 0;
    //    }   
    //    BYTE fightType;     //对战类型
    //    BYTE success;           
    //};

    public class stRetLeftCardLibNumUserCmd : stHeroCardCmd
    {
        public uint selfNum;	    //自己剩余	
        public uint otherNum;	    //对方剩余

        public stRetLeftCardLibNumUserCmd()
        {
            byParam = RET_LEFT_CARDLIB_NUM_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);

            selfNum = ba.readUnsignedInt();
            otherNum = ba.readUnsignedInt();
        }
    }

    //const BYTE RET_LEFT_CARDLIB_NUM_CMD = 20;
    //struct stRetLeftCardLibNumUserCmd : public stHeroCardCmd 
    //{
    //stRetLeftCardLibNumUserCmd()
    //{
    //    byParam = RET_LEFT_CARDLIB_NUM_CMD;
    //    selfNum = 0;
    //    otherNum = 0;
    //}
    //DWORD selfNum;	    //自己剩余	
    //DWORD otherNum;	    //对方剩余
    //};

    public class stRetMagicPointInfoUserCmd : stHeroCardCmd
    {
        public t_MagicPoint self;
        public t_MagicPoint other;

        public stRetMagicPointInfoUserCmd()
        {
            byParam = RET_MAGIC_POINT_INFO_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);

            self = new t_MagicPoint();
            self.derialize(ba);
            other = new t_MagicPoint();
            other.derialize(ba);
        }
    }

    //const BYTE RET_MAGIC_POINT_INFO_CMD = 21;
    //struct stRetMagicPointInfoUserCmd : public stHeroCardCmd
    //{
    //stRetMagicPointInfoUserCmd()
    //{
    //    byParam = RET_MAGIC_POINT_INFO_CMD;
    //}
    //t_MagicPoint self;
    //t_MagicPoint other;
    //};

    public class stReqEndMyRoundUserCmd : stHeroCardCmd
    {
        public stReqEndMyRoundUserCmd()
        {
            byParam = REQ_END_MY_ROUND_CMD;
        }
    }

    //const BYTE REQ_END_MY_ROUND_CMD = 22; 
    //struct stReqEndMyRoundUserCmd : public stHeroCardCmd
    //{   
    //    stReqEndMyRoundUserCmd()
    //    {   
    //        byParam = REQ_END_MY_ROUND_CMD;
    //    }   
    //};  

    public class stRetRefreshBattleStateUserCmd : stHeroCardCmd
    {
        public byte state;

        public stRetRefreshBattleStateUserCmd()
        {
            byParam = RET_REFRESH_BATTLE_STATE_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);
            state = ba.readUnsignedByte();
        }
    }

    //const BYTE RET_REFRESH_BATTLE_STATE_CMD = 23;
    //struct stRetRefreshBattleStateUserCmd : public stHeroCardCmd
    //{
    //stRetRefreshBattleStateUserCmd()
    //{
    //    byParam = RET_REFRESH_BATTLE_STATE_CMD;
    //    state = 0;
    //}
    //BYTE state;       // ChallengeState
    //};

    public class stRetRefreshBattlePrivilegeUserCmd : stHeroCardCmd
    {
        public byte priv;

        public stRetRefreshBattlePrivilegeUserCmd()
        {
            byParam = RET_REFRESH_BATTLE_PRIVILEGE_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);

            priv = ba.readUnsignedByte();
        }
    }

    //const BYTE RET_REFRESH_BATTLE_PRIVILEGE_CMD = 24;
    //struct stRetRefreshBattlePrivilegeUserCmd : public stHeroCardCmd
    //{
    //stRetRefreshBattlePrivilegeUserCmd()
    //{
    //    byParam = RET_REFRESH_BATTLE_PRIVILEGE_CMD;
    //    priv = 0;
    //}
    //BYTE priv;	//1,自己 2,对方
    //};

    public class stReqGiveUpOneBattleUserCmd : stHeroCardCmd
    {
        public stReqGiveUpOneBattleUserCmd()
        {
            byParam = REQ_GIVEUP_ONE_BATTLE_CMD;
        }
    }

    //const BYTE REQ_GIVEUP_ONE_BATTLE_CMD = 25;
    //struct stReqGiveUpOneBattleUserCmd : public stHeroCardCmd
    //{
    //stReqGiveUpOneBattleUserCmd()
    //{
    //    byParam = REQ_GIVEUP_ONE_BATTLE_CMD;
    //}
    //};

    public class stAddBattleCardPropertyUserCmd : stHeroCardCmd
    {
        public byte slot;	    //哪个槽
        public byte who;	    //1,自己 2,对方
        public byte byActionType;  //
        public t_Card mobject;	

        public stAddBattleCardPropertyUserCmd()
        {
            byParam = ADD_BATTLE_CARD_PROPERTY_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);

            slot = ba.readUnsignedByte();
            who = ba.readUnsignedByte();
            byActionType = ba.readUnsignedByte();
            mobject = new t_Card();
            mobject.derialize(ba);
        }
    }

    //const BYTE ADD_BATTLE_CARD_PROPERTY_CMD = 26;
    //struct stAddBattleCardPropertyUserCmd : public stHeroCardCmd
    //{
    //stAddBattleCardPropertyUserCmd()
    //{
    //    byParam = ADD_BATTLE_CARD_PROPERTY_CMD;
    //    slot = 0;
    //    who = 0;
    //    byActionType = 0;
    //}
    //BYTE slot;	    //哪个槽
    //BYTE who;	    //1,自己 2,对方
    //BYTE byActionType;  //
    //t_Card object;	    
    //};

    public class stNotifyFightEnemyInfoUserCmd : stHeroCardCmd
    {
        public uint occupation;	    //职业
        public string name;

        public stNotifyFightEnemyInfoUserCmd()
        {
            byParam = NOTIFY_FIGHT_ENEMY_INFO_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);

            occupation = ba.readUnsignedInt();
            name = ba.readMultiByte(CVMsg.MAX_NAMESIZE + 1, GkEncode.UTF8);
        }
    }

    //const BYTE NOTIFY_FIGHT_ENEMY_INFO_CMD = 27;
    //struct stNotifyFightEnemyInfoUserCmd : public stHeroCardCmd
    //{
    //stNotifyFightEnemyInfoUserCmd()
    //{
    //    byParam = NOTIFY_FIGHT_ENEMY_INFO_CMD;
    //    bzero(name, sizeof(name));
    //}
    //char name[MAX_NAMESIZE+1];
    //};

    public class stReqFightPrepareOverUserCmd : stHeroCardCmd
    {
        public stReqFightPrepareOverUserCmd()
        {
            byParam = REQ_FIGHT_PREPARE_OVER_CMD;
        }
    }

    //const BYTE REQ_FIGHT_PREPARE_OVER_CMD = 28;
    //struct stReqFightPrepareOverUserCmd : public stHeroCardCmd
    //{
    //stReqFightPrepareOverUserCmd()
    //{
    //    byParam = REQ_FIGHT_PREPARE_OVER_CMD;
    //}
    //};

    public class stMoveGameCardUserCmd : stHeroCardCmd
    {
        public uint qwThisID;		    //卡牌thisID
	    public stObjectLocation dst;	    //目标位置信息

        public stMoveGameCardUserCmd()
	    {
	        byParam = MOVE_CARD_USERCMD_PARAMETER;
	    }

        public override void serialize(IByteArray ba)
        {
            base.serialize(ba);
            ba.writeUnsignedInt(qwThisID);
            dst.serialize(ba);
        }
    }

    //const BYTE MOVE_CARD_USERCMD_PARAMETER = 29;
    //struct stMoveGameCardUserCmd : public stHeroCardCmd
    //{
    //stMoveGameCardUserCmd()
    //{
    //    byParam = MOVE_CARD_USERCMD_PARAMETER;
    //}
    //DWORD qwThisID;		    //卡牌thisID
    //stObjectLocation  dst;	    //目标位置信息
    //};

    public class stRetMoveGameCardUserCmd : stHeroCardCmd
    {
        public uint qwThisID;		    //卡牌thisID
        public stObjectLocation dst;	    //目标位置信息
        public byte success;		    //1,成功 0,失败

        public stRetMoveGameCardUserCmd()
        {
            byParam = RET_MOVE_CARD_USERCMD_PARAMETER;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);

            qwThisID = ba.readUnsignedInt();
            dst = new stObjectLocation();
            dst.derialize(ba);
            success = ba.readUnsignedByte();
        }
    }

    //const BYTE RET_MOVE_CARD_USERCMD_PARAMETER = 31;
    //struct stRetMoveGameCardUserCmd : public stHeroCardCmd
    //{
    //stRetMoveGameCardUserCmd()
    //{
    //    byParam = RET_MOVE_CARD_USERCMD_PARAMETER;
    //}
    //DWORD qwThisID;		    //卡牌thisID
    //stObjectLocation  dst;	    //目标位置信息
    //BYTE success;		    //1,成功 0,失败
    //};

    public class stRetFirstHandCardUserCmd : stHeroCardCmd
    {
        public byte upperHand;	    //1,先手 0,后手
        public uint[] id;

        public stRetFirstHandCardUserCmd()
        {
            byParam = RET_FIRST_HAND_CARD_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);
            upperHand = ba.readUnsignedByte();

            id = new uint[4];
            int idx = 0;
            while (idx < 4)
            {
                id[idx] = ba.readUnsignedInt();

                ++idx;
            }
        }
    }

    //const BYTE RET_FIRST_HAND_CARD_CMD = 30; 
    //struct stRetFirstHandCardUserCmd : public stHeroCardCmd
    //{   
    //    stRetFirstHandCardUserCmd()
    //    {   
    //        byParam = RET_FIRST_HAND_CARD_CMD;
    //    }   
    //    DWORD id[4];
    //};

    public class stRetNotifyHandIsFullUserCmd : stHeroCardCmd
    {
        public uint id;
        public byte who;

        public stRetNotifyHandIsFullUserCmd()
        {
            byParam = RET_NOTIFY_HAND_IS_FULL_CMD;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);

            id = ba.readUnsignedInt();
            who = ba.readUnsignedByte();
        }
    }

    //const BYTE RET_NOTIFY_HAND_IS_FULL_CMD = 32;
    //struct stRetNotifyHandIsFullUserCmd : public stHeroCardCmd
    //{
    //stRetNotifyHandIsFullUserCmd()
    //{
    //    byParam = RET_NOTIFY_HAND_IS_FULL_CMD;
    //    id = 0;
    //    who = 0;
    //}
    //DWORD id;   // 鍗＄墝琛╥d
    //BYTE who;   // 1,鑷繁 2,瀵规柟
    //};
}