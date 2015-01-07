using Game.Msg;
using System.Collections.Generic;

namespace SDK.Common
{
    /**
     * @brief 卡牌数据
     */
    public class DataCard
    {
        public bool m_canReqData = true;            // 请求所有的卡牌数据
        public bool m_canReqCardGroup = true;       // 是否可以请求卡牌组列表

        // 分职业卡牌
        public List<CardItemBase>[] m_cardListArr = new List<CardItemBase>[(int)EnPlayerCareer.ePCTotal];      // 每一个职业一个列表
        public Dictionary<uint, CardItemBase> m_id2CardDic = new Dictionary<uint, CardItemBase>();

        // 每一个卡牌组
        public List<CardGroupItem> m_cardGroupListArr = new List<CardGroupItem>();      // 每一个职业一个列表
        public Dictionary<uint, CardGroupItem> m_id2CardGroupDic = new Dictionary<uint, CardGroupItem>();

        public DataCard()
        {
            int idx = 0;
            while(idx < (int)EnPlayerCareer.ePCTotal)
            {
                m_cardListArr[idx] = new List<CardItemBase>();
                ++idx;
            }
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

        public void reqCardGroup()
        {
            if (m_canReqCardGroup)
            {
                m_canReqCardGroup = false;
                stReqCardGroupListInfoUserCmd cmd = new stReqCardGroupListInfoUserCmd();
                UtilMsg.sendMsg(cmd);
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

        public void psstRetCardGroupListInfoUserCmd(List<t_group_list> info)
        {
            CardGroupItem item;
            foreach (var itemlist in info)
            {
                item = new CardGroupItem();
                item.m_cardGroup = itemlist;

                m_cardGroupListArr.Add(item);
                m_id2CardGroupDic[item.m_cardGroup.index] = item;
            }
        }

        public void psstRetOneCardGroupInfoUserCmd(stRetOneCardGroupInfoUserCmd msg)
        {
            if(m_id2CardGroupDic.ContainsKey(msg.index))
            {
                m_id2CardGroupDic[msg.index].m_cardList = msg.id;
            }
        }

        public void psstRetCreateOneCardGroupUserCmd(stRetCreateOneCardGroupUserCmd msg)
        {
            CardGroupItem item = new CardGroupItem();
            item.m_cardGroup = new t_group_list();
            item.m_cardGroup.occupation = msg.occupation;
            item.m_cardGroup.index = msg.index;
            item.m_cardGroup.name = msg.name;

            m_cardGroupListArr.Add(item);
            m_id2CardGroupDic[item.m_cardGroup.index] = item;
        }

        public void psstRetDeleteOneCardGroupUserCmd(uint index)
        {
            m_cardGroupListArr.Remove(m_id2CardGroupDic[index]);
            m_id2CardGroupDic.Remove(index);
        }
    }
}