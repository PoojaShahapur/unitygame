using SDK.Common;

namespace FightCore
{
    public enum PosType
    {
        eHandDown,          // 手牌区域 Down
        eHandUp,            // 手牌区域 Up
        eOutDown,            // 出牌区域 Down
        eTotal
    }

    /**
     * @brief 位置点列表
     */
    public class WayPtList
    {
        protected MList<WayPtItem> m_ptList;

        public WayPtList()
        {
            m_ptList = new MList<WayPtItem>((int)PosType.eTotal);
            int idx = 0;
            for(idx = 0; idx < (int)PosType.eTotal; ++idx)
            {
                m_ptList.Add(null);
            }
        }

        public void setPosInfo(PosType wherePos, WayPtItem pos)
        {
            m_ptList.Add(pos);
        }

        public WayPtItem getPosInfo(PosType wherePos)
        {
            return m_ptList[(int)wherePos];
        }

        public WayPtItem getAndAddPosInfo(PosType wherePos)
        {
            if (m_ptList[(int)wherePos] == null)
            {
                m_ptList[(int)wherePos] = new WayPtItem();
            }

            return m_ptList[(int)wherePos];
        }
    }
}