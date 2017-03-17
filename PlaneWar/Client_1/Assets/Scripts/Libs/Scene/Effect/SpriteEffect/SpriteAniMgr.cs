using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief UI 帧动画管理器，仅仅是存放 ImageSpriteAni 渲染器
     */
    public class SpriteAniMgr : DelayPriorityHandleMgrBase, ITickedObject, IDelayHandleItem, INoOrPriorityObject
    {
        protected List<ImageSpriteAni> mSceneEntityList;

        public SpriteAniMgr()
        {
            mSceneEntityList = new List<ImageSpriteAni>();
        }

        public void add2List(ImageSpriteAni entity)
        {
            addObject(entity);
        }

        override protected void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
        {
            if (this.isInDepth())
            {
                base.addObject(delayObject);
            }
            else
            {
                mSceneEntityList.Add(delayObject as ImageSpriteAni);
            }
        }

        public void removeFromList(ImageSpriteAni entity)
        {
            removeObject(entity);
        }

        override protected void removeObject(IDelayHandleItem delayObject)
        {
            if (this.isInDepth())
            {
                base.removeObject(delayObject);
            }
            else
            {
                mSceneEntityList.Remove(delayObject as ImageSpriteAni);
            }
        }

        virtual public void onTick(float delta, TickMode tickMode)
        {
            this.incDepth();

            this.onTickExec(delta, tickMode);

            this.decDepth();
        }

        virtual protected void onTickExec(float delta, TickMode tickMode)
        {
            foreach (ImageSpriteAni entity in mSceneEntityList)
            {
                if (!(entity as ImageSpriteAni).isClientDispose())
                {
                    (entity as ImageSpriteAni).onTick(delta, tickMode);
                }
            }
        }

        public SpriteAni createAndAdd()
        {
            ImageSpriteAni ani = null;
            ani = new ImageSpriteAni();

            Ctx.mInstance.mSpriteAniMgr.add2List(ani);
            return ani;
        }

        //public void removeAndDestroy(ImageSpriteAni ani)
        //{
        //    this.removeFromList(ani);
        //    ani.dispose();
        //}

        public void setClientDispose(bool isDispose)
        {

        }

        public bool isClientDispose()
        {
            return false;
        }
    }
}