namespace SDK.Common
{
    /**
     * @brief 玩家基本数据
     */
    public class DataPlayer
    {
        public DataPack m_dataPack = new DataPack();            // 包裹数据
        public DataCard m_dataCard = new DataCard();            // 卡牌数据
        public t_MainUserData m_dataMain = new t_MainUserData();// 主角自己数据
        public DataFriend m_dataFriend;                         // 好友数据
    }
}