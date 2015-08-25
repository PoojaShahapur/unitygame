using SDK.Lib;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief UI 帧动画管理器，仅仅是存放 ImageSpriteAni 渲染器
     */
    public class SpriteAniMgr : DelayHandleMgrBase, ITickedObject, IDelayHandleItem
    {
        protected List<ImageSpriteAni> m_sceneEntityList;

        public SpriteAniMgr()
        {
            m_sceneEntityList = new List<ImageSpriteAni>();
        }

        public void add2List(ImageSpriteAni entity)
        {
            addObject(entity);
        }

        override public void addObject(IDelayHandleItem delayObject, float priority = 0.0f)
        {
            if (bInDepth())
            {
                base.addObject(delayObject);
            }
            else
            {
                m_sceneEntityList.Add(delayObject as ImageSpriteAni);
            }
        }

        public void removeFromList(ImageSpriteAni entity)
        {
            delObject(entity);
        }

        override public void delObject(IDelayHandleItem delayObject)
        {
            if (bInDepth())
            {
                base.delObject(delayObject);
            }
            else
            {
                m_sceneEntityList.Remove(delayObject as ImageSpriteAni);
            }
        }

        virtual public void onTick(float delta)
        {
            incDepth();

            onTickExec(delta);

            decDepth();
        }

        virtual protected void onTickExec(float delta)
        {
            foreach (ImageSpriteAni entity in m_sceneEntityList)
            {
                if (!(entity as ImageSpriteAni).getClientDispose())
                {
                    (entity as ImageSpriteAni).onTick(delta);
                }
            }
        }

        public SpriteAni createAndAdd()
        {
            ImageSpriteAni ani = null;
            ani = new ImageSpriteAni();

            Ctx.m_instance.m_spriteAniMgr.add2List(ani);
            return ani;
        }

        //public void removeAndDestroy(ImageSpriteAni ani)
        //{
        //    this.removeFromList(ani);
        //    ani.dispose();
        //}

        public void setClientDispose()
        {

        }

        public bool getClientDispose()
        {
            return false;
        }
    }
}