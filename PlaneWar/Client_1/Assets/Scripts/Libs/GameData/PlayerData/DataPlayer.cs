using Game.Msg;

namespace SDK.Lib
{
    /**
     * @brief 玩家基本数据
     */
    public class DataPlayer
    {
        public bool mCanReqMaidData;                    // 是否可以请求主角自己的数据，主角数据上线请求一次
        public DataPack mDataPack;            // 包裹数据
        public t_MainUserData mDataMain;// 主角自己数据
        public AccountData mAccountData;   // 主角账号数据
        public DataFriend mDataFriend;                         // 好友数据

        public DataShop mDataShop;            // 商城数据
        public DataHero mDataHero;            // hero 数据
        public ChatData mChatData;

        public DataPlayer()
        {
            this.mCanReqMaidData = true;
            this.mDataMain = new t_MainUserData();
            this.mAccountData = new AccountData();

            this.mDataShop = new DataShop();
            this.mDataHero = new DataHero();
            this.mChatData = new ChatData();
        }

        public void init()
        {
            
        }

        public void dispose()
        {

        }

        // 强求主角数据
        public void reqMainData()
        {
            if(mCanReqMaidData)
            {
                mCanReqMaidData = false;
                stReqUserBaseDataInfoPropertyUserCmd cmd = new stReqUserBaseDataInfoPropertyUserCmd();
                UtilMsg.sendMsg(cmd);               
            }
        }
    }
}