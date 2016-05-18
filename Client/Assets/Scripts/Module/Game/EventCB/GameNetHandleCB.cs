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
            this.addCmdHandle(stNullUserCmd.DATA_USERCMD, new GameDataUserCmdHandle());
            this.addCmdHandle(stNullUserCmd.PROPERTY_USERCMD, new GamePropertyUserCmdHandle());
            this.addCmdHandle(stNullUserCmd.HERO_CARD_USERCMD, new GameHeroCardCmdHandle());
            this.addCmdHandle(stNullUserCmd.CHAT_USERCMD, new GameChatCmdHandle());
            this.addCmdHandle(stNullUserCmd.TIME_USERCMD, new GameTimeCmdHandle());
        }
    }
}