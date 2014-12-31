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
}