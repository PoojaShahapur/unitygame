using Game.Msg;
using SDK.Common;
using SDK.Lib;

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
            m_msg = msg;
            m_side = side;
            m_sceneItem = sceneItem;

            SceneCardBase card = Ctx.m_instance.m_sceneCardMgr.getCardByThisId(m_msg.dwThisID);
            card.setSvrDispose();       // 设置服务器死亡标志
        }

        // 执行删除
        override public void processOneAttack()
        {
            Ctx.m_instance.m_logSys.log("[Fight] 开始处理死亡");

            SceneCardBase card = Ctx.m_instance.m_sceneCardMgr.getCardByThisId(m_msg.dwThisID);
            // 死亡
            DieItem dieItem = null;
            dieItem = card.fightData.hurtData.createItem(EHurtType.eDie) as DieItem;
            dieItem.initDieItemData(card, m_msg);
            card.fightData.hurtData.allHurtExecEndDisp.addEventHandle(onDieEndHandle);
        }

        protected void onDieEndHandle(IDispatchObject dispObj)
        {
            // 删除死亡对象
            Ctx.m_instance.m_logSys.log("[Fight] 真正删除一个卡牌");
            m_sceneDZData.m_sceneDZAreaArr[m_side].removeAndDestroyOneCardByItem(m_sceneItem);
            m_OneAttackAndHurtEndDisp.dispatchEvent(this);
        }
    }
}