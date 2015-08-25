using Game.Msg;
using Game.UI;
using SDK.Lib;
using System;
using System.Collections.Generic;

namespace FightCore
{
    public enum EnGameOp
    {
        eOpNone,        // 没进行任何操作
        eOpNormalAttack,    // 攻击操作
        eOpFaShu,           // 法术操作，这个操作是指法术牌，并且有攻击目标，如果没有攻击目标，就直接作为普通出牌操作了
        eOpZhanHouAttack,   // 战吼攻击，这个操作是指战吼牌，并且有攻击目标，如果没有攻击目标，就直接作为普通出牌操作了
        eOpMoveIn2Out,      // 移动卡牌从手牌区域到卡牌区域
        eOpSkillAttackTarget,   // 技能攻击需要攻击目录
        eOpTotal
    }

    /**
     * @brief 游戏当前操作状态，仅处理场景中交互的内容，如果触摸到不交互的场景的内容，不打断场景逻辑
     */
    public class GameOpState
    {
        protected SceneDZData m_sceneDZData;

        protected EnGameOp m_curOp;         // 当前操作
        protected SceneCardBase m_opCard;      // 当前操作的卡牌

        public GameOpState(SceneDZData sceneDZData)
        {
            m_curOp = EnGameOp.eOpNone;
            m_sceneDZData = sceneDZData;
        }

        public EnGameOp curOp
        {
            get
            {
                return m_curOp;
            }
        }

        public SceneCardBase curCard
        {
            get
            {
                return m_opCard;
            }
        }

        // 进入攻击操作
        public void enterAttackOp(EnGameOp op, SceneCardBase card)
        {
            // 进入操作后，需要禁止手牌区域卡牌拖动操作
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].disableAllInCardDragExceptOne(card as SceneCardBase);

            // 如果不是自己的回合，直接返回
            if (!Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
            {
                return;
            }
            // 进入某一个状态的时候，要查看之前的状态是否需要需要处理
            checkPreAttackOp(op, m_opCard);

            m_curOp = op;
            m_opCard = card;
            // 开始拖动箭头
            m_sceneDZData.m_attackArrow.startArrow();

            addAttackTargetFlags();
        }

        // 检查之前的攻击状态
        public void checkPreAttackOp(EnGameOp op, SceneCardBase card)
        {
            if (EnGameOp.eOpZhanHouAttack == m_curOp)   // 如果是战吼
            {
                // 需要将其回退回去
                m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].putHandFromOutByCard(m_opCard);
            }
            else if(EnGameOp.eOpFaShu == m_curOp)       // 法术
            {
                if (card.sceneCardItem.m_cardTableItem.m_bNeedFaShuTarget > 0)         // 如果有攻击目标
                {
                    m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].cancelFashuOp(m_opCard);
                }
            }
        }

        // 这个说明攻击操作完成
        public void endAttackOp()
        {
            m_curOp = EnGameOp.eOpNone;
            m_opCard = null;
            m_sceneDZData.m_attackArrow.stopArrow();
            clearAttackTargetFlags();

            // 退出操作后，需要开启手牌区域卡牌拖动操作
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].enableAllInCardDragExceptOne(null);
        }

        // 取消攻击操作
        public void cancelAttackOp()
        {
            if (m_opCard != null)
            {
                checkPreAttackOp(m_curOp, m_opCard);
                endAttackOp();
            }
        }

        // 判断是否在某个操作中
        public bool bInOp(EnGameOp op)
        {
            return op == m_curOp;
        }

        public bool canAttackOp(SceneCardBase card, EnGameOp gameOp)
        {
            bool ret = false;
            if (m_opCard != null)
            {
                if (gameOp == m_curOp)  // 如果当前处于这个操作状态
                {
                    if (gameOp == EnGameOp.eOpNormalAttack)  // 如果当前处于攻击
                    {
                        ret = canNormalAttack(card, gameOp);
                    }
                    else if (gameOp == EnGameOp.eOpFaShu || gameOp == EnGameOp.eOpSkillAttackTarget)  // 当前处于法术牌攻击
                    {
                        ret = canFaShuAttack(card, gameOp);
                    }
                    else if (gameOp == EnGameOp.eOpZhanHouAttack)  // 当前处于战吼牌攻击
                    {
                        ret = canZhanHouAttack(card, gameOp);
                    }
                }
            }

            return ret;
        }

        public void enterMoveOp(SceneCardBase card)
        {
            // 进入操作后，需要禁止手牌区域卡牌拖动操作
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].disableAllInCardDragExceptOne(card as SceneCardBase);

            m_curOp = EnGameOp.eOpMoveIn2Out;
            m_opCard = card;
        }

        public void quitMoveOp()
        {
            m_curOp = EnGameOp.eOpNone;
            m_opCard = null;

            // 退出操作后，需要开启手牌区域卡牌拖动操作
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].enableAllInCardDragExceptOne(null);
        }

        protected bool canNormalAttack(SceneCardBase card, EnGameOp gameOp)
        {
            //if (m_opCard.sceneCardItem.m_playerSide != card.sceneCardItem.m_playerSide && !UtilMath.checkState(StateID.CARD_STATE_SLEEP, card.sceneCardItem.m_svrCard.state))
            //{
            //    return true;
            //}

            //return false;
            bool ret = false;
            stCardAttackMagicUserCmd cmd = new stCardAttackMagicUserCmd();
            cmd.dwAttThisID = m_opCard.sceneCardItem.svrCard.qwThisID;
            cmd.dwDefThisID = card.sceneCardItem.svrCard.qwThisID;
            cmd.dwMagicType = (uint)m_opCard.sceneCardItem.m_cardTableItem.m_faShu;
            ret = Ctx.m_instance.m_dataPlayer.m_dzData.cardAttackMagic(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_opCard.sceneCardItem.playerSide], cmd);

            if(ret)
            {
                (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg("Client 普通攻击验证通过");
            }

            return ret;
        }

        protected bool canFaShuAttack(SceneCardBase card, EnGameOp gameOp)
        {
            return canSkillAttack(card, gameOp, false); 
        }

        protected bool canZhanHouAttack(SceneCardBase card, EnGameOp gameOp)
        {
            return canSkillAttack(card, gameOp, true);
        }

        protected bool canSkillAttack(SceneCardBase card, EnGameOp gameOp, bool bZhanHou)
        {
            stCardAttackMagicUserCmd cmd = new stCardAttackMagicUserCmd();
            cmd.dwAttThisID = m_opCard.sceneCardItem.svrCard.qwThisID;
            cmd.dwDefThisID = card.sceneCardItem.svrCard.qwThisID;

            int attackTarget = 0;

            if (bZhanHou)
            {
                cmd.bZhanHou = true;
                attackTarget = m_opCard.sceneCardItem.m_cardTableItem.m_bNeedZhanHouTarget;
                cmd.dwMagicType = (uint)m_opCard.sceneCardItem.m_cardTableItem.m_zhanHou;
            }
            else
            {
                cmd.bZhanHou = false;
                attackTarget = m_opCard.sceneCardItem.m_cardTableItem.m_bNeedFaShuTarget;
                cmd.dwMagicType = (uint)m_opCard.sceneCardItem.m_cardTableItem.m_faShu;
            }

            if (Ctx.m_instance.m_dataPlayer.m_dzData.cardAttackMagic(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_opCard.sceneCardItem.playerSide], cmd))
            {
                if (UtilMath.checkAttackState(AttackTarget.ATTACK_TARGET_SHERO, (uint)attackTarget))
                {
                    if (EnDZPlayer.ePlayerSelf == card.sceneCardItem.playerSide)       // 如果是自己
                    {
                        if (CardArea.CARDCELLTYPE_HERO == card.sceneCardItem.cardArea)     // 如果是主角
                        {
                            (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg("Client 法术攻击验证通过");
                            return true;
                        }
                    }
                }
                if (UtilMath.checkAttackState(AttackTarget.ATTACK_TARGET_SATTEND, (uint)attackTarget))
                {
                    if (EnDZPlayer.ePlayerSelf == card.sceneCardItem.playerSide)       // 如果是自己
                    {
                        if (CardArea.CARDCELLTYPE_COMMON == card.sceneCardItem.cardArea)     // 如果是出牌区
                        {
                            (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg("Client 法术攻击验证通过");
                            return true;
                        }
                    }
                }
                if (UtilMath.checkAttackState(AttackTarget.ATTACK_TARGET_EHERO, (uint)attackTarget))
                {
                    if (EnDZPlayer.ePlayerEnemy == card.sceneCardItem.playerSide)       // 如果是 enemy
                    {
                        if (CardArea.CARDCELLTYPE_HERO == card.sceneCardItem.cardArea)     // 如果是主角
                        {
                            (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg("Client 法术攻击验证通过");
                            return true;
                        }
                    }
                }
                if (UtilMath.checkAttackState(AttackTarget.ATTACK_TARGET_EATTEND, (uint)attackTarget))
                {
                    if (EnDZPlayer.ePlayerEnemy == card.sceneCardItem.playerSide)       // 如果是 enemy
                    {
                        if (CardArea.CARDCELLTYPE_COMMON == card.sceneCardItem.cardArea)     // 如果是出牌区
                        {
                            (Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat).outMsg("Client 法术攻击验证通过");
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public uint getOpCardID()
        {
            if(m_opCard != null)
            {
                return m_opCard.sceneCardItem.svrCard.qwThisID;
            }

            return 0;
        }

        public int getOpCardFaShu()
        {
            if (m_opCard != null)
            {
                return m_opCard.sceneCardItem.m_cardTableItem.m_faShu;
            }

            return 0;
        }

        public int getZhanHouCommonClientIdx()
        {
            return m_opCard.getZhanHouCommonClientIdx();
        }

        // 可以攻击的目标显示效果，发送攻击消息的时候去掉显示
        protected void addAttackTargetFlags()
        {
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].updateCanLaunchAttState(false);
            // 遍历所有对象，包括 Enemy 的场牌、Enemy 的英雄、Enemy 的 Skill、自己的场牌、自己的英雄卡、自己的技能卡
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].updateCardAttackedState(this);
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].updateCardAttackedState(this);
        }

        // 清除可攻击的标识
        protected void clearAttackTargetFlags()
        {
            // 遍历所有的 enemy 对象
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].clearCardAttackedState();
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].clearCardAttackedState();
            if (Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
            {
                m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].updateCanLaunchAttState(true);
            }
        }
    }
}