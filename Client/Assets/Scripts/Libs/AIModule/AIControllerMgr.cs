namespace SDK.Lib
{
    /**
     * @brief AIController 管理器
     */
    public class AIControllerMgr : EntityMgrBase
    {
        virtual protected void onTickExec(float delta)
        {
            foreach (ISceneEntity entity in m_sceneEntityList)
            {
                entity.onTick(delta);
            }
        }
    }
}