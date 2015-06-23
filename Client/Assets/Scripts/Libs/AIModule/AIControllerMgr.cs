using SDK.Lib;

namespace BehaviorLibrary
{
    /**
     * @brief AIController 管理器
     */
    public class AIControllerMgr : EntityMgrBase
    {
        override protected void onTickExec(float delta)
        {
            foreach (SceneEntityBase entity in m_sceneEntityList)
            {
                if (!entity.getClientDispose())
                {
                    entity.onTick(delta);
                }
            }
        }

        public void addController(AIController ai)
        {
            this.addObject(ai);
        }

        public void removeController(AIController ai)
        {
            this.delObject(ai);
        }
    }
}