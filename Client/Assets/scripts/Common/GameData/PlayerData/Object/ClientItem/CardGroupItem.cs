using Game.Msg;
using Game.UI;
using SDK.Lib;
using System.Collections.Generic;

namespace SDK.Common
{
    public class CardGroupItem
    {
        public bool m_canReqCardPerGroup = true;       // 是否可以请求每一组中卡牌列表
        public t_group_list m_cardGroup;
        public List<uint> m_cardList;

        //public int id;              // 卡牌组 ID -- m_cardGroup.index
        //public string name;         // 卡牌组名字 -- m_cardGroup.name
        //public CardClass classs = CardClass.kwarrior;    // 卡牌组类型 -- m_cardGroup.occupation
        //public string cards;        // 自己携带的卡牌 -- m_cardList 这里面是 ID

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

        public void copyFrom(CardGroupItem rhv)
        {
            //this.id = rhv.id;
            //this.name = rhv.name;
            //this.classs = rhv.classs;
            //this.cards = rhv.cards;

            if(this.m_cardList == null)
            {
                this.m_cardList = new List<uint>();
            }
            this.m_cardList.Clear();
            if (rhv.m_cardList != null)
            {
                this.m_cardList.AddRange(rhv.m_cardList);
            }

            //if(m_cardGroup != null)
            //{
            //    m_cardGroup = new t_group_list();
            //}

            if (rhv.m_cardGroup != null)
            {
                if (m_cardGroup == null)
                {
                    m_cardGroup = new t_group_list();
                }
                m_cardGroup.copyFrom(rhv.m_cardGroup);
            }
        }
    }
}