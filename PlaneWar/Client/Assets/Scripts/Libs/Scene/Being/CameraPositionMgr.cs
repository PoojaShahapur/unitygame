using System;

namespace SDK.Lib
{
    public class CameraPositionMgr : ITickedObject, IDelayHandleItem
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

        public void onTick(float delta)
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