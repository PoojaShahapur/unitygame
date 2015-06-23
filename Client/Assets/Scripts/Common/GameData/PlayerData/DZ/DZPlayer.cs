using Game.Msg;
using SDK.Lib;
using System.Collections.Generic;

namespace SDK.Common
{
    /**
     * @brief 对战的一个玩家
     */
    public class DZPlayer
    {
        public EnDZPlayer m_side;               // 玩家属于哪一边的
        public string m_heroName;               // 自己的 Hero 名字
        public uint m_heroOccupation;           // 自己的 Hero 的职业
        public uint[] m_startCardList;          // 第一次自己 Hero 得到的 Card，仅仅有 ID ，注意仅仅第一次报名进入的时候才有，如果是中途下线，然后再上线，这个时候再次进入上一次没有结束的战斗，这个值是没有的
        protected List<SceneCardItem> m_sceneCardList = new List<SceneCardItem>();     // 完成的卡牌数据

        public int m_recStartCardNum;                   // 接收到的开始卡牌的数量，因为启动的时候，仅仅发送过来卡牌 ID，真正的数据后来才发送过来

        public t_MagicPoint m_heroMagicPoint;           // 英雄的魔法点
        public uint m_leftCardNum;	                    // 玩家剩余卡牌的数量

        public DZPlayer(EnDZPlayer size)
        {
            m_recStartCardNum = 0;
            m_side = size;
        }

        // 清理一些基本的数据，因为每一次进场景的时候都会重新记录一些数据
        public void clear()
        {
            m_recStartCardNum = 0;
            if(m_sceneCardList != null)
            {
                m_sceneCardList.Clear();
            }
        }

        public List<SceneCardItem> sceneCardList
        {
            get
            {
                return m_sceneCardList;
            }
            set
            {
                m_sceneCardList = value;
            }
        }

        public SceneCardItem createCardItemBySvrData(EnDZPlayer playerSide, t_Card mobject)
        {
            SceneCardItem sceneItem = null;
            sceneItem = new SceneCardItem();
            sceneItem.svrCard = mobject;
            sceneItem.cardArea = (CardArea)mobject.pos.dwLocation;
            sceneItem.m_playerSide = playerSide;
            sceneItem.m_cardTableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, mobject.dwObjectID).m_itemBody as TableCardItemBody;

            return sceneItem;
        }

        public void addOneSceneCard(SceneCardItem card)
        {
            //m_sceneCardList.Add(card);
            m_sceneCardList.Insert(card.svrCard.pos.y, card);
        }

        public void removeOneSceneCard(SceneCardItem card)
        {
            m_sceneCardList.Remove(card);
        }

        public bool removeOneSceneCardByThisID(uint thisid, ref SceneCardItem sceneItem)
        {
            foreach(SceneCardItem card in m_sceneCardList)
            {
                if(card.svrCard.qwThisID == thisid)
                {
                    sceneItem = card;
                    m_sceneCardList.Remove(card);
                    return true;
                }
            }

            return false;
        }

        public bool bHaveStartCard()
        {
            return (m_startCardList != null && m_startCardList.Length > 0);
        }

        public int getStartCardNum()
        {
            if (m_startCardList != null)
            {
                int len = 0;
                int idx = 0;
                while (idx < m_startCardList.Length)
                {
                    if (m_startCardList[idx] > 0)
                    {
                        ++len;
                    }
                    ++idx;
                }

                return len;
            }

            return 0;
        }

        public bool updateCardInfo(stRetMoveGameCardUserCmd cmd)
        {
            foreach(SceneCardItem item in m_sceneCardList)
            {
                if(item.svrCard.qwThisID == cmd.qwThisID)
                {
                    item.cardArea = (CardArea)cmd.dst.dwLocation;
                    item.svrCard.pos.copyFrom(cmd.dst);
                    cmd.m_sceneCardItem = item;
                    return true;
                }
            }

            return false;
        }

        public SceneCardItem updateCardInfoByCardItem(t_Card card)
        {
            foreach (SceneCardItem item in m_sceneCardList)
            {
                if (item.svrCard.qwThisID == card.qwThisID)
                {
                    // item.m_svrCard = card;   // 不能直接赋值，因为很多都是保存的引用，这样就会有问题
                    item.svrCard.copyFrom(card);

                    return item;
                }
            }

            return null;
        }

        public bool checkTaunt()
        {
            foreach(SceneCardItem item in m_sceneCardList)
            {
                if(item.hasTaunt() && !item.isSneak())
                {
                    return true;
                }
            }
            return false;
        }

        public bool checkMp(int mpcost)
        {
            return m_heroMagicPoint.mp >= mpcost;
        }
    }
}