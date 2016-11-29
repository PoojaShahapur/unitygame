using System.Collections.Generic;

namespace SDK.Lib
{
    public class EntityMgrBase : DelayHandleMgrBase, ITickedObject, IDelayHandleItem
    {
        protected MList<SceneEntityBase> m_sceneEntityList;
        protected Dictionary<uint, SceneEntityBase> mId2EntityDic;
        protected MList<SceneEntityBase> mBufferPool;

        public EntityMgrBase()
        {
            m_sceneEntityList = new MList<SceneEntityBase>();
            mBufferPool = new MList<SceneEntityBase>();
        }

        override protected void addObject(IDelayHandleItem entity, float priority = 0.0f)
        {
            if (bInDepth())
            {
                base.addObject(entity);
            }
            else
            {
                if (m_sceneEntityList.IndexOf(entity as SceneEntityBase) == -1)
                {
                    m_sceneEntityList.Add(entity as SceneEntityBase);
                }
            }
        }

        override protected void removeObject(IDelayHandleItem entity)
        {
            if (bInDepth())
            {
                base.removeObject(entity);
            }
            else
            {
                if (m_sceneEntityList.IndexOf(entity as SceneEntityBase) != -1)
                {
                    m_sceneEntityList.Remove(entity as SceneEntityBase);
                }
            }
        }

        virtual public void addEntity(SceneEntityBase entity)
        {
            this.addObject(entity);
            entity.onInit();
        }

        public void removeEntity(SceneEntityBase entity, bool isDispose = true)
        {
            this.removeObject(entity);
            this.mBufferPool.Add(entity);
            if(isDispose)
            {
                entity.onDestroy();
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
            foreach (SceneEntityBase entity in m_sceneEntityList.list())
            {
                if (!entity.isClientDispose())
                {
                    entity.onTick(delta);
                }
            }
        }

        public void setClientDispose()
        {

        }

        public bool isClientDispose()
        {
            return false;
        }

        public SceneEntityBase getEntityByThisId(uint thisId)
        {
            if(mId2EntityDic.ContainsKey(thisId))
            {
                return mId2EntityDic[thisId];
            }
            return null;
        }

        public SceneEntityBase getBufferEntity()
        {
            SceneEntityBase entity = null;
            if (mBufferPool.Count() > 0)
            {
                entity = mBufferPool[0];
                mBufferPool.Remove(entity);
            }

            return entity;
        }
    }
}