using Game.Msg;
using SDK.Common;

namespace FightCore
{
    /**
     * @brief 删除战斗项
     */
    public class FightRoundDelItem : FightRoundItemBase
    {
        protected stRetRemoveBattleCardUserCmd m_msg;
        protected int m_side;
        protected SceneCardItem m_sceneItem;

        public FightRoundDelItem(SceneDZData data) :
            base(data)
        {

        }

        override public void psstRetRemoveBattleCardUserCmd(stRetRemoveBattleCardUserCmd msg, int side, SceneCardItem sceneItem)
        {
            SceneCardBase card = Ctx.m_instance.m_sceneCardMgr.getCard(sceneItem.svrCard.qwThisID);
            card.setSvrDispose();       // 设置服务器死亡标志
            m_msg = msg;
            m_side = side;
            m_sceneItem = sceneItem;
        }

        // 执行删除
        override public void processOneAttack()
        {
            Ctx.m_instance.m_logSys.log("[Fight] 真正删除一个卡牌");

            m_sceneDZData.m_sceneDZAreaArr[m_side].removeAndDestroyOneCardByItem(m_sceneItem);
            m_OneAttackAndHurtEndDisp.dispatchEvent(this);
        }
    }
}