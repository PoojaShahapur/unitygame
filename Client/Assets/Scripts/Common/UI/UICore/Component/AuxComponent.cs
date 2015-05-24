using SDK.Lib;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 辅助基类
     */
    public class AuxComponent : IDispatchObject
    {
        protected GameObject m_selfGo;      // 自己节点

        public GameObject selfGo
        {
            get
            {
                return m_selfGo;
            }
            set
            {
                m_selfGo = value;
            }
        }

        virtual public void dispose()
        {
            
        }

        public void show()
        {
            UtilApi.SetActive(m_selfGo, true);
        }

        public void hide()
        {
            UtilApi.SetActive(m_selfGo, false);
        }
    }
}