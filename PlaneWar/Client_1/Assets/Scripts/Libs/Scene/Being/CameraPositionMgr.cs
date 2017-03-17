﻿using System;

namespace SDK.Lib
{
    public class CameraPositionMgr : ITickedObject, IDelayHandleItem, INoOrPriorityObject
    {
        public CameraPositionMgr()
        {
            
        }

        public void init()
        {

        }

        public void dispose()
        {
        }

        public void onTick(float delta, TickMode tickMode)
        {
            if(Ctx.mInstance.mPlayerMgr.getHero() != null
               /*&& (BeingState.eBSWalk == Ctx.mInstance.mPlayerMgr.getHero().getBeingState() || BeingState.eBSIOControlWalk == Ctx.mInstance.mPlayerMgr.getHero().getBeingState())*/)
            {
                Ctx.mInstance.mPlayerMgr.getHero().onChildChanged();
            }
        }

        public void setClientDispose(bool isDispose)
        {
           
        }

        public bool isClientDispose()
        {
            return false;
        }
    }
}