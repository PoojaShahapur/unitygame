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
            cmdHandle = new GameDataUserCmdHandle();
            this.addCmdHandle(stNullUserCmd.DATA_USERCMD, cmdHandle, cmdHandle.call);
            cmdHandle = new GamePropertyUserCmdHandle();
            this.addCmdHandle(stNullUserCmd.PROPERTY_USERCMD, cmdHandle, cmdHandle.call);
            cmdHandle = new GameHeroCardCmdHandle();
            this.addCmdHandle(stNullUserCmd.HERO_CARD_USERCMD, cmdHandle, cmdHandle.call);
            cmdHandle = new GameChatCmdHandle();
            this.addCmdHandle(stNullUserCmd.CHAT_USERCMD, cmdHandle, cmdHandle.call);
            cmdHandle = new GameTimeCmdHandle();
            this.addCmdHandle(stNullUserCmd.TIME_USERCMD, cmdHandle, cmdHandle.call);
        }
    }
}