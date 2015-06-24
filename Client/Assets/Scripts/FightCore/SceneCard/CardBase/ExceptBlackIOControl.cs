using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    public class ExceptBlackIOControl : IOControlBase
    {
        public ExceptBlackIOControl(SceneCardBase rhv) : 
            base(rhv)
        {
            
        }

        public override void init()
        {
            base.init();
            this.m_card.clickEntityDisp.addEventHandle(onCardClick);
        }

        // 英雄卡、随从卡即可以作为攻击者也可以作为被击者，法术卡、技能卡、装备卡只能作为攻击者
        override public void onCardClick(IDispatchObject dispObj)
        {
            if (!m_card.m_sceneDZData.m_gameRunState.isInState(GameRunState.INITCARD))      // 如果处于初始化卡牌阶段
            {
                if (EnDZPlayer.ePlayerSelf != m_card.sceneCardItem.playerSide || CardArea.CARDCELLTYPE_HAND != m_card.sceneCardItem.cardArea)         // 如果点击的不是自己的手牌
                {
                    if (m_card.sceneCardItem != null)
                    {
                        if (m_card.m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpNormalAttack))     // 选择攻击目标
                        {
                            if (m_card.m_sceneDZData.m_gameOpState.canAttackOp(m_card, EnGameOp.eOpNormalAttack))
                            {
                                // 发送攻击指令
                                stCardAttackMagicUserCmd cmd = new stCardAttackMagicUserCmd();
                                cmd.dwAttThisID = m_card.m_sceneDZData.m_gameOpState.getOpCardID();
                                cmd.dwDefThisID = m_card.sceneCardItem.svrCard.qwThisID;
                                UtilMsg.sendMsg(cmd);

                                //m_sceneDZData.m_gameOpState.quitAttackOp(false);
                            }
                            else
                            {
                                m_card.behaviorControl.enterAttack();
                            }
                        }
                        else if ((m_card.m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpFaShu)))        // 法术攻击
                        {
                            if (m_card.m_sceneDZData.m_gameOpState.canAttackOp(m_card, EnGameOp.eOpFaShu))     // 选择攻击目标
                            {
                                // 必然是有目标的法术攻击
                                // 发送法术攻击消息
                                stCardMoveAndAttackMagicUserCmd cmd = new stCardMoveAndAttackMagicUserCmd();
                                cmd.dwAttThisID = m_card.m_sceneDZData.m_gameOpState.getOpCardID();
                                cmd.dwMagicType = (uint)m_card.m_sceneDZData.m_gameOpState.getOpCardFaShu();
                                cmd.dwDefThisID = m_card.sceneCardItem.svrCard.qwThisID;
                                //m_sceneDZData.m_gameOpState.quitAttackOp(false);
                                UtilMsg.sendMsg(cmd);
                            }
                            else
                            {
                                m_card.behaviorControl.enterAttack();
                            }
                        }
                        else if (m_card.m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpZhanHouAttack))      // 战吼攻击
                        {
                            if (m_card.m_sceneDZData.m_gameOpState.canAttackOp(m_card, EnGameOp.eOpZhanHouAttack))     // 选择攻击目标
                            {
                                // 发送攻击指令
                                stCardMoveAndAttackMagicUserCmd cmd = new stCardMoveAndAttackMagicUserCmd();
                                cmd.dwAttThisID = m_card.m_sceneDZData.m_gameOpState.getOpCardID();
                                cmd.dwMagicType = (uint)m_card.m_sceneDZData.m_gameOpState.getOpCardFaShu();
                                cmd.dwDefThisID = m_card.sceneCardItem.svrCard.qwThisID;
                                cmd.dst = new stObjectLocation();
                                cmd.dst.dwLocation = (uint)m_card.sceneCardItem.cardArea;
                                cmd.dst.y = m_card.curIndex;
                                UtilMsg.sendMsg(cmd);

                                //m_sceneDZData.m_gameOpState.quitAttackOp();
                            }
                            else
                            {
                                m_card.behaviorControl.enterAttack();
                            }
                        }
                        else if (m_card.m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpSkillAttackTarget))      // 技能目标攻击
                        {
                            if (m_card.m_sceneDZData.m_gameOpState.canAttackOp(m_card, EnGameOp.eOpSkillAttackTarget))     // 选择攻击目标
                            {
                                // 必然是有目标的法术攻击
                                // 发送法术攻击消息
                                stCardMoveAndAttackMagicUserCmd cmd = new stCardMoveAndAttackMagicUserCmd();
                                cmd.dwAttThisID = m_card.m_sceneDZData.m_gameOpState.getOpCardID();
                                cmd.dwMagicType = (uint)m_card.m_sceneDZData.m_gameOpState.getOpCardFaShu();
                                cmd.dwDefThisID = m_card.sceneCardItem.svrCard.qwThisID;
                                UtilMsg.sendMsg(cmd);
                            }
                        }
                        else        // 默认点击处理都走这里
                        {
                            m_card.behaviorControl.enterAttack();
                        }
                    }
                }
            }
        }

        // 输入按下
        override public void onCardDown(IDispatchObject dispObj)
        {
            m_card.m_sceneDZData.m_watchCardInfo.startWatch(m_card);
        }

        // 输入释放
        override public void onCardUp(IDispatchObject dispObj)
        {
            m_card.m_sceneDZData.m_watchCardInfo.stopWatch();
        }

        // 结束转换模型
        override public void endConvModel(int type)
        {
            this.m_card.clickEntityDisp.addEventHandle(onCardClick);
        }
    }
}