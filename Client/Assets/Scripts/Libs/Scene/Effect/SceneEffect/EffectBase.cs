using SDK.Common;
using System;
using UnityEngine;

namespace SDK.Lib
{
    public class EffectBase : SceneEntity
    {
        public EffectBase()
        {
            m_render = new EffectSpriteRender(this);
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
            Ctx.m_instance.m_sceneEffectMgr.removeAndDestroy(this);
            base.dispose();
        }

        override public void onTick(float delta)
        {
            m_render.onTick(delta);
        }

        // 添加特效播放结束处理
        public void addEffectPlayEndHandle(Action<IDispatchObject> handle)
        {
            (m_render as EffectSpriteRender).spriteRender.playEndEventDispatch.addEventHandle(handle);
        }

        virtual public void addMoveDestEventHandle(Action<IDispatchObject> dispObj)
        {

        }
    }
}