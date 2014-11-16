using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 蒙皮网格骨骼动画模型资源
     */
    public class SkinAniModel
    {
        protected GameObject m_rootGo;                  // 跟 GO
        public GameObject[] m_modelList;                // 一个数组
        protected Transform m_transform;                // 位置信息

        public GameObject rootGo
        {
            get
            {
                return m_rootGo;
            }
            set
            {
                m_rootGo = value;
                m_transform = m_rootGo.transform;
            }
        }

        public Transform transform
        {
            get
            {
                return m_transform;
            }
            set
            {
                m_transform = value;
            }
        }
    }
}
