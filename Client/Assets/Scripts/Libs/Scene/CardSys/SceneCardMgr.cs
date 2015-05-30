using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 场景中所有的卡牌
     */
    public class SceneCardMgr : EntityMgrBase
    {
        public SceneCardMgr()
        {

        }

        virtual protected void onTickExec(float delta)
        {
            foreach(ISceneEntity entity in m_sceneEntityList)
            {
                entity.onTick(delta);
            }
        }
    }
}