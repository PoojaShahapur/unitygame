using SDK.Common;
using System;
using UnityEngine;

namespace SDK.Lib
{
    public class EffectBase : SceneEntity
    {
        protected EventDispatch m_playEndEventDispatch;         // 特效播放完成事件分发
        protected bool m_bAutoRemove;       // 特效播放完成是否自动移除

        public EffectBase()
        {
            m_render = new EffectSpriteRender(this);
            m_playEndEventDispatch = new AddOnceAndCallOnceEventDispatch();
            (m_render as EffectSpriteRender).spriteRender.playEndEventDispatch.addEventHandle(onEffectPlayEnd);
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
            (m_render as EffectSpriteRender).spriteRender.selfGo = go_;
        }

        virtual public void setTableID(int tableId)
        {
            (m_render as EffectSpriteRender).spriteRender.tableID = tableId;
        }

        virtual public void setLoop(bool bLoop)
        {
            (m_render as EffectSpriteRender).spriteRender.bLoop = bLoop;
        }

        virtual public void play()
        {
            (m_render as EffectSpriteRender).spriteRender.play();
        }

        virtual public void stop()
        {
            (m_render as EffectSpriteRender).spriteRender.stop();
        }

        override public void dispose()
        {
            Ctx.m_instance.m_sceneEffectMgr.delObject(this);
            base.dispose();
        }

        override public void onTick(float delta)
        {
            m_render.onTick(delta);
        }

        // 添加特效播放结束处理
        public void addEffectPlayEndHandle(Action<IDispatchObject> handle)
        {
            m_playEndEventDispatch.addEventHandle(handle);
        }

        virtual public void addMoveDestEventHandle(Action<IDispatchObject> dispObj)
        {

        }

        override public void setPnt(GameObject pntGO_)
        {
            (m_render as EffectSpriteRender).setPnt(pntGO_);
        }

        public bool bPlay()
        {
            return (m_render as EffectSpriteRender).bPlay();
        }
    }
}