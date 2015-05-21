using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 自己有占位资源 m_placeHolderGo
     */
    public class AuxPlaceHolderComponent : AuxComponent
    {
        protected GameObject m_placeHolderGo;      // 自己节点，资源挂在 m_placeHolderGo 上， m_placeHolderGo 挂在 m_pntGo 上

        public GameObject placeHolderGo
        {
            get
            {
                return m_placeHolderGo;
            }
            set
            {
                m_placeHolderGo = value;
            }
        }

        public void linkPlaceHolder2Parent()
        {
            UtilApi.SetParent(m_placeHolderGo, m_pntGo);
        }
    }
}