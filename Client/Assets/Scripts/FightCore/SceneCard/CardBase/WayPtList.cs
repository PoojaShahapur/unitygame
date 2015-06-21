using SDK.Common;

namespace FightCore
{
    public enum PosType
    {
        eHandDown,          // 手牌区域 Down
        eHandUp,            // 手牌区域 Up
        eOutDown,           // 出牌区域 Down
        eWatchUp,           // 观察卡牌 Up
        eScaleUp,           // 缩放卡牌 Up ，就是手滑动过某一个，某一个放大
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
            for (int idx = 0; idx < (int)PosType.eTotal; ++idx)
            {
                m_ptList.Add(new WayPtItem());
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