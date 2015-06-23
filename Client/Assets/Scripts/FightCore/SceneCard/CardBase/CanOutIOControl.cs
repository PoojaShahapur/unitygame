using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 可以出到出牌区的移动控制器，基本就是自己的手牌，因为 Enemy 敌人的手牌是使用 EnemyCard，虽然 Enemy 的手牌也是可以出到出牌区域的，但是不走这个流程，是直接新建的一张卡牌
     */
    public class CanOutIOControl : ExceptBlackIOControl
    {
        protected Action m_moveDisp;           // 拖动中分发
        protected Vector3 m_centerPos;         // 中心点位置
        protected float m_radius = 1;          // 半径
        protected bool m_isCalc;               // 本次是否计算过超过圆形区域的处理

        public CanOutIOControl(SceneCardBase rhv) : 
            base(rhv)
        {
            
        }

        override public void init()
        {
            base.init();

            this.m_card.dragOverEntityDisp.addEventHandle(onDragOver);
            this.m_card.dragOutEntityDisp.addEventHandle(onDragOut);

            if (!m_card.m_sceneDZData.m_gameRunState.isInState(GameRunState.INITCARD))     // 初始化卡牌阶段是不能拖动的
            {
                enableDrag();
            }
        }

        override public void setMoveDisp(Action rhv)
        {
            m_moveDisp = rhv;
        }

        override public void setCenterPos(Vector3 rhv)
        {
            m_centerPos = rhv;
        }

        // 能拖动的必然所有的操作都完成后才能操作
        override protected void onStartDrag()
        {
            // 保存当前操作的卡牌
            m_card.m_sceneDZData.m_curDragItem = m_card;     // 设置当前拖放的目标
            enableDragTitle();      // Drag Title 动画
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

        // 开启拖动
        override public void enableDrag()
        {
            if (m_card.gameObject().GetComponent<UIDragObject>() == null)
            {
                UIDragObject drag = m_card.gameObject().AddComponent<UIDragObject>();
                drag.target = m_card.gameObject().transform;
                drag.m_startDragDisp = onStartDrag;
                drag.m_moveDisp = onMove;
                drag.m_dropEndDisp = onDragEnd;
                drag.m_canMoveDisp = canMove;
                drag.m_planePt = new Vector3(0, SceneDZCV.DRAG_YDELTA, 0);
            }
            if (m_card.gameObject().GetComponent<WindowDragTilt>() == null)
            {
                m_card.gameObject().AddComponent<WindowDragTilt>();
            }
        }

        // 关闭拖放功能
        override public void disableDrag()
        {
            UIDragObject drag = m_card.gameObject().GetComponent<UIDragObject>();
            //drag.enabled = false;
            UtilApi.Destroy(drag);
            WindowDragTilt dragTitle = m_card.gameObject().GetComponent<WindowDragTilt>();
            //dragTitle.enabled = false;
            UtilApi.Destroy(dragTitle);
        }

        override public void enableDragTitle()
        {
            WindowDragTilt dragTitle = m_card.gameObject().GetComponent<WindowDragTilt>();
            if (dragTitle != null)
            {
                dragTitle.enabled = true;
            }
        }

        override public void disableDragTitle()
        {
            WindowDragTilt dragTitle = m_card.gameObject().GetComponent<WindowDragTilt>();
            if (dragTitle != null)  // 手牌是有这个组件的，如果是出到场牌，这个组件就没有了，例如拖动出去后，这个组件就没有了
            {
                dragTitle.enabled = false;
            }
        }

        // 指明当前是否可以改变位置
        override protected bool canMove()
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

        override protected void onMove()
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
            if (Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
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
        override protected void onDragEnd()
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
                m_card.trackAniControl.endDragAni();       // 结束动画

#if DEBUG_NOTNET
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
        override public void backCard2Orig()
        {
            UICamera.simuStopDrag();
            m_card.m_sceneDZData.m_curDragItem = null;
            UIDragObject drag = m_card.gameObject().GetComponent<UIDragObject>();
            drag.reset();
            m_card.trackAniControl.moveBackToPre();      // 退回去
        }

        // 输入释放
        override public void onCardUp(IDispatchObject dispObj)
        {
            base.onCardUp(dispObj);
            m_card.trackAniControl.normalState();
        }

        // 输入按下移动到卡牌上
        override public void onDragOver(IDispatchObject dispObj)
        {
            m_card.trackAniControl.expandState();
        }

        // 输入按下移动到卡牌出
        override public void onDragOut(IDispatchObject dispObj)
        {
            m_card.trackAniControl.normalState();
        }

        // 除了 Enemy 手牌外，所有的卡牌都可以点击，包括主角、装备、技能、手里卡牌、出的卡牌
        override public void onCardClick(IDispatchObject dispObj)
        {
            if (m_card.m_sceneDZData.m_gameRunState.isInState(GameRunState.INITCARD))      // 如果处于初始化卡牌阶段
            {
                string resPath = "";
                // 这个时候还没有服务器的数据 m_sceneCardItem
                int idx = 0;
                idx = m_card.m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].inSceneCardList.findCardIdx(m_card);
                // 显示换牌标志
                if (m_card.m_sceneDZData.m_changeCardList.IndexOf(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[idx]) != -1)      // 如果已经选中
                {
                    m_card.m_sceneDZData.m_changeCardList.Remove(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[idx]);
                    // 去掉叉号
                    UtilApi.Destroy(m_card.chaHaoGo);        // 释放资源
                }
                else  // 选中
                {
                    m_card.m_sceneDZData.m_changeCardList.Add(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[idx]);
                    // 添加叉号
                    resPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "ChaHao.prefab");
                    ModelRes model = Ctx.m_instance.m_modelMgr.getAndSyncLoad<ModelRes>(resPath) as ModelRes;
                    m_card.chaHaoGo = model.InstantiateObject(resPath) as GameObject;
                    UtilApi.SetParent(m_card.chaHaoGo.transform, m_card.transform(), false);
                }
            }
            else        // 如果在对战阶段
            {
                if (EnDZPlayer.ePlayerSelf != m_card.sceneCardItem.m_playerSide || CardArea.CARDCELLTYPE_HAND != m_card.sceneCardItem.cardArea)         // 如果点击的不是自己的手牌
                {
                    if (m_card.sceneCardItem != null)
                    {
                        if (m_card.m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpNormalAttack))
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
                                m_card.enterAttack();
                            }
                        }
                        else if ((m_card.m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpFaShu)))        // 法术攻击
                        {
                            if (m_card.m_sceneDZData.m_gameOpState.canAttackOp(m_card, EnGameOp.eOpFaShu))
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
                                m_card.enterAttack();
                            }
                        }
                        else if (m_card.m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpZhanHouAttack))      // 战吼攻击
                        {
                            if (m_card.m_sceneDZData.m_gameOpState.canAttackOp(m_card, EnGameOp.eOpZhanHouAttack))
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
                                m_card.enterAttack();
                            }
                        }
                        else if (m_card.m_sceneDZData.m_gameOpState.bInOp(EnGameOp.eOpSkillAttackTarget))      // 技能目标攻击
                        {
                            if (m_card.m_sceneDZData.m_gameOpState.canAttackOp(m_card, EnGameOp.eOpSkillAttackTarget))
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
                            m_card.enterAttack();
                        }
                    }
                }
            }
        }

        // 结束转换模型
        override public void endConvModel(int type)
        {
            base.endConvModel(type);

            if (0 == type)      // 转换到手牌需要能滑动
            {
                this.m_card.dragOverEntityDisp.addEventHandle(onDragOver);
                this.m_card.dragOutEntityDisp.addEventHandle(onDragOut);
            }
            else if (1 == type)       // 转换到场牌需要开启按下和起来事件
            {
                this.m_card.downEntityDisp.addEventHandle(onCardDown);
                this.m_card.upEntityDisp.addEventHandle(onCardUp);
            }
        }
    }
}