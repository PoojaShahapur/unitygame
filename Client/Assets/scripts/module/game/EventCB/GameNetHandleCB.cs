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
            m_id2DispDic[stNullUserCmd.HERO_CARD_USERCMD] = new GameHeroCardCmdHandle();
            m_id2DispDic[stNullUserCmd.CHAT_USERCMD] = new GameChatCmdHandle();
            m_id2DispDic[stNullUserCmd.TIME_USERCMD] = new GameTimeCmdHandle();
        }
    }
}