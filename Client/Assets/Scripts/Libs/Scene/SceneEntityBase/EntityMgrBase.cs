using SDK.Common;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class EntityMgrBase : DelayHandleMgrBase, ITickedObject
    {
        protected List<SceneEntityBase> m_sceneEntityList;

        public EntityMgrBase()
        {
            m_sceneEntityList = new List<SceneEntityBase>();
        }

        override public void addObject(IDelayHandleItem entity, float priority = 0.0f)
        {
            if (m_duringAdvance)
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
            if (m_duringAdvance)
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
            m_duringAdvance = true;
            onTickExec(delta);
            m_duringAdvance = false;
            onTickEnd();
        }

        virtual protected void onTickExec(float delta)
        {

        }

        virtual protected void onTickEnd()
        {
            processScheduledObjects();
        }
    }
}