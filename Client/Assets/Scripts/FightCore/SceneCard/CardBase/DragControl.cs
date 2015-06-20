using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 场景中可拖放的卡牌，仅限场景中可拖放的卡牌，只有手里卡牌区域中的卡牌可以拖放，其它的都是不能拖放的，目前装备、技能、手牌、出牌都是用这个类， hero 使用另外一个
     */
    public class DragControl : CardControlBase
    {
        public const uint WHITECARDID = 10000;
        // 拖动中分发
        public Action m_moveDisp;
        public Vector3 m_centerPos;         // 中心点位置
        public float m_radius = 1;              // 半径
        public bool m_isCalc;               // 本次是否计算过超过圆形区域的处理

        public DragControl(SceneCardBase rhv) : 
            base(rhv)
        {
            
        }

        public override void init()
        {
            base.init();

            if (!m_card.m_sceneDZData.m_gameRunState.isInState(GameRunState.INITCARD))     // 初始化卡牌阶段是不能拖动的
            {
                enableDrag();
            }
        }

        // 能拖动的必然所有的操作都完成后才能操作
        protected void onStartDrag()
        {
            // 保存当前操作的卡牌
            m_card.m_sceneDZData.m_curDragItem = m_card;     // 设置当前拖放的目标
            // 开始拖动动画
            m_card.m_sceneDZData.m_curDragItem.trackAniControl.startDragAni();

            // 判断法术攻击
            //if (m_sceneCardItem != null)
            //{
                //if (m_sceneCardItem.m_cardTableItem.m_type == (int)CardType.CARDTYPE_MAGIC)   // 如果是法术牌，拖动时发起攻击
                //{
                //    if (this.m_sceneCardItem.m_cardArea == CardArea.CARDCELLTYPE_HAND)
                //    {
                //        // 只有点击自己的时候，才启动攻击
                //        if (m_sceneCardItem.m_playerSide == EnDZPlayer.ePlayerSelf)
                //        {
                //            m_sceneDZData.m_gameOpState.enterAttackOp(EnGameOp.eOpFaShu, this);
                //        }
                //    }
                //}
                //else
                //{
                //    m_sceneDZData.m_gameOpState.quitAttackOp();     // 退出之前可能的攻击状态
                //}
            //}
        }

        // 关闭拖放功能
        virtual public void disableDrag()
        {
            UIDragObject drag = m_card.gameObject().GetComponent<UIDragObject>();
            //drag.enabled = false;
            UtilApi.Destroy(drag);
            WindowDragTilt dragTitle = m_card.gameObject().GetComponent<WindowDragTilt>();
            //dragTitle.enabled = false;
            UtilApi.Destroy(dragTitle);
        }

        // 开启拖动
        virtual public void enableDrag()
        {
            if (m_card.gameObject().GetComponent<UIDragObject>() == null)
            {
                UIDragObject drag = m_card.gameObject().AddComponent<UIDragObject>();
                drag.target = m_card.gameObject().transform;
                drag.m_startDragDisp = onStartDrag;
                drag.m_moveDisp = onMove;
                drag.m_dropEndDisp = onDragEnd;
                drag.m_canMoveDisp = canMove;
            }
            if (m_card.gameObject().GetComponent<WindowDragTilt>() == null)
            {
                m_card.gameObject().AddComponent<WindowDragTilt>();
            }
        }

        // 指明当前是否可以改变位置
        protected bool canMove()
        {
            if (m_card.m_sceneDZData.m_gameRunState.isInState(GameRunState.INITCARD))      // 如果处于初始化卡牌阶段
            {
                return false;
            }

            // 如果出牌区域卡牌已经满了，也是不能移动的
            if (m_card.m_sceneDZData.m_sceneDZAreaArr[(int)m_card.sceneCardItem.m_playerSide].bOutAreaCardFull())
            {
                return false;
            }

            if (!m_card.m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpNone))        // 如果当前有操作，也不能进行拖动
            {
                return false;
            }

            //if (m_sceneCardItem != null)
            //{
            //    if (m_sceneCardItem.m_cardArea == CardArea.CARDCELLTYPE_HAND && m_sceneCardItem.m_cardTableItem.m_type == (int)CardType.CARDTYPE_MAGIC && m_sceneCardItem.m_cardTableItem.m_bNeedFaShuTarget > 0)       // 如果是法术牌，并且有攻击目标，暂时不做效果，直接不能拖动
            //    {
            //        return false;
            //    }
            //    return true;
            //}

            return true;
        }

        protected void onMove()
        {
            // 这个地方需要判断不是自己回合在允许的范围内
            if (!m_isCalc)
            {
                if (!bInSafeTyArea())
                {
                    m_isCalc = true;

                    if (!Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())        // 不是自己回合
                    {
                        Ctx.m_instance.m_logSys.log(Ctx.m_instance.m_langMgr.getText(LangTypeId.eDZ4, LangItemID.eItem10));
                        backCard2Orig();
                        m_isCalc = false;
                    }
                    else if (m_card.sceneCardItem.svrCard.mpcost > Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_card.sceneCardItem.m_playerSide].m_heroMagicPoint.mp)   // Mp 不够
                    {
                        Ctx.m_instance.m_logSys.log(Ctx.m_instance.m_langMgr.getText(LangTypeId.eDZ4, LangItemID.eItem11));
                        backCard2Orig();
                        m_isCalc = false;
                    }
                }
            }

            // 如果不是自己出牌或者拖动的是装备卡牌，就不用通知其它的移动了
            if(Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
            {
                if (m_card.sceneCardItem != null)
                {
                    if (m_card.sceneCardItem.m_cardTableItem.m_type != (int)CardType.CARDTYPE_EQUIP)        // 如果拖动的手里的牌不是装备牌
                    {
                        if (m_moveDisp != null)
                        {
                            m_moveDisp();
                        }
                    }
                }
            }
        }

        // 拖放结束处理
        protected void onDragEnd()
        {
            m_isCalc = false;

            // 如果在一定小范围内
            if (bInSafeTyArea())
            {
                // 拖动结束直接退回去
                // 开始缩放
                //m_card.trackAniControl.destScale = SceneDZCV.SMALLFACT;
                backCard2Orig();
            }
            else
            {
            #if DEBUG_NOTNET
                m_card.trackAniControl.endDragAni();       // 结束动画
                m_card.m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].addCardToOutList(m_card);        // 放入输出列表
            #endif
                if (m_card.sceneCardItem != null)
                {
                    if (m_card.sceneCardItem.svrCard != null)
                    {
                        // 法术攻击一次必然要消失
                        if (m_card.sceneCardItem.m_cardTableItem.m_type == (int)CardType.CARDTYPE_MAGIC)   // 如果是法术牌
                        {
                            if (m_card.sceneCardItem.m_cardTableItem.m_bNeedFaShuTarget > 0)         // 如果有攻击目标
                            {
                                // 直接放下去，然后选择攻击目标
                                m_card.m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].outSceneCardList.removeWhiteCard();       // 将占位的牌移除
                                m_card.hide();      // 隐藏起来
                                // 英雄播放攻击准备特效
                                if (m_card.sceneCardItem.m_cardTableItem.m_skillPrepareEffect > 0)
                                {
                                    m_card.m_sceneDZData.m_sceneDZAreaArr[(int)(m_card.sceneCardItem.m_playerSide)].centerHero.effectControl.startSkillAttPrepareEffect((int)m_card.sceneCardItem.m_cardTableItem.m_skillPrepareEffect);
                                }
                                m_card.m_sceneDZData.m_gameOpState.enterAttackOp(EnGameOp.eOpFaShu, m_card);
                            }
                            else        // 如果没有攻击目标，直接拖出去
                            {
                                // 发送法术攻击消息
                                stCardMoveAndAttackMagicUserCmd cmd = new stCardMoveAndAttackMagicUserCmd();
                                cmd.dwAttThisID = m_card.sceneCardItem.svrCard.qwThisID;
                                cmd.dwMagicType = (uint)m_card.sceneCardItem.m_cardTableItem.m_faShu;
                                UtilMsg.sendMsg(cmd);

                                m_card.m_sceneDZData.m_gameOpState.enterMoveOp(m_card);      // 进入移动操作
                            }
                        }
                        else if (m_card.sceneCardItem.m_cardTableItem.m_zhanHou > 0 && m_card.sceneCardItem.m_cardTableItem.m_bNeedZhanHouTarget > 0)           // 如果有战吼，并且需要攻击目标
                        {
                            // 直接放下去，然后选择攻击目标
                            m_card.m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].outSceneCardList.removeWhiteCard();       // 将占位的牌移除
                            m_card.m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].removeFormInList(m_card);     // 从手牌区移除卡牌
                            m_card.m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].addCardToOutList(m_card, m_card.m_sceneDZData.curWhiteIdx);
                            m_card.convOutModel();
                            m_card.m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].inSceneCardList.updateSceneCardPos(false);       // 仅仅更新位置信息，不更新索引信息，因为卡牌可能退回来
                            m_card.m_sceneDZData.m_gameOpState.enterAttackOp(EnGameOp.eOpZhanHouAttack, m_card);
                        }
                        else        // 如果是普通移动牌，就发送移动消息
                        {
                            stCardMoveAndAttackMagicUserCmd cmd = new stCardMoveAndAttackMagicUserCmd();
                            cmd.dst = new stObjectLocation();
                            if (m_card.sceneCardItem.m_cardTableItem != null)
                            {
                                cmd.dst.dwTableID = 0;
                                cmd.dst.x = 0;
                                cmd.dwAttThisID = m_card.sceneCardItem.svrCard.qwThisID;

                                if (m_card.sceneCardItem.m_cardTableItem.m_type == (int)CardType.CARDTYPE_EQUIP)
                                {
                                    cmd.dst.dwLocation = (int)CardArea.CARDCELLTYPE_EQUIP;
                                    cmd.dst.y = 0;
                                }
                                else
                                {
                                    cmd.dst.dwLocation = (int)CardArea.CARDCELLTYPE_COMMON;
                                    cmd.dst.y = (byte)m_card.m_sceneDZData.curWhiteIdx;
                                }
                                UtilMsg.sendMsg(cmd);
                            }

                            m_card.m_sceneDZData.m_gameOpState.enterMoveOp(m_card);      // 进入移动操作
                        }
                    }
                }
            }
        }

        // 如果在范围内，就不发生交互
        protected bool bInSafeTyArea()
        {
            if (UtilMath.xzDis(m_centerPos, m_card.transform().localPosition) <= m_radius)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 回退卡牌到原始位置
        public void backCard2Orig()
        {
            UICamera.simuStopDrag();
            m_card.m_sceneDZData.m_curDragItem = null;
            UIDragObject drag = m_card.gameObject().GetComponent<UIDragObject>();
            drag.reset();
            m_card.trackAniControl.moveBackToPre();      // 退回去
        }
    }
}