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
            m_ptList.Add(new WayPtItem());
            m_ptList[(int)PosType.eHandDown].rot = new UnityEngine.Vector3(0, 0, -3);
            m_ptList[(int)PosType.eHandDown].scale = new UnityEngine.Vector3(0.5f, 0.5f, 0.5f);

            m_ptList.Add(new WayPtItem());
            m_ptList[(int)PosType.eHandUp].rot = new UnityEngine.Vector3(0, 0, 0);
            m_ptList[(int)PosType.eHandUp].scale = new UnityEngine.Vector3(1, 1, 1);

            m_ptList.Add(new WayPtItem());
            m_ptList[(int)PosType.eOutDown].rot = new UnityEngine.Vector3(0, 0, 0);
            m_ptList[(int)PosType.eOutDown].scale = new UnityEngine.Vector3(1, 1, 1);
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