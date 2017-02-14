namespace SDK.Lib
{
    public class EntityMgrBase : DelayHandleMgrBase, ITickedObject, IDelayHandleItem
    {
        protected MList<SceneEntityBase> mSceneEntityList;
        protected MDictionary<string, SceneEntityBase> mId2EntityDic;
        protected MDictionary<uint, SceneEntityBase> mThisId2EntityDic;
        protected MList<SceneEntityBase> mBufferPool;
        protected UniqueStrIdGen mUniqueStrIdGen;

        public EntityMgrBase()
        {
            this.mSceneEntityList = new MList<SceneEntityBase>();
            this.mId2EntityDic = new MDictionary<string, SceneEntityBase>();
            this.mThisId2EntityDic = new MDictionary<uint, SceneEntityBase>();
            this.mBufferPool = new MList<SceneEntityBase>();
        }

        override public void init()
        {

        }

        override public void dispose()
        {
            this.clearAll();
            this.mBufferPool.Clear();   // 清理缓冲池
        }

        override protected void addObject(IDelayHandleItem entity, float priority = 0.0f)
        {
            if (this.isInDepth())
            {
                base.addObject(entity);
            }
            else
            {
                if (!this.mSceneEntityList.Contains(entity as SceneEntityBase))
                {
                    this.mSceneEntityList.Add(entity as SceneEntityBase);
                }
            }
        }

        override protected void removeObject(IDelayHandleItem entity)
        {
            if (this.isInDepth())
            {
                base.removeObject(entity);
            }
            else
            {
                if (this.mSceneEntityList.Contains(entity as SceneEntityBase))
                {
                    this.mSceneEntityList.Remove(entity as SceneEntityBase);
                }
            }
        }

        virtual public void addEntity(SceneEntityBase entity)
        {
            this.addObject(entity);

            if(!this.mId2EntityDic.ContainsKey(entity.getEntityUniqueId()))
            {
                this.mId2EntityDic[entity.getEntityUniqueId()] = entity;
            }
            else
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("EntityMgrBase already exist key", LogTypeId.eLogCommon);
                }
            }

            if (!this.mThisId2EntityDic.ContainsKey(entity.getThisId()))
            {
                this.mThisId2EntityDic[entity.getThisId()] = entity;
            }

            //entity.onInit();
        }

        public void removeEntity(SceneEntityBase entity, bool isDispose = true)
        {
            this.removeObject(entity);
            this.mBufferPool.Add(entity);

            if (this.mId2EntityDic.ContainsKey(entity.getEntityUniqueId()))
            {
                this.mId2EntityDic.Remove(entity.getEntityUniqueId());
            }
            else
            {
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("EntityMgrBase already remove key", LogTypeId.eLogCommon);
                }
            }

            if (this.mThisId2EntityDic.ContainsKey(entity.getThisId()))
            {
                this.mThisId2EntityDic.Remove(entity.getThisId());
            }

            if (isDispose)
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

        public void setClientDispose(bool isDispose)
        {

        }

        public bool isClientDispose()
        {
            return false;
        }

        // 通过 Id 获取元素
        public SceneEntityBase getEntityByThisId(uint thisId)
        {
            if (this.mThisId2EntityDic.ContainsKey(thisId))
            {
                return this.mThisId2EntityDic[thisId];
            }
            return null;
        }

        // 通过 Unique Id 获取元素，Unique Id 是客户端自己的唯一 id ，与服务器没有关系
        public SceneEntityBase getEntityByUniqueId(string uniqueId)
        {
            if(this.mId2EntityDic.ContainsKey(uniqueId))
            {
                return this.mId2EntityDic[uniqueId];
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

        public string getCurStrId()
        {
            return mUniqueStrIdGen.getCurStrId();
        }

        public uint getCurId()
        {
            return mUniqueStrIdGen.getCurId();
        }

        public string genStrIdById(uint id)
        {
            return mUniqueStrIdGen.genStrIdById(id);
        }

        public int getEntityCount()
        {
            return this.mSceneEntityList.Count();
        }

        public MList<SceneEntityBase> getSceneEntityList()
        {
            return this.mSceneEntityList;
        }

        public void clearAll()
        {
            incDepth();

            int idx = 0;
            int len = this.mSceneEntityList.Count();
            SceneEntityBase entity = null;

            while (idx < len)
            {
                entity = this.mSceneEntityList[idx];

                // 必然释放
                // if (!entity.isClientDispose())
                //{
                    entity.dispose();
                //}

                ++idx;
            }

            decDepth();
        }

        public void changeThisId(uint srcThisId, uint destThisId, SceneEntityBase entity)
        {
            this.mThisId2EntityDic.Remove(srcThisId);
            this.mThisId2EntityDic[destThisId] = entity;
        }
    }
}