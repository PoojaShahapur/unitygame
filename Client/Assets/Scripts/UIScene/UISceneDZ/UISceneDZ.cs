using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 对战界面
     */
    public class UISceneDZ : SceneForm
    {
        public SceneDZData m_sceneDZData;

        public override void onReady()
        {
            base.onReady();

            m_sceneDZData = new SceneDZData();
            findWidget();
            addEventHandle();

            m_sceneDZData.init();
        }

        public override void onShow()
        {
            base.onShow();
        }

        public override void onExit()
        {
            base.onExit();
        }

        // 获取控件
        public void findWidget()
        {
            m_sceneDZData.findWidget();
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.LuckyCoin), onLuckyCoinBtnClk);       // 点击幸运币
            UtilApi.addEventHandle(m_sceneDZData.m_startGO, onStartBtnClk);       // 开始游戏

            UtilApi.addHoverHandle(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.MyCardDeap), onSelfStartHover);
            UtilApi.addHoverHandle(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyCardDeap), onEnemyStartHover);

            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.CollideBG), onClkBg);   // 监听点击背景事件
        }

        // 幸运币点击
        protected void onLuckyCoinBtnClk(GameObject go)
        {
            // 只有是自己出牌的时候才能结束
            if (Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
            {
                
            }
        }

        protected void onStartBtnClk(GameObject go)
        {
            m_sceneDZData.stopTimer();        // 停止定时器
            // 点击后直接隐藏按钮
            m_sceneDZData.m_startGO.SetActive(false);

            // 卸载可能加载的叉号资源
            string resPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "ChaHao.prefab");
            Ctx.m_instance.m_modelMgr.unload(resPath, null);

            stReqFightPrepareOverUserCmd cmd = new stReqFightPrepareOverUserCmd();

            int idx = 0;
            // 设置需要交换的卡牌
            foreach (uint cardid in Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList)
            {
                if (m_sceneDZData.m_changeCardList.IndexOf(cardid) != -1)
                {
                    cmd.change |= (byte)(1 << idx);
                }

                ++idx;
            }

            UtilMsg.sendMsg(cmd);
        }

        protected void onSelfStartHover(GameObject go, bool state)
        {
            if(state)
            {
                UISceneTips tips = Ctx.m_instance.m_uiSceneMgr.loadAndShowForm<UISceneTips>(UISceneFormID.eUISceneTips) as UISceneTips;
                tips.showTips(Ctx.m_instance.m_coordConv.getCurTouchScenePos(), string.Format("当前剩余{0}", Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_leftCardNum));
            }
            else
            {
                Ctx.m_instance.m_uiSceneMgr.hideSceneForm(UISceneFormID.eUISceneTips);
            }
        }

        protected void onEnemyStartHover(GameObject go, bool state)
        {
            if (state)
            {
                UISceneTips tips = Ctx.m_instance.m_uiSceneMgr.loadAndShowForm<UISceneTips>(UISceneFormID.eUISceneTips) as UISceneTips;
                tips.showTips(Ctx.m_instance.m_coordConv.getCurTouchScenePos(), string.Format("当前剩余{0}", Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerEnemy].m_leftCardNum));
            }
            else
            {
                Ctx.m_instance.m_uiSceneMgr.hideSceneForm(UISceneFormID.eUISceneTips);
            }
        }

        public void psstRetLeftCardLibNumUserCmd(stRetLeftCardLibNumUserCmd msg)
        {

        }

        public void psstRetMagicPointInfoUserCmd(stRetMagicPointInfoUserCmd msg)
        {
            // 更新 MP 显示
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].updateMp();
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].updateMp();

            if (Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
            {
                // 显示那张牌可以出
                // 如果出牌区域已经有 7 张牌，就不能再出了
                if (m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].bOutAreaCardFull())
                {
                    m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].updateInCardOutState(false);
                }
                else
                {
                    m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].updateInCardOutState(true);
                }
            }
        }

        // 刷新状态
        public void psstRetRefreshBattleStateUserCmd(stRetRefreshBattleStateUserCmd msg)
        {
            // ChallengeState.CHALLENGE_STATE_BATTLE 状态或者是刚开始，或者是中间掉线，然后重新上线
            if(Ctx.m_instance.m_dataPlayer.m_dzData.m_state == (int)ChallengeState.CHALLENGE_STATE_BATTLE)
            {
                m_sceneDZData.m_bStartRound = true;
                // 停止各种倒计时
                m_sceneDZData.stopTimer();
                if (m_sceneDZData.m_DJSTimer != null)
                {
                    m_sceneDZData.m_DJSTimer.stopTimer();
                }

                if (Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
                {
                    // 开始定时器
                    if (m_sceneDZData.m_timer == null)        // 如果定时器没有
                    {
                        m_sceneDZData.startDZTimer();
                    }
                    else
                    {
                        m_sceneDZData.changeTimer();
                    }
                }

                if (Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].bHaveStartCard())    // 如果自己有初始化的牌
                {
                    m_sceneDZData.m_startGO.SetActive(false);
                    m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].inSceneCardList.startCardMoveTo();      // 一定初始化卡牌到卡牌列表
                }

                m_sceneDZData.m_gameRunState.enterState(GameRunState.STARTDZ);      // 进入对战状态
            }
        }

        public void psstRetRefreshBattlePrivilegeUserCmd(stRetRefreshBattlePrivilegeUserCmd msg)
        {
            // 停止倒计时定时器
            m_sceneDZData.stopTimer();
            if (m_sceneDZData.m_DJSTimer != null)
            {
                m_sceneDZData.m_DJSTimer.stopTimer();
            }

            // 显示各种提示和动画
            if(Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
            {
                if (m_sceneDZData.m_bStartRound)          // 只有当回合开始后，如果到自己出牌，才开启倒计时，这个消息已进入对战就发送过来了
                {
                    m_sceneDZData.m_selfTurnTip.turnBegin();
                    m_sceneDZData.m_roundBtn.myTurn();
                    //m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].updateInCardGreenFrame(true);

                    // 开始定时器
                    if (m_sceneDZData.m_timer == null)        // 如果定时器没有
                    {
                        m_sceneDZData.startDZTimer();
                    }
                    else
                    {
                        m_sceneDZData.changeTimer();
                    }
                }
            }
            else 
            {
                m_sceneDZData.m_roundBtn.enemyTurn();
                m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].updateInCardOutState(false);
                m_sceneDZData.m_gameOpState.quitAttackOp();
            }
        }

        public void psstAddBattleCardPropertyUserCmd(stAddBattleCardPropertyUserCmd msg, SceneCardItem sceneItem)
        {
            m_sceneDZData.m_sceneDZAreaArr[msg.who - 1].psstAddBattleCardPropertyUserCmd(msg, sceneItem);
        }

        public void psstNotifyBattleCardPropertyUserCmd(stNotifyBattleCardPropertyUserCmd msg)
        {
            // 更新动画
            EnDZPlayer attSide = EnDZPlayer.ePlayerTotal;
            EnDZPlayer defSide = EnDZPlayer.ePlayerTotal;

            CardArea attSlot = CardArea.CARDCELLTYPE_NONE;
            CardArea defSlot = CardArea.CARDCELLTYPE_NONE;

            SceneCardBase att = m_sceneDZData.getSceneCardByThisID(msg.pAttThisID, ref attSide, ref attSlot);
            SceneCardBase def = m_sceneDZData.getSceneCardByThisID(msg.pDefThisID, ref defSide, ref defSlot);
            int num = 0;

            if (attSide == EnDZPlayer.ePlayerTotal ||
                defSide == EnDZPlayer.ePlayerTotal ||
                attSlot == CardArea.CARDCELLTYPE_NONE ||
                defSlot == CardArea.CARDCELLTYPE_NONE)
            {
                Ctx.m_instance.m_logSys.log("攻击失败");
            }

            if (att != null && def != null)
            {
                if ((int)EnAttackType.ATTACK_TYPE_NORMAL == msg.attackType || (int)EnAttackType.ATTACK_TYPE_S_MAGIC == msg.attackType)  // 只有单攻才会有移动的动画
                {
                    if (msg.pDefThisID == att.sceneCardItem.svrCard.qwThisID)        // 只有发送给被击者的信息的时候，做一次动画，发送给攻击者的时候就不用了
                    {
                        att.playAttackAni(def.transform.localPosition);     // 播放动画
                    }
                }

                // 播放 Fly 数字,，攻击者和被击者都有可能伤血，播放掉血数字
                // 攻击者掉血
                num = (int)def.sceneCardItem.svrCard.damage;
                if (num > 0)        // 攻击力可能为 0 
                {
                    att.playFlyNum(num);
                }
                num = (int)att.sceneCardItem.svrCard.damage;
                if (num > 0)
                {
                    def.playFlyNum(num);
                }
            }

            // 继续更新属性
            stAddBattleCardPropertyUserCmd stUpdate = new stAddBattleCardPropertyUserCmd();

            stUpdate.slot = (byte)attSlot;
            stUpdate.who = (byte)((int)attSide + 1);
            stUpdate.byActionType = 2;
            stUpdate.mobject = msg.A_object;
            m_sceneDZData.m_sceneDZAreaArr[(int)attSide].psstAddBattleCardPropertyUserCmd(stUpdate, att.sceneCardItem);

            stUpdate.slot = (byte)defSlot;
            stUpdate.who = (byte)((int)defSide + 1);
            stUpdate.byActionType = 2;
            stUpdate.mobject = msg.D_object;
            m_sceneDZData.m_sceneDZAreaArr[(int)defSide].psstAddBattleCardPropertyUserCmd(stUpdate, def.sceneCardItem);
        }

        public void psstNotifyFightEnemyInfoUserCmd(stNotifyFightEnemyInfoUserCmd msg)
        {
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].centerHero.setClasss((EnPlayerCareer)Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroOccupation);   // 设置职业
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].centerHero.setClasss((EnPlayerCareer)Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerEnemy].m_heroOccupation);   // 设置职业
        }

        // 自己第一次获得的卡牌的处理，如果换牌，还是会再次发送这个消息
        public void psstRetFirstHandCardUserCmd(stRetFirstHandCardUserCmd cmd)
        {
            if (m_sceneDZData.bAddselfCard)     // 如果是换牌后发送过来的数据
            {
                // 直接替换掉卡牌就行了
                m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].inSceneCardList.replaceInitCard();
            }
            else        // 第一次发送过来的卡牌数据
            {
                m_sceneDZData.bAddselfCard = true;
                if (m_sceneDZData.bHeroAniEnd)       // 如果 hero 动画已经结束
                {
                    m_sceneDZData.addSelfFirstCard();
                }
            }
        }

        public void psstRetMoveGameCardUserCmd(stRetMoveGameCardUserCmd msg)
        {
            if (msg.side != 1 && msg.side != 2 && msg.success == 1)
            {
                Ctx.m_instance.m_logSys.log(msg.side.ToString());
            }

            // 只有有效值的时候才处理
            if (msg.side == 1 || msg.side == 2)
            {
                m_sceneDZData.m_sceneDZAreaArr[msg.side - 1].outSceneCardList.removeWhiteCard();       // 将占位的牌移除

                if (msg.success == 1)     // 如果成功，就放进出牌位置
                {
                    m_sceneDZData.m_sceneDZAreaArr[msg.side - 1].changeSceneCard(msg);

                    if ((msg.side - 1) == (int)EnDZPlayer.ePlayerSelf)
                    {
                        m_sceneDZData.m_curDragItem = null;
                    }
                }
                else                    // 退回到原来的位置
                {
                    m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].moveDragBack();

                    m_sceneDZData.m_curDragItem = null;
                }
            }
        }

        public void psstAddEnemyHandCardPropertyUserCmd()
        {
            ++Ctx.m_instance.m_dataPlayer.m_dzData.m_enemyCardCount;
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].psstAddEnemyHandCardPropertyUserCmd();
        }

        public void psstRetEnemyHandCardNumUserCmd(stRetEnemyHandCardNumUserCmd msg)
        {
            Ctx.m_instance.m_dataPlayer.m_dzData.m_enemyCardCount = msg.count;
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].inSceneCardList.addInitCard();
        }

        public void psstDelEnemyHandCardPropertyUserCmd(stDelEnemyHandCardPropertyUserCmd msg)
        {
            (m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].inSceneCardList as EnemyInSceneCardList).removeEmptyCard();
        }

        // side 删除的某一方的一个卡牌
        public void psstRetRemoveBattleCardUserCmd(stRetRemoveBattleCardUserCmd cmd, int side, SceneCardItem sceneItem)
        {
            m_sceneDZData.m_sceneDZAreaArr[side].delOneCard(sceneItem);
        }

        public void psstRetNotifyHandIsFullUserCmd(stRetNotifyHandIsFullUserCmd msg)
        {
            m_sceneDZData.m_selfCardFullTip.getGameObject().SetActive(true);

            if (1 == msg.who)            // 如果是自己
            {
                m_sceneDZData.m_selfCardFullTip.m_desc.text = "自己的卡牌已经满了";
            }
            else            // 对方
            {
                m_sceneDZData.m_selfCardFullTip.m_desc.text = "对方的卡牌已经满了";
            }

            // 启动定时器
            TimerItemBase timer = new TimerItemBase();
            timer.m_internal = 1;
            timer.m_timerDisp = endSelfFullTip;
            Ctx.m_instance.m_timerMgr.addObject(timer);
        }

        public void endSelfFullTip(TimerItemBase timer)
        {
            m_sceneDZData.m_selfCardFullTip.getGameObject().SetActive(false);
        }

        public void psstRetCardAttackFailUserCmd(stRetCardAttackFailUserCmd cmd)
        {
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].psstRetCardAttackFailUserCmd(cmd);
        }

        public void psstRetBattleHistoryInfoUserCmd(stRetBattleHistoryInfoUserCmd cmd)
        {
            m_sceneDZData.m_historyArea.psstRetBattleHistoryInfoUserCmd(cmd);
        }

        // 点击背景处理
        public void onClkBg(GameObject go)
        {
            if (Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
            {
                m_sceneDZData.m_gameOpState.quitAttackOp();
            }
        }
    }
}