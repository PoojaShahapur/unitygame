using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 可缩放的格子元素
     */
    public class ScaleGridElement : GridElementBase
    {
        protected IElemMove m_movedEntity;  // 保存移动信息的实体

        protected float m_normalWidth;      // 正常宽度
        protected float m_normalHeight;     // 正常高度
        protected float m_expandWidth;      // 扩展宽度
        protected float m_expandHeight;     // 扩展高度

        public ScaleGridElement()
        {
            m_normalWidth = 1.2f;
            m_normalHeight = 2.2f;
            m_expandWidth = 2.4f;
            m_expandHeight = 4.4f;
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
        override public void setNormalPos(Vector3 pos)
        {
            m_movedEntity.setNormalPos(pos);
        }

        override public void setExpandPos(Vector3 pos)
        {
            m_movedEntity.setExpandPos(pos);
        }
    }
}