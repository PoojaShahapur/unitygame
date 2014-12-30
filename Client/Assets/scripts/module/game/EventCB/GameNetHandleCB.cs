using SDK.Common;

namespace Game.Game
{
    /**
     * @brief 游戏网络处理
     */
    public class GameNetHandleCB : NetDispHandle
    {
        public GameNetHandleCB()
        {
            m_id2DispDic[stNullUserCmd.DATA_USERCMD] = new GameDataUserCmdHandle();
            m_id2DispDic[stNullUserCmd.PROPERTY_USERCMD] = new GamePropertyUserCmdHandle();
        }
    }
}