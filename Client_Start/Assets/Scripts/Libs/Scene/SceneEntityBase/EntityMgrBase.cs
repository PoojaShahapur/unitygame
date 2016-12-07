using System.Collections.Generic;

namespace SDK.Lib
{
    public class EntityMgrBase : DelayHandleMgrBase, ITickedObject, IDelayHandleItem
    {
        protected MList<SceneEntityBase> mSceneEntityList;
        protected Dictionary<uint, SceneEntityBase> mId2EntityDic;
        protected MList<SceneEntityBase> mBufferPool;
        protected UniqueStrIdGen mUniqueStrIdGen;

        public EntityMgrBase()
        {
            mSceneEntityList = new MList<SceneEntityBase>();
            mBufferPool = new MList<SceneEntityBase>();
        }

        virtual public void init()
        {

        }

        virtual public void dispose()
        {

        }

        override protected void addObject(IDelayHandleItem entity, float priority = 0.0f)
        {
            if (bInDepth())
            {
                base.addObject(entity);
            }
            else
            {
                if (mSceneEntityList.IndexOf(entity as SceneEntityBase) == -1)
                {
                    mSceneEntityList.Add(entity as SceneEntityBase);
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
                if (mSceneEntityList.IndexOf(entity as SceneEntityBase) != -1)
                {
                    mSceneEntityList.Remove(entity as SceneEntityBase);
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
            // 去掉 foreach
            //foreach (SceneEntityBase entity in mSceneEntityList.list())

            int idx = 0;
            int count = this.mSceneEntityList.Count();
            SceneEntityBase entity;
            while(idx < count)
            {
                entity = this.mSceneEntityList[idx];

                if (!entity.isClientDispose())
                {
                    entity.onTick(delta);
                }

                ++idx;
            }
        }

        public void setClientDispose()
        {

        }

        public bool isClientDispose()
        {
            return false;
        }

        // 通过 Id 获取元素
        public SceneEntityBase getEntityByThisId(uint thisId)
        {
            if(this.mId2EntityDic.ContainsKey(thisId))
            {
                return this.mId2EntityDic[thisId];
            }
            return null;
        }

        // 通过数组下标获取元素
        public SceneEntityBase getEntityByIndex(int index)
        {
            if (index < this.mSceneEntityList.Count())
            {
                return this.mSceneEntityList[index];
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

        public string genNewStrId()
        {
            return mUniqueStrIdGen.genNewStrId();
        }

        public int getEntityCount()
        {
            return this.mSceneEntityList.Count();
        }
    }
}