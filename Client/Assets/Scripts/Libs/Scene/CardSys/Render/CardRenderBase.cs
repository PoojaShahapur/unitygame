﻿using UnityEngine;
namespace SDK.Lib
{
    /**
     * @brief 卡牌使用的渲染器，所有卡牌渲染器的基类
     */
    public class CardRenderBase : EntityRenderBase, IDispatchObject
    {
        protected EventDispatch m_clickEntityDisp;  // 点击事件分发

        public CardRenderBase(SceneEntityBase entity_) :
            base(entity_)
        {
            m_clickEntityDisp = new AddOnceEventDispatch();
        }

        override public void dispose()
        {
            base.dispose();
            m_clickEntityDisp.clearEventHandle();
        }

        public EventDispatch clickEntityDisp
        {
            get
            {
                return m_clickEntityDisp;
            }
        }

        public void onEntityClick(GameObject go)
        {
            m_clickEntityDisp.dispatchEvent(this);
        }
    }
}