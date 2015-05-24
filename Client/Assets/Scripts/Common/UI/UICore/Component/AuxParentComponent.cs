using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 有父节点的组件
     */
    public class AuxParentComponent : AuxComponent
    {
        protected GameObject m_pntGo;       // 指向父节点

        public GameObject pntGo
        {
            get
            {
                return m_pntGo;
            }
            set
            {
                m_pntGo = value;
            }
        }

        public virtual void setPntGo(GameObject go)
        {
            m_pntGo = go;
        }
    }
}