using SDK.Common;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class EntityMgrBase : DelayHandleMgrBase, ITickedObject, IDelayHandleItem
    {
        protected List<SceneEntityBase> m_sceneEntityList;

        public EntityMgrBase()
        {
            m_sceneEntityList = new List<SceneEntityBase>();
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

        virtual public void OnTick(float delta)
        {
            incDepth();

            onTickExec(delta);

            decDepth();
        }

        virtual protected void onTickExec(float delta)
        {
            foreach (SceneEntityBase entity in m_sceneEntityList)
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

        public SceneEntity getEntityByThisId(uint thisId)
        {
            return null;
        }
    }
}