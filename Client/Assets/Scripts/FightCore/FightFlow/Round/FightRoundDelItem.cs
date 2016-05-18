using Game.Msg;
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
        protected SceneCardBase m_card;

        public FightRoundDelItem(SceneDZData data) :
            base(data)
        {
            m_parallelFlag = FightExecParallelMask.eDie;
            UtilMath.setState((int)FightExecParallelMask.eDie, ref m_parallelMask);         // 死亡是可以和死亡并行执行的
        }

        override public void psstRetRemoveBattleCardUserCmd(stRetRemoveBattleCardUserCmd msg, int side, SceneCardItem sceneItem)
        {
            m_msg = msg;
            m_side = side;
            m_sceneItem = sceneItem;

            m_card = Ctx.m_instance.m_sceneCardMgr.getCardByThisId(m_msg.dwThisID);
            m_card.setSvrDispose();       // 设置服务器死亡标志
        }

        // 执行删除
        override public void processOneAttack()
        {
            Ctx.m_instance.m_logSys.fightLog(string.Format("[Fight] 开始处理死亡 {0}", m_card.getDesc()));
            // 死亡
            DieItem dieItem = null;
            dieItem = m_card.fightData.hurtData.createItem(EHurtType.eDie) as DieItem;
            dieItem.initDieItemData(m_card, m_msg);
            m_card.fightData.hurtData.allHurtExecEndDisp.uniqueId = UniqueId.DEBUG_ID_1;
            m_card.fightData.hurtData.allHurtExecEndDisp.addEventHandle(null, onDieEndHandle);
        }

        protected void onDieEndHandle(IDispatchObject dispObj)
        {
            // 删除死亡对象
            Ctx.m_instance.m_logSys.fightLog(string.Format("[Fight] 真正删除一个卡牌 {0}", m_card.getDesc()));
            //m_sceneDZData.m_sceneDZAreaArr[m_side].removeAndDestroyOneCardByItem(m_sceneItem);
            m_card.dispose();
            m_card = null;
            m_OneAttackAndHurtEndDisp.dispatchEvent(this);
        }
    }
}