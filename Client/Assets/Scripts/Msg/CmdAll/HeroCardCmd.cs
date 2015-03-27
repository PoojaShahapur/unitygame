using SDK.Common;
using SDK.Lib;
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

        public override void derialize(ByteArray ba)
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

        public override void derialize(ByteArray ba)
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

        public override void derialize(ByteArray ba)
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

        public override void derialize(ByteArray ba)
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

        public override void serialize(ByteArray ba)
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

        public override void derialize(ByteArray ba)
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

        public override void serialize(ByteArray ba)
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

        public override void serialize(ByteArray ba)
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

         public override void derialize(ByteArray ba)
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

        public override void serialize(ByteArray ba)
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

        public override void derialize(ByteArray ba)
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

        public override void derialize(ByteArray ba)
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

        public override void derialize(ByteArray ba)
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

        public override void derialize(ByteArray ba)
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

        public override void serialize(ByteArray ba)
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

        public override void derialize(ByteArray ba)
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

        public override void derialize(ByteArray ba)
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

        public override void derialize(ByteArray ba)
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

        public override void derialize(ByteArray ba)
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

        public override void derialize(ByteArray ba)
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

    //public enum EnAddCardActionType
    //{
    //    CARD_DATA_NONE,
    //    CARD_DATA_ADD	= 1,	//添加
    //    CARD_DATA_REFRESH	= 2,	//刷新
    //};

    public class stAddBattleCardPropertyUserCmd : stHeroCardCmd
    {
        public byte slot;	    //哪个槽
        public byte who;	    //1,自己 2,对方
        public byte byActionType;  // 1 添加 2 刷新，定义见 EnAddCardActionType
        public t_Card mobject;

        public byte attackType;    //攻击类型
        // 只有攻击刷新属性的时候
        public uint pAttThisID;   //攻击者
        public uint pDefThisID;   //防御者

        public stAddBattleCardPropertyUserCmd()
        {
            byParam = ADD_BATTLE_CARD_PROPERTY_CMD;
        }

        public override void derialize(ByteArray ba)
        {
            base.derialize(ba);

            slot = ba.readUnsignedByte();
            who = ba.readUnsignedByte();
            byActionType = ba.readUnsignedByte();
            mobject = new t_Card();
            mobject.derialize(ba);

            attackType = ba.readUnsignedByte();
            pAttThisID = ba.readUnsignedInt();
            pDefThisID = ba.readUnsignedInt();
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

        public override void derialize(ByteArray ba)
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

        public override void serialize(ByteArray ba)
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
        public stObjectLocation dst;	//目标位置信息
        public byte success;		    //1,成功 0,失败

        public int side;               // 1 自己 2 enemy， 客户端自己使用
        public SceneCardItem m_sceneCardItem;       // 客户端自己使用

        public stRetMoveGameCardUserCmd()
        {
            byParam = RET_MOVE_CARD_USERCMD_PARAMETER;
        }

        public override void derialize(ByteArray ba)
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

        public override void derialize(ByteArray ba)
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

    // 通知自己手里的牌满了
    public class stRetNotifyHandIsFullUserCmd : stHeroCardCmd
    {
        public uint id;
        public byte who;

        public stRetNotifyHandIsFullUserCmd()
        {
            byParam = RET_NOTIFY_HAND_IS_FULL_CMD;
        }

        public override void derialize(ByteArray ba)
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

    // enemy 手里添加一张牌
    public class stAddEnemyHandCardPropertyUserCmd : stHeroCardCmd
    {
        public stAddEnemyHandCardPropertyUserCmd()
        {
            byParam = ADD_ENEMY_HAND_CARD_PROPERTY_CMD;
        }
    }

    //const BYTE ADD_ENEMY_HAND_CARD_PROPERTY_CMD = 33; 
    //struct stAddEnemyHandCardPropertyUserCmd : public stHeroCardCmd
    //{   
    //    stAddEnemyHandCardPropertyUserCmd()
    //    {   
    //        byParam = ADD_ENEMY_HAND_CARD_PROPERTY_CMD;
    //    }   
    //}; 

    // enemy 手里删除一张牌
    public class stDelEnemyHandCardPropertyUserCmd : stHeroCardCmd
    {
        public stDelEnemyHandCardPropertyUserCmd()
        {
            byParam = DEL_ENEMY_HAND_CARD_PROPERTY_CMD;
        }
    }

    //const BYTE DEL_ENEMY_HAND_CARD_PROPERTY_CMD = 40; 
    //struct stDelEnemyHandCardPropertyUserCmd : public stHeroCardCmd
    //{   
    //    stDelEnemyHandCardPropertyUserCmd()
    //    {   
    //        byParam = DEL_ENEMY_HAND_CARD_PROPERTY_CMD;
    //    }   
    //}; 

    // 通知客户端最后一场战斗没有结束
    public class stRetNotifyUnfinishedGameUserCmd : stHeroCardCmd
    {
        public stRetNotifyUnfinishedGameUserCmd()
        {
            byParam = RET_NOTIFY_UNFINISHED_GAME_CMD;
        }
    }

    //const BYTE RET_NOTIFY_UNFINISHED_GAME_CMD = 34; 
    //struct stRetNotifyUnfinishedGameUserCmd : public stHeroCardCmd
    //{   
    //    stRetNotifyUnfinishedGameUserCmd()
    //    {   
    //        byParam = RET_NOTIFY_UNFINISHED_GAME_CMD;
    //    }   
    //};  

    public class stReqEnterUnfinishedGameUserCmd : stHeroCardCmd
    {
        public stReqEnterUnfinishedGameUserCmd()
        {
            byParam = REQ_ENTER_UNFINISHED_GAME_CMD;
        }
    }

    //const BYTE REQ_ENTER_UNFINISHED_GAME_CMD = 35; 
    //struct stReqEnterUnfinishedGameUserCmd : public stHeroCardCmd
    //{   
    //    stReqEnterUnfinishedGameUserCmd()
    //    {   
    //        byParam = REQ_ENTER_UNFINISHED_GAME_CMD;
    //    }   
    //}; 

    public class CardListItem
    {
        public byte who;
        public t_Card mobject;

        public void derialize(ByteArray ba)
        {
            who = ba.readUnsignedByte();
            mobject = new t_Card();
            mobject.derialize(ba);
        }
    }

    //    struct CardListItem
    //    {   
    //        BYTE who;
    //        t_Card object;
    //    };

    public class stAddBattleCardListPropertyUserCmd : stHeroCardCmd
    {
        public ushort count;
        public List<CardListItem> list;

        public stAddBattleCardListPropertyUserCmd()
        {
            byParam = ADD_BATTLE_CARD_LIST_PROPERTY_CMD;
        }

        public override void derialize(ByteArray ba)
        {
 	        base.derialize(ba);

            count = ba.readUnsignedShort();
            list = new List<CardListItem>();
            CardListItem item;
            int idx = 0;
            while(idx < count)
            {
                item = new CardListItem();
                list.Add(item);
                item.derialize(ba);
                ++idx;
            }
        }
    }

    //const BYTE ADD_BATTLE_CARD_LIST_PROPERTY_CMD = 36; 
    //struct stAddBattleCardListPropertyUserCmd : public stHeroCardCmd
    //{   
    //    stAddBattleCardListPropertyUserCmd()
    //    {   
    //        byParam = ADD_BATTLE_CARD_LIST_PROPERTY_CMD;
    //        count = 0;
    //    }   
    //    WORD count;
    //    struct 
    //    {   
    //        BYTE who;
    //        t_Card object;
    //    }list[0];
    //}; 

    public class stRetEnemyHandCardNumUserCmd : stHeroCardCmd
    {
        public ushort count;

        public stRetEnemyHandCardNumUserCmd()
        {
            byParam = RET_ENEMY_HAND_CARD_NUM_CMD;
        }

        public override void derialize(ByteArray ba)
        {
            base.derialize(ba);

            count = ba.readUnsignedShort();
        }
    }

    //const BYTE RET_ENEMY_HAND_CARD_NUM_CMD = 37; 
    //struct stRetEnemyHandCardNumUserCmd : public stHeroCardCmd
    //{   
    //    stRetEnemyHandCardNumUserCmd()
    //    {   
    //        byParam = RET_ENEMY_HAND_CARD_NUM_CMD;
    //        count = 0;
    //    }   
    //    WORD count;
    //};

    public class stCardAttackMagicUserCmd : stHeroCardCmd
    {
        public uint dwAttThisID;
        public uint dwDefThisID;
        public uint dwMagicType;	//技能ID

        public stCardAttackMagicUserCmd()
        {
            byParam = REQ_CARD_MAGIC_USERCMD_PARA;
        }

        public override void serialize(ByteArray ba)
        {
            base.serialize(ba);

            ba.writeUnsignedInt(dwAttThisID);
            ba.writeUnsignedInt(dwDefThisID);
            ba.writeUnsignedInt(dwMagicType);
        }
    }

    //const BYTE REQ_CARD_MAGIC_USERCMD_PARA = 38; 
    //struct stCardAttackMagicUserCmd : public stHeroCardCmd
    //{   
    //    stCardAttackMagicUserCmd()
    //    {   
    //        byParam = REQ_CARD_MAGIC_USERCMD_PARA;
    //        dwAttThisID = 0;
    //        dwDefThisID = 0;
    //    }   

    //    DWORD dwAttThisID;    
    //    DWORD dwDefThisID;    
    //    DWORD dwMagicType;	//技能ID
    //};  

    public class stRetRemoveBattleCardUserCmd : stHeroCardCmd
    {
        public uint dwThisID;

        public stRetRemoveBattleCardUserCmd()
        {
            byParam = RET_REMOVE_BATTLE_CARD_USERCMD;
        }

        public override void derialize(ByteArray ba)
        {
            base.derialize(ba);

            dwThisID = ba.readUnsignedInt();
        }
    }

    //const BYTE RET_REMOVE_BATTLE_CARD_USERCMD = 39; 
    //struct stRetRemoveBattleCardUserCmd : public stHeroCardCmd
    //{   
    //    stRetRemoveBattleCardUserCmd()
    //    {   
    //        byParam = RET_REMOVE_BATTLE_CARD_USERCMD;
    //        dwThisID = 0;
    //    }   
    //    DWORD dwThisID;
    //}; 

    public class stRetRefreshCardAllStateUserCmd: stHeroCardCmd
    {
        public uint dwThisID;
        public byte who;	//1,自己 2,对方
        public byte[] state;

        public stRetRefreshCardAllStateUserCmd()
        {
            byParam = RET_REFRESH_CARD_ALL_STATE_CMD;
        }

        public override void derialize(ByteArray ba)
        {
            base.derialize(ba);

            dwThisID = ba.readUnsignedInt();
            who = ba.readUnsignedByte();
            state = ba.readBytes((((int)StateID.CARD_STATE_MAX + 7) / 8));
        }
    }

    //const BYTE RET_REFRESH_CARD_ALL_STATE_CMD = 41;
    //struct stRetRefreshCardAllStateUserCmd: public stHeroCardCmd
    //{
    //stRetRefreshCardAllStateUserCmd()
    //{
    //    byParam = RET_REFRESH_CARD_ALL_STATE_CMD;
    //    dwThisID = 0;
    //    who = 0;
    //    bzero(state, sizeof(state));
    //}
    //DWORD dwThisID;
    //BYTE who;	//1,自己 2,对方
    //BYTE state[(CARD_STATE_MAX + 7) / 8];
    //};

    public class stRetClearCardOneStateUserCmd : stHeroCardCmd
    {
        public uint dwThisID;
        public byte who;   //1,自己 2,对方
        public byte stateNum;	    //状态的枚举

        public stRetClearCardOneStateUserCmd()
        {
            byParam = RET_CLEAR_CARD_ONE_STATE_CMD;
        }

        public override void derialize(ByteArray ba)
        {
            base.derialize(ba);

            dwThisID = ba.readUnsignedInt();
            who = ba.readUnsignedByte();
            stateNum = ba.readUnsignedByte();
        }
    }

    //const BYTE RET_CLEAR_CARD_ONE_STATE_CMD = 42;
    //struct stRetClearCardOneStateUserCmd : public stHeroCardCmd
    //{
    //stRetClearCardOneStateUserCmd()
    //{
    //    byParam = RET_CLEAR_CARD_ONE_STATE_CMD;
    //    dwThisID  = 0;
    //    who = 0;
    //    stateNum = 0;
    //}
    //DWORD dwThisID;
    //BYTE who;   //1,自己 2,对方
    //BYTE stateNum;	    //状态的枚举
    //};

    public class stRetSetCardOneStateUserCmd : stHeroCardCmd
    {
        public uint dwThisID;
        public byte who;   //1,自己 2,对方
        public byte stateNum;	    //状态的枚举

        public stRetSetCardOneStateUserCmd()
        {
            byParam = RET_SET_CARD_ONE_STATE_CMD;
        }

        public override void derialize(ByteArray ba)
        {
            base.derialize(ba);

            dwThisID = ba.readUnsignedInt();
            who = ba.readUnsignedByte();
            stateNum = ba.readUnsignedByte();
        }
    }

    //const BYTE RET_SET_CARD_ONE_STATE_CMD = 43;
    //struct stRetSetCardOneStateUserCmd : public stHeroCardCmd
    //{
    //stRetSetCardOneStateUserCmd()
    //{
    //    byParam = RET_SET_CARD_ONE_STATE_CMD;
    //    dwThisID  = 0;
    //    who = 0;
    //    stateNum = 0;
    //}
    //DWORD dwThisID;
    //BYTE who;   //1,自己 2,对方
    //BYTE stateNum;	    //状态的枚举
    //};

    public class stRetBattleGameResultUserCmd : stHeroCardCmd
    {
        public byte win;

        public stRetBattleGameResultUserCmd()
        {
            byParam = RET_BATTLE_GAME_RESULT_CMD;
        }

        public override void derialize(ByteArray ba)
        {
            base.derialize(ba);

            win = ba.readUnsignedByte();
        }
    }

    //const BYTE RET_BATTLE_GAME_RESULT_CMD = 44;
    //struct stRetBattleGameResultUserCmd : public stHeroCardCmd
    //{
    //stRetBattleGameResultUserCmd()
    //{
    //    byParam = RET_BATTLE_GAME_RESULT_CMD;
    //    win = 0;
    //}
    //BYTE win;	    //1赢, 0输
    //};

    public class stRetHeroIntoBattleSceneUserCmd : stHeroCardCmd
    {
        public stRetHeroIntoBattleSceneUserCmd()
        {
            byParam = RET_HERO_INTO_BATTLE_SCENE_CMD;
        } 
    }

    //const BYTE RET_HERO_INTO_BATTLE_SCENE_CMD = 45; 
    //struct stRetHeroIntoBattleSceneUserCmd : public stHeroCardCmd
    //{   
    //    stRetHeroIntoBattleSceneUserCmd()
    //    {   
    //        byParam = RET_HERO_INTO_BATTLE_SCENE_CMD;
    //    }   
    //}; 

    public class stRetCardAttackFailUserCmd : stHeroCardCmd
    {
        public uint dwAttThisID;

        public stRetCardAttackFailUserCmd()
        {
            byParam = RET_CARD_ATTACK_FAIL_USERCMD_PARA;
        }

        public override void derialize(ByteArray ba)
        {
            base.derialize(ba);

            dwAttThisID = ba.readUnsignedInt();
        }

        public override void serialize(ByteArray ba)
        {
            base.serialize(ba);

            ba.writeUnsignedInt(dwAttThisID);
        }
    }

    //const BYTE RET_CARD_ATTACK_FAIL_USERCMD_PARA = 46; 
    //struct stRetCardAttackFailUserCmd : public stHeroCardCmd
    //{   
    //    stRetCardAttackFailUserCmd()
    //    {   
    //        byParam = RET_CARD_ATTACK_FAIL_USERCMD_PARA;
    //        dwAttThisID = 0;
    //    }   
    //    DWORD dwAttThisID;    
    //};

    public class stCardMoveAndAttackMagicUserCmd : stHeroCardCmd
    {
        public uint dwAttThisID;
        public uint dwDefThisID;
        public uint dwMagicType;	//技能ID
        public stObjectLocation  dst;	    //移动目标位置信息

        public stCardMoveAndAttackMagicUserCmd()
        {
            byParam = REQ_CARD_MOVE_AND_MAGIC_USERCMD_PARA;
        }

        public override void derialize(ByteArray ba)
        {
            base.derialize(ba);

            dwAttThisID = ba.readUnsignedInt();
            dwDefThisID = ba.readUnsignedInt();
            dwMagicType = ba.readUnsignedInt();

            dst = new stObjectLocation();
            dst.derialize(ba);
        }
    }

    //const BYTE REQ_CARD_MOVE_AND_MAGIC_USERCMD_PARA = 47;
    //struct stCardMoveAndAttackMagicUserCmd : public stHeroCardCmd
    //{
    //stCardMoveAndAttackMagicUserCmd()
    //{
    //    byParam = REQ_CARD_MOVE_AND_MAGIC_USERCMD_PARA;
    //    dwAttThisID = 0;
    //    dwDefThisID = 0;
    //    dwMagicType = 0;
    //}

    //DWORD dwAttThisID;      
    //DWORD dwDefThisID;   
    //DWORD dwMagicType;	//技能ID
    //stObjectLocation  dst;	    //移动目标位置信息
    //};

    public class stRetBattleHistoryInfoUserCmd : stHeroCardCmd
    {
        public t_Card maincard;
        public byte opType;
        public ushort count;
        public t_Card[] othercard;

        public stRetBattleHistoryInfoUserCmd()
        {
            byParam = RET_BATTLE_HISTORY_INFO_CMD;
        }

        public override void derialize(ByteArray ba)
        {
            base.derialize(ba);

            maincard = new t_Card();
            maincard.derialize(ba);

            opType = ba.readUnsignedByte();
            count = ba.readUnsignedShort();

            int idx = 0;
            othercard = new t_Card[count];
            t_Card cardItem;
            for(; idx < count; ++idx)
            {
                cardItem = new t_Card();
                cardItem.derialize(ba);
                othercard[idx] = cardItem;
            }
        }
    }

    //const BYTE RET_BATTLE_HISTORY_INFO_CMD = 48; 
    //struct stRetBattleHistoryInfoUserCmd : public stHeroCardCmd
    //{   
    //    stRetBattleHistoryInfoUserCmd()
    //    {   
    //        byParam = RET_BATTLE_HISTORY_INFO_CMD;
    //        count = 0;
    //        opType = 0;
    //    }   
    //    t_Card maincard;
    //    BYTE opType;
    //    WORD count;
    //    t_Card othercard[0];
    //};
}