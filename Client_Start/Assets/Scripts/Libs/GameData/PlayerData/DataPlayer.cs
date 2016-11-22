using Game.Msg;

namespace SDK.Lib
{
    /**
     * @brief 玩家基本数据
     */
    public class DataPlayer
    {
        public bool m_canReqMaidData = true;                    // 是否可以请求主角自己的数据，主角数据上线请求一次
        public DataPack m_dataPack = new DataPack();            // 包裹数据
        public t_MainUserData m_dataMain = new t_MainUserData();// 主角自己数据
        public AccountData m_accountData = new AccountData();   // 主角账号数据
        public DataFriend m_dataFriend;                         // 好友数据

        public DataShop m_dataShop = new DataShop();            // 商城数据
        public DataHero m_dataHero = new DataHero();            // hero 数据
        public ChatData m_chatData = new ChatData();

        // 强求主角数据
        public void reqMainData()
        {
            if(m_canReqMaidData)
            {
                m_canReqMaidData = false;
                stReqUserBaseDataInfoPropertyUserCmd cmd = new stReqUserBaseDataInfoPropertyUserCmd();
                UtilMsg.sendMsg(cmd);
                Ctx.mInstance.mLogSys.log("请求主数据");
            }
        }
    }
}