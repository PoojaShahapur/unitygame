using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 特效渲染器比较特殊， Render 中的包含真正的 Render ，其他的模型都是 Render 直接包含显示对象
     */
    public class SpriteEffectRender : EffectRenderBase
    {
        protected SpriteRenderSpriteAni m_spriteRender;

        public SpriteEffectRender(SceneEntityBase entity_) :
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

        override public void setClientDispose()
        {
            base.setClientDispose();
            m_spriteRender.setClientDispose();
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

        override public bool checkRender()
        {
            return m_spriteRender.checkRender();
        }

        override public bool bPlay()
        {
            return m_spriteRender.bPlay();
        }

        override public void addPlayEndEventHandle(Action<IDispatchObject> handle)
        {
            m_spriteRender.playEndEventDispatch.addEventHandle(handle);
        }

        override public void setSelfGo(GameObject go_)
        {
            m_spriteRender.selfGo = go_;
        }

        override public void setTableID(int tableId)
        {
            m_spriteRender.tableID = tableId;
        }

        override public void setLoop(bool bLoop)
        {
            m_spriteRender.bLoop = bLoop;
        }

        override public void play()
        {
            m_spriteRender.play();
        }

        override public void stop()
        {
            m_spriteRender.stop();
        }

        override public void setKeepLastFrame(bool bKeep)
        {
            m_spriteRender.setKeepLastFrame(bKeep);
        }

        override public void setLoopType(eSpriteLoopType type)
        {
            m_spriteRender.setLoopType(type);
        }
    }
}