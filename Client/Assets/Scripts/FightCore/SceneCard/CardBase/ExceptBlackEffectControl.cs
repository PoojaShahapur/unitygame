using SDK.Lib;

namespace FightCore
{
    public class ExceptBlackEffectControl : EffectControl
    {
        public ExceptBlackEffectControl(SceneCardBase rhv) :
            base(rhv)
        {

        }

        // 每一次回合开始就更新牌的状态，双方的场牌，英雄卡、技能卡
        override public void updateStateEffect()
        {
            LinkEffect effect = null;
            int idx = 0;
            TableStateItemBody stateTabelItem = null;
            for (idx = 1; idx < (int)StateID.CARD_STATE_MAX; ++idx)
            {
                if (UtilMath.checkState((StateID)idx, m_card.sceneCardItem.svrCard.state))   // 如果这个状态改变
                {
                    stateTabelItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_STATE, (uint)idx).m_itemBody as TableStateItemBody;
                    if (stateTabelItem.m_effectId > 0)
                    {
                        effect = m_card.effectControl.startStateEffect((StateID)idx, stateTabelItem.m_effectId);
                    }
                }
                else    // 删除状态，停止特效
                {
                    m_card.effectControl.stopStateEffect((StateID)idx);
                }
            }
        }

        // 每一个状态对应一个特效，这个特效是循环特效，每一次战斗都要更新状态，双方的场牌，英雄卡、技能卡
        override public void updateAttHurtStateEffect(FightItemBase item)
        {
            LinkEffect effect = null;

            if (item.bStateChange())       // 每一个状态对应一个特效，需要播放特效
            {
                int idx = 0;
                TableStateItemBody stateTabelItem = null;
                for (idx = 1; idx < (int)StateID.CARD_STATE_MAX; ++idx)
                {
                    if (UtilMath.checkState((StateID)idx, item.changedState))   // 如果这个状态改变
                    {
                        if (UtilMath.checkState((StateID)idx, item.curState))   // 如果是增加状态
                        {
                            stateTabelItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_STATE, (uint)idx).m_itemBody as TableStateItemBody;
                            if (stateTabelItem.m_effectId > 0)
                            {
                                effect = m_card.effectControl.startStateEffect((StateID)idx, stateTabelItem.m_effectId);
                            }
                        }
                        else    // 删除状态，停止特效
                        {
                            m_card.effectControl.stopStateEffect((StateID)idx);
                        }
                    }
                }
            }
        }
    }
}