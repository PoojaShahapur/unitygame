using SDK.Common;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class EntityMgrBase : ITickedObject
    {
        protected List<ISceneEntity> m_sceneEntityList;

        public EntityMgrBase()
        {
            m_sceneEntityList = new List<ISceneEntity>();
        }

        public void add2List(ISceneEntity entity)
        {
            m_sceneEntityList.Add(entity);
        }

        public void removeFromeList(ISceneEntity entity)
        {
            m_sceneEntityList.Remove(entity);
        }

        virtual public void OnTick(float delta)
        {
            
        }
    }
}