using SDK.Lib;

namespace Game.Game
{
    /**
     * @brief 游戏网络处理
     */
    public class GameNetHandleCB : NetDispHandle
    {
        public GameNetHandleCB()
        {
            NetCmdHandleBase cmdHandle;
            cmdHandle = new GameTimeCmdHandle();
            this.addCmdHandle(stNullUserCmd.TIME_USERCMD, cmdHandle, cmdHandle.call);
        }
    }
}