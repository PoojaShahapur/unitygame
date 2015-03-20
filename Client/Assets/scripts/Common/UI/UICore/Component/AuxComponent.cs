using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 辅助基类，场景中的必然要有 parent
     */
    public class AuxComponent
    {
        protected GameObject m_pntGo;       // 指向父节点

        public GameObject pntGo
        {
            get
            {
                return m_pntGo;
            }
        }

        public virtual void setPntGo(GameObject go)
        {
            m_pntGo = go;
        }
    }
}