namespace SDK.Lib
{
    public class EntityMgrBase : DelayHandleMgrBase, ITickedObject, IDelayHandleItem
    {
        protected MList<SceneEntityBase> m_sceneEntityList;

        public EntityMgrBase()
        {
            m_sceneEntityList = new MList<SceneEntityBase>();
        }

        override public void addObject(IDelayHandleItem entity, float priority = 0.0f)
        {
            if (bInDepth())
            {
                base.addObject(entity);
            }
            else
            {
                m_sceneEntityList.Add(entity as SceneEntityBase);
            }
        }

        override public void delObject(IDelayHandleItem entity)
        {
            if (bInDepth())
            {
                base.delObject(entity);
            }
            else
            {
                m_sceneEntityList.Remove(entity as SceneEntityBase);
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
                if (!entity.getClientDispose())
                {
                    entity.onTick(delta);
                }
            }
        }

        public void setClientDispose()
        {

        }

        public bool getClientDispose()
        {
            return false;
        }

        public SceneEntityBase getEntityByThisId(uint thisId)
        {
            return null;
        }
    }
}