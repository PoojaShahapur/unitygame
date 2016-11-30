using System;
using UnityEngine;

namespace SDK.Lib
{
    public class EffectBase : SceneEntityBase
    {
        protected EventDispatch m_playEndEventDispatch;         // 特效播放完成事件分发
        protected bool m_bAutoRemove;       // 特效播放完成是否自动移除

        public EffectBase(EffectRenderType renderType)
        {
            if (EffectRenderType.eSpriteEffectRender == renderType)
            {
                mRender = new SpriteEffectRender(this);
            }
            else if (EffectRenderType.eShurikenEffectRender == renderType)
            {
                mRender = new ShurikenEffectRender(this);
            }
            else if (EffectRenderType.eFxEffectRender == renderType)
            {
                mRender = new FxEffectRender(this);
            }

            m_playEndEventDispatch = new AddOnceAndCallOnceEventDispatch();
            effectRender.addPlayEndEventHandle(onEffectPlayEnd);
        }

        public EventDispatch playEndEventDispatch
        {
            get
            {
                return m_playEndEventDispatch;
            }
            set
            {
                m_playEndEventDispatch = value;
            }
        }

        public bool bAutoRemove
        {
            get
            {
                return m_bAutoRemove;
            }
            set
            {
                m_bAutoRemove = value;
            }
        }

        protected EffectRenderBase effectRender
        {
            get
            {
                return mRender as EffectRenderBase;
            }
        }

        virtual public void onEffectPlayEnd(IDispatchObject dispObj)
        {
            m_playEndEventDispatch.dispatchEvent(this);

            if (m_bAutoRemove)
            {
                this.dispose();          // 释放资源
            }
        }

        virtual public void setSelfGo(GameObject go_)
        {
            effectRender.setSelfGo(go_);
        }

        virtual public void setTableID(int tableId)
        {
            effectRender.setTableID(tableId);
        }

        virtual public void setLoop(bool bLoop)
        {
            effectRender.setLoop(bLoop);
        }

        virtual public void play()
        {
            effectRender.play();
        }

        virtual public void stop()
        {
            effectRender.stop();
        }

        override public void dispose()
        {
            Ctx.mInstance.mSceneEffectMgr.removeEffect(this);
            base.dispose();
        }

        override public void onTick(float delta)
        {
            mRender.onTick(delta);
        }

        // 添加特效播放结束处理
        public void addEffectPlayEndHandle(MAction<IDispatchObject> handle)
        {
            m_playEndEventDispatch.addEventHandle(null, handle);
        }

        virtual public void addMoveDestEventHandle(MAction<IDispatchObject> dispObj)
        {

        }

        override public void setPnt(GameObject pntGO_)
        {
            effectRender.setPntGo(pntGO_);
        }

        public bool bPlay()
        {
            return effectRender.bPlay();
        }

        public void setKeepLastFrame(bool bKeep)
        {
            effectRender.setKeepLastFrame(bKeep);
        }

        public void setLoopType(eSpriteLoopType type)
        {
            effectRender.setLoopType(type);
        }
    }
}