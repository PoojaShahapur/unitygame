using System;
using UnityEngine;

namespace SDK.Lib
{
    public enum GridElementType
    {
        eBasic,     // 基本类型
        eScale,     // 缩放
    }

    public enum GridElementState
    {
        eNormal,        // 正常状态
        eExpand,        // 放大状体
    }

    /**
     * @brief 基本格子元素
     */
    public class GridElementBase : IDispatchObject
    {
        protected EventDispatch m_needUpdateDisp;           // 需要更新分发器
        protected bool m_bValid;            // 是否有效，如果无效就不计算这个元素了
        protected GridElementState m_elemState;

        public GridElementBase()
        {
            m_bValid = true;
            m_needUpdateDisp = new AddOnceEventDispatch();
            m_elemState = GridElementState.eNormal;
        }

        public void dispose()
        {
            m_needUpdateDisp.clearEventHandle();
        }

        public EventDispatch needUpdateDisp
        {
            get
            {
                return m_needUpdateDisp;
            }
            set
            {
                m_needUpdateDisp = value;
            }
        }

        public bool bValid
        {
            get
            {
                return m_bValid;
            }
            set
            {
                m_bValid = value;
            }
        }

        public GridElementState elemState
        {
            get
            {
                return m_elemState;
            }
            set
            {
                m_elemState = value;
            }
        }

        public bool isNormalState()
        {
            return m_elemState == GridElementState.eNormal;
        }

        virtual public float getNormalWidth()
        {
            return 0;
        }

        virtual public float getNormalHeight()
        {
            return 0;
        }

        virtual public float getExpandWidth()
        {
            return 0;
        }

        virtual public float getExpandHeight()
        {
            return 0;
        }

        // 设置位置信息
        virtual public void setNormalPos(Vector3 pos)
        {

        }

        virtual public void setExpandPos(Vector3 pos)
        {

        }

        public void addUpdateHandle(Action<IDispatchObject> handle)
        {
            m_needUpdateDisp.addEventHandle(null, handle);
        }
    }
}