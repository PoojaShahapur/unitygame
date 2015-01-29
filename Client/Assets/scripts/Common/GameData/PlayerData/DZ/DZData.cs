using Game.Msg;

namespace SDK.Common
{
    public enum EnDZPlayer
    {
        ePlayerSelf,            // 自己
        ePlayerEnemy,           // 敌人
        ePlayerTotal            // 总共数量
    }

    /**
     * @brief 对战基本数据
     */
    public class DZData
    {
        public bool m_canReqDZ = true;     // 是否可以请求对战，如果已经请求了需要等待服务器返回
        public DZPlayer[] m_playerArr = new DZPlayer[(int)EnDZPlayer.ePlayerTotal];
        public byte m_priv;                 // 当前拥有权力的一方
        public byte m_state;                // 当前游戏所处的阶段，每一个阶段允许的操作是不同的
        public ushort m_enemyCardCount = 0;       // enemy 卡牌数量

        public bool m_bLastEnd = true;          // 下线前最后一次战斗是否结束，如果没有结束，需要继续进入战斗
        protected int m_curPlayCardCount = 0;          // 当前总共的出牌次数

        public DZData()
        {
            m_playerArr[(int)EnDZPlayer.ePlayerSelf] = new DZPlayer();
            m_playerArr[(int)EnDZPlayer.ePlayerEnemy] = new DZPlayer();
        }

        public void setSelfHeroInfo(CardGroupItem cardGroup)
        {
            m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroName = cardGroup.m_cardGroup.name;
            m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroOccupation = cardGroup.m_cardGroup.occupation;
        }

        public void clear()
        {
            m_playerArr[(int)EnDZPlayer.ePlayerSelf].clear();
            m_playerArr[(int)EnDZPlayer.ePlayerEnemy].clear();
        }

        // 移动后，更新数据
        public int updateCardInfo(stRetMoveGameCardUserCmd cmd)
        {
            // 查找后两边更新
            if(m_playerArr[(int)EnDZPlayer.ePlayerSelf].updateCardInfo(cmd))
            {
                return 1;
            }
            else
            {
                m_playerArr[(int)EnDZPlayer.ePlayerEnemy].updateCardInfo(cmd);
                return 2;
            }
        }

        // 是否是自己一般控制
        public bool bSelfSide()
        {
            return Ctx.m_instance.m_dataPlayer.m_dzData.m_priv == 1;
        }

        // 计算卡牌属性哪一方出的
        public int getCardSideByThisID(uint thisID)
        {
            int idx = 0;
            while (idx < 2)
            {
                foreach (SceneCardItem item in m_playerArr[idx].m_sceneCardList)
                {
                    if (item.m_svrCard.qwThisID == thisID)
                    {
                        return idx;
                    }
                }

                ++idx;
            }

            return idx;
        }

        public void getCardSideAndItemByThisID(uint thisID, ref int side, ref SceneCardItem retItem)
        {
            int idx = 0;
            while (idx < 2)
            {
                foreach (SceneCardItem item in m_playerArr[idx].m_sceneCardList)
                {
                    if (item.m_svrCard.qwThisID == thisID)
                    {
                        retItem = item;
                        side = idx;
                        return;
                    }
                }

                ++idx;
            }

            side = 2;       // 说明没有查找到
        }

        public SceneCardItem getCardItemByThisID(uint thisID)
        {
            int idx = 0;
            while (idx < 2)
            {
                foreach (SceneCardItem item in m_playerArr[idx].m_sceneCardList)
                {
                    if (item.m_svrCard.qwThisID == thisID)
                    {
                        return item;
                    }
                }

                ++idx;
            }

            return null;
        }

        public int curPlayCardCount
        {
            get
            {
                return m_curPlayCardCount;
            }
            set
            {
                m_curPlayCardCount = value;
            }
        }

        // 获取战局数量，两面都出算一次战局
        public int getWarCount()
        {
            return (m_curPlayCardCount + 1 / 2);
        }
    }
}