using System.Collections.Generic;

namespace SDK.Common
{
    /**
     * @brief 对战的一个玩家
     */
    public class DZPlayer
    {
        public string m_heroName;               // 自己的 Hero 名字
        public uint m_heroOccupation;           // 自己的 Hero 的职业
        public uint[] m_startCardList;          // 第一次自己 Hero 得到的 Card，仅仅有 ID
        public List<SceneCardItem> m_sceneCardList = new List<SceneCardItem>();     // 完成的卡牌数据

        public int m_recStartCardNum;                   // 接收到的开始卡牌的数量，因为启动的时候，仅仅发送过来卡牌 ID，真正的数据后来才发送过来

        public t_MagicPoint m_heroMagicPoint;           // 英雄的魔法点
        public uint m_leftCardNum;	                    // 玩家剩余卡牌的数量

        // 清理一些基本的数据，因为每一次进场景的时候都会重新记录一些数据
        public void clear()
        {
            m_recStartCardNum = 0;
        }

        public int getStartCardNum()
        {
            if(m_sceneCardList != null)
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
    }
}