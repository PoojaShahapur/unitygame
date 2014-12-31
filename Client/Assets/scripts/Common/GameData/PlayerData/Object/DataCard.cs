using Game.Msg;
using System.Collections.Generic;

namespace SDK.Common
{
    /**
     * @brief 卡牌数据
     */
    public class DataCard
    {
        public bool m_canReqData = true;

        public List<CardItemBase>[] m_cardListArr = new List<CardItemBase>[(int)EnPlayerCareer.ePCTotal];      // 每一个职业一个列表
        public Dictionary<uint, CardItemBase> m_id2CardDic = new Dictionary<uint, CardItemBase>();

        public DataCard()
        {
            int idx = 0;
            while(idx < (int)EnPlayerCareer.ePCTotal)
            {
                m_cardListArr[idx] = new List<CardItemBase>();
                ++idx;
            }
        }

        public void psstNotifyAllCardTujianInfoCmd(List<t_Tujian> list)
        {
            CardItemBase item = null;
            int idx = 0;
            while(idx < list.Count)
            {
                item = new CardItemBase();
                item.m_tujian = list[idx];
                item.m_tableItemCard = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, item.m_tujian.id);

                m_cardListArr[(item.m_tableItemCard.m_itemBody as TableCardItemBody).m_career].Add(item);
                m_id2CardDic[list[idx].id] = item;

                ++idx;
            }
        }

        // 新增\数量改变,不包括删除
        public void psstNotifyOneCardTujianInfoCmd(uint id, byte num)
        {
            if (!m_id2CardDic.ContainsKey(id))
            {
                CardItemBase item = new CardItemBase();
                item.m_tujian = new t_Tujian();
                item.m_tujian.id = id;
                item.m_tujian.num = num;
                item.m_tableItemCard = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, item.m_tujian.id);

                m_cardListArr[(item.m_tableItemCard.m_itemBody as TableCardItemBody).m_career].Add(item);
                m_id2CardDic[id] = item;
            }
            m_id2CardDic[id].m_tujian.num = num;
        }

        public void reqAllCard()
        {
            if (m_canReqData)
            {
                m_canReqData = false;
                stReqAllCardTujianDataUserCmd cmd = new stReqAllCardTujianDataUserCmd();
                UtilMsg.sendMsg(cmd);
            }
        }
    }
}