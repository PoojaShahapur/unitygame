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
}