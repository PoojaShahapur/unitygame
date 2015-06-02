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
                entity.onTick(delta);
            }
        }
    }
}