﻿using UnityEngine;
namespace SDK.Lib
{
    /**
     * @brief 可缩放的格子元素
     */
    public class ScaleGridElement : GridElementBase
    {
        protected IElemMove m_movedEntity;  // 保存移动信息的实体

        protected float m_normalWidth;  // 
        protected float m_normalHeight;  // 
        protected float m_expandWidth;  // 
        protected float m_expandHeight;  // 

        public ScaleGridElement()
        {

        }

        public void setWidthAndHeight(float normalWidth_, float normalHeight_, float expandWidth_, float expandHeight_)
        {
            m_normalWidth = normalWidth_;
            m_normalHeight = normalHeight_;
            m_expandWidth = expandWidth_;
            m_expandHeight = expandHeight_;
        }

        public void setMovedEntity(IElemMove entity)
        {
            m_movedEntity = entity;
        }

        public void normal()
        {
            m_elemState = GridElementState.eNormal;
            m_needUpdateDisp.dispatchEvent(this);
        }

        public void expand()
        {
            m_elemState = GridElementState.eExpand;
            m_needUpdateDisp.dispatchEvent(this);
        }

        override public float getNormalWidth()
        {
            return m_normalWidth;
        }

        override public float getNormalHeight()
        {
            return m_normalHeight;
        }

        override public float getExpandWidth()
        {
            return m_expandWidth;
        }

        override public float getExpandHeight()
        {
            return m_expandHeight;
        }

        // 设置位置信息
        override public void setPos(Vector3 pos)
        {
            m_movedEntity.setPos(pos);
        }

        override public void updatePos()
        {
            m_movedEntity.updatePos();
        }
    }
}