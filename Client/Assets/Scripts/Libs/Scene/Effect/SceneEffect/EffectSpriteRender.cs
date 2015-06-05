using UnityEngine;

namespace SDK.Lib
{
    public class EffectSpriteRender : EntityRenderBase
    {
        protected SpriteRenderSpriteAni m_spriteRender;

        public EffectSpriteRender(SceneEntityBase entity_) :
            base(entity_)
        {
            m_spriteRender = new SpriteRenderSpriteAni(m_entity);
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

        override public void setPnt(GameObject pntGO_)
        {
            m_spriteRender.pntGo = pntGO_;
        }
    }
}