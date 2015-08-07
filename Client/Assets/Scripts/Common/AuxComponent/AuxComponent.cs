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
        protected LuaCSBridge m_luaCSBridge;

        public AuxComponent(LuaCSBridge luaCSBridge_ = null)
        {
            m_luaCSBridge = luaCSBridge_;
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
                bool bPntChange = bChange(m_selfGo, value);
                m_selfGo = value;
                if (bPntChange)
                {
                    onSelfChanged();
                }
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
                bool bPntChange = bChange(m_pntGo, value);
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
                if(m_bNeedPlaceHolderGo)
                {
                    if (m_placeHolderGo == null)
                    {
                        m_placeHolderGo = UtilApi.createGameObject("PlaceHolderGO");
                    }
                }
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

        protected bool bChange(GameObject srcGO, GameObject destGO)
        {
            if (srcGO == null || !srcGO.Equals(destGO))
            {
                return true;
            }

            return false;
        }

        // 父节点发生改变
        virtual protected void onPntChanged()
        {

        }

        // 自己发生改变
        virtual protected void onSelfChanged()
        {

        }

        public void linkPlaceHolder2Parent()
        {
            if (m_placeHolderGo == null)
            {
                m_placeHolderGo = UtilApi.createGameObject("PlaceHolderGO");
            }
            UtilApi.SetParent(m_placeHolderGo, m_pntGo, false);
        }

        public void linkSelf2Parent()
        {
            if (m_selfGo != null && m_pntGo != null)   // 现在可能还没有创建
            {
                UtilApi.SetParent(m_selfGo, m_pntGo, false);
            }
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

        public bool IsVisible()
        {
            return UtilApi.IsActive(m_selfGo);
        }
    }
}