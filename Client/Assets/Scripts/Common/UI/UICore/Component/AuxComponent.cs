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
        protected GameObject m_pntGo;       // 指向父节点
        protected GameObject m_placeHolderGo;      // 自己节点，资源挂在 m_placeHolderGo 上， m_placeHolderGo 挂在 m_pntGo 上
        protected bool m_bNeedPlaceHolderGo;    // 是否需要占位 GameObject

        public AuxComponent()
        {
            m_bNeedPlaceHolderGo = false;
        }

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

        public GameObject pntGo
        {
            get
            {
                return m_pntGo;
            }
            set
            {
                bool bPntChange = false;
                if (m_pntGo == null || !m_pntGo.Equals(m_pntGo))
                {
                    bPntChange = true;
                }

                m_pntGo = value;

                if (bPntChange)
                {
                    onPntChanged();
                }
            }
        }

        public virtual void setPntGo(GameObject go)
        {
            m_pntGo = go;
        }

        public bool bNeedPlaceHolderGo
        {
            get
            {
                return m_bNeedPlaceHolderGo;
            }
            set
            {
                m_bNeedPlaceHolderGo = value;
            }
        }

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

        virtual public void dispose()
        {
            if (m_bNeedPlaceHolderGo && m_placeHolderGo != null)
            {
                UtilApi.Destroy(m_placeHolderGo);
            }
        }

        // 父节点发生改变
        virtual protected void onPntChanged()
        {

        }

        public void linkPlaceHolder2Parent()
        {
            if (m_placeHolderGo == null)
            {
                m_placeHolderGo = new GameObject();
            }
            UtilApi.SetParent(m_placeHolderGo, m_pntGo, false);
        }

        public void show()
        {
            if (m_selfGo != null)
            {
                UtilApi.SetActive(m_selfGo, true);
            }
        }

        public void hide()
        {
            if (m_selfGo != null)
            {
                UtilApi.SetActive(m_selfGo, false);
            }
        }
    }
}