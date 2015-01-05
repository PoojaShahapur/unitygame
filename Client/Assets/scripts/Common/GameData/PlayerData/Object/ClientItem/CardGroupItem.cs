using Game.Msg;
using System.Collections.Generic;

namespace SDK.Common
{
    public class CardGroupItem
    {
        public bool m_canReqCardPerGroup = true;       // 是否可以请求每一组中卡牌列表
        public t_group_list m_cardGroup;
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
    }
}