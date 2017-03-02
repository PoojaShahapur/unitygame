using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 特效渲染器比较特殊， Render 中的包含真正的 Render ，其他的模型都是 Render 直接包含显示对象
     */
    public class EffectSpriteRender : EntityRenderBase
    {
        protected SpriteRenderSpriteAni m_spriteRender;

        public EffectSpriteRender(SceneEntityBase entity_) :
            base(entity_)
        {
            m_spriteRender = new SpriteRenderSpriteAni(mEntity);
        }

        public SpriteRenderSpriteAni spriteRender
        {
            get
            {
                return m_spriteRender;
            }
            set
            {
                m_spriteRender = value;
            }
        }

        override public void dispose()
        {
            if (m_spriteRender != null)
            {
                m_spriteRender.dispose();
                m_spriteRender = null;
            }
        }

        override public void setClientDispose(bool isDispose)
        {
            base.setClientDispose(isDispose);
            m_spriteRender.setClientDispose(isDispose);
        }

        override public GameObject gameObject()
        {
            return m_spriteRender.selfGo;
        }

        override public Transform transform()
        {
            return m_spriteRender.selfGo.transform;
        }

        override public void onTick(float delta)
        {
            m_spriteRender.onTick(delta);
        }

        override public void setGameObject(GameObject rhv)
        {
            m_spriteRender.selfGo = rhv;
        }

        override public void setPntGo(GameObject pntGO_)
        {
            m_spriteRender.pntGo = pntGO_;
        }

        override public bool checkRender()
        {
            return m_spriteRender.checkRender();
        }

        public bool bPlay()
        {
            return m_spriteRender.bPlay();
        }
    }
}