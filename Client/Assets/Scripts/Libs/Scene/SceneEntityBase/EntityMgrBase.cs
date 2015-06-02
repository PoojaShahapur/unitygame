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

        public void add2List(SceneEntityBase entity)
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

        public void removeFromeList(SceneEntityBase entity)
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

        }

        virtual protected void onTickEnd()
        {
            processScheduledObjects();
        }
    }
}