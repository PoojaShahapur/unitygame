using SDK.Lib;

namespace Game.Game
{
    /**
     * @brief 游戏网络处理
     */
    public class GameNetHandleCB : NetModuleDispHandle
    {
        public GameNetHandleCB()
        {
            NetCmdDispHandle cmdHandle = null;
            cmdHandle = new GameTimeCmdHandle();
            this.addCmdHandle(stNullUserCmd.TIME_USERCMD, cmdHandle, cmdHandle.call);
        }
    }
}