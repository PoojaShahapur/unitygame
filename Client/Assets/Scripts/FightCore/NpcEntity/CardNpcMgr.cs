using SDK.Common;
namespace FightCore
{
    public class CardNpcMgr
    {
        protected SceneDZData m_sceneDZData;

        public RoundBtn m_roundBtn;             // 翻转按钮，结束当前一局
        public LuckCoinCard m_luckCoin;         // 对战场景中的幸运币
        public SelfRoundTip m_selfRoundTip;         // 自己回合提示
        public SelfCardFullTip m_selfCardFullTip;   // 自己卡牌满

        public CardNpcMgr(SceneDZData sceneDZData_)
        {
            m_sceneDZData = sceneDZData_;
            m_roundBtn = new RoundBtn();
            m_luckCoin = new LuckCoinCard();
            m_selfRoundTip = new SelfRoundTip();
            m_selfCardFullTip = new SelfCardFullTip();
        }

        public void findWidget()
        {
            m_roundBtn.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.TurnBtn));
            m_roundBtn.m_sceneDZData = m_sceneDZData;
            //m_luckCoin.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.LuckyCoin));
            m_selfRoundTip.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfTurnTip));
            m_selfCardFullTip.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfCardFullTip));
            m_selfCardFullTip.desc = new AuxLabel(m_selfCardFullTip.gameObject(), CVSceneDZPath.SelfCardFullTipText);
            m_selfCardFullTip.hide();
        }

        public void dispose()
        {
            m_roundBtn.dispose();
            m_selfRoundTip.dispose();
            m_selfCardFullTip.dispose();
        }
    }
}