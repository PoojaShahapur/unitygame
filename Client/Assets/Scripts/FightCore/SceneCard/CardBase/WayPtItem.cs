using UnityEngine;

namespace FightCore
{
    /**
     * @brief 位置点
     */
    public class WayPtItem
    {
        protected Vector3 m_pos;       // 位置
        protected Vector3 m_rot;       // 旋转
        protected Vector3 m_scale;     // 缩放

        public Vector3 pos
        {
            get
            {
                return m_pos;
            }
            set
            {
                m_pos = value;
            }
        }

        public Vector3 rot
        {
            get
            {
                return m_rot;
            }
            set
            {
                m_rot = value;
            }
        }

        public Vector3 scale
        {
            get
            {
                return m_scale;
            }
            set
            {
                m_scale = value;
            }
        }
    }
}