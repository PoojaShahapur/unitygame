using Game.Msg;
using Game.UI;
using SDK.Lib;
using System.Collections.Generic;

namespace SDK.Common
{
    public class CardGroupItem
    {
        public bool m_canReqCardPerGroup = true;        // 是否可以请求每一组中卡牌列表
        public t_group_list m_cardGroup;                // 套牌中的一个套牌
        public TableJobItemBody m_tableJobItemBody;     // 表中的数据
        public List<uint> m_cardList;

        public void reqCardList()
        {
            if (m_canReqCardPerGroup)
            {
                m_canReqCardPerGroup = false;
                stReqOneCardGroupInfoUserCmd cmd = new stReqOneCardGroupInfoUserCmd();
                cmd.index = m_cardGroup.index;
                UtilMsg.sendMsg(cmd);
            }
        }

        public int getCardCount()
        {
            if(m_cardList == null)
            {
                return 0;
            }

            return m_cardList.Count;
        }

        public void copyFrom(CardGroupItem rhv)
        {
            if (rhv.m_cardGroup != null)
            {
                if (m_cardGroup == null)
                {
                    m_cardGroup = new t_group_list();
                }
                m_cardGroup.copyFrom(rhv.m_cardGroup);
            }

            this.m_tableJobItemBody = rhv.m_tableJobItemBody;

            if (this.m_cardList == null)
            {
                this.m_cardList = new List<uint>();
            }
            this.m_cardList.Clear();
            if (rhv.m_cardList != null)
            {
                this.m_cardList.AddRange(rhv.m_cardList);
            }
        }
    }
}