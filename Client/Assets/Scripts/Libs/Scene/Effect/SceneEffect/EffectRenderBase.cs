﻿using System;
using UnityEngine;

namespace SDK.Lib
{
    public class EffectRenderBase : EntityRenderBase
    {
        public EffectRenderBase(SceneEntityBase entity_) :
            base(entity_)
        {
            
        }

        virtual public void addPlayEndEventHandle(Action<IDispatchObject> handle)
        {

        }

        virtual public void setSelfGo(GameObject go_)
        {
            
        }

        virtual public void setTableID(int tableId)
        {
            
        }

        virtual public void setLoop(bool bLoop)
        {
            
        }

        virtual public void play()
        {
            
        }

        virtual public void stop()
        {
            
        }

        override public void onTick(float delta)
        {
            
        }

        virtual public void setPnt(GameObject pntGO_)
        {
            
        }

        virtual public bool bPlay()
        {
            return false;
        }
    }
}