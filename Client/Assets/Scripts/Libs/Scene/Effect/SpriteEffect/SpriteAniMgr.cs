using SDK.Common;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief UI 帧动画管理器，仅仅是存放 ImageSpriteAni 渲染器
     */
    public class SpriteAniMgr : DelayHandleMgrBase, ITickedObject
    {
        protected List<ImageSpriteAni> m_sceneEntityList;

        public SpriteAniMgr()
        {
            m_sceneEntityList = new List<ImageSpriteAni>();
        }

        public void add2List(ImageSpriteAni entity)
        {
            if (m_duringAdvance)
            {
                addObject(entity);
            }
            else
            {
                m_sceneEntityList.Add(entity);
            }
        }

        public void removeFromeList(ImageSpriteAni entity)
        {
            if (m_duringAdvance)
            {
                delObject(entity);
            }
            else
            {
                m_sceneEntityList.Remove(entity);
            }
        }

        virtual public void OnTick(float delta)
        {
            m_duringAdvance = true;
            onTickExec(delta);
            m_duringAdvance = false;
            onTickEnd();
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

        virtual protected void onTickEnd()
        {
            processScheduledObjects();
        }

        public SpriteAni createAndAdd()
        {
            ImageSpriteAni ani = null;
            ani = new ImageSpriteAni();

            Ctx.m_instance.m_spriteAniMgr.add2List(ani);
            return ani;
        }

        public void removeAndDestroy(ImageSpriteAni ani)
        {
            this.removeFromeList(ani);
            ani.dispose();
        }

        public void setClientDispose()
        {

        }

        public bool getClientDispose()
        {
            return false;
        }
    }
}