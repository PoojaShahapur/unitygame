using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 对战界面
     */
    public class UISceneDZ : SceneForm
    {
        public SceneDZData m_sceneDZData = new SceneDZData();
        public SceneDZArea[] m_sceneDZAreaArr = new SceneDZArea[(int)EnDZPlayer.ePlayerTotal];
        public HistoryArea m_historyArea = new HistoryArea();

        public override void onReady()
        {
            base.onReady();
            //Ctx.m_instance.m_camSys.m_dzcam.setGameObject(UtilApi.GoFindChildByPObjAndName("Main Camera"));
            getWidget();
            addEventHandle();

            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf] = new SelfDZArea(m_sceneDZData, EnDZPlayer.ePlayerSelf);
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy] = new EnemyDZArea(m_sceneDZData, EnDZPlayer.ePlayerEnemy);

            m_sceneDZData.m_sceneDZAreaArr = new SceneDZArea[(int)EnDZPlayer.ePlayerTotal];
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf] = m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf];
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy] = m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy];

            m_historyArea.m_sceneDZData = m_sceneDZData;
            m_sceneDZData.m_attackArrow = new AttackArrow(m_sceneDZData);
            m_sceneDZData.m_gameOpState = new GameOpState(m_sceneDZData);
        }

        public override void onShow()
        {
            base.onShow();
        }

        // 获取控件
        public void getWidget()
        {
            //GameObject[] goList = UtilApi.FindGameObjectsWithTag("aaaa");
            m_sceneDZData.m_dzturn.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.TurnBtn));
            m_sceneDZData.m_luckycoin.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.LuckyCoin));
            m_sceneDZData.m_selfTurnTip.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfTurnTip));
            m_sceneDZData.m_selfCardFullTip.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfCardFullTip));
            m_sceneDZData.m_selfCardFullTip.m_desc = UtilApi.getComByP<Text>(m_sceneDZData.m_selfCardFullTip.getGameObject(), CVSceneDZPath.SelfCardFullTipText);
            m_sceneDZData.m_selfCardFullTip.getGameObject().SetActive(false);

            m_sceneDZData.m_centerGO = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.CenterGO);
            m_sceneDZData.m_startGO = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.StartGO);

            m_sceneDZData.m_cardCenterGOArr[(int)EnDZPlayer.ePlayerSelf, (int)CardArea.CARDCELLTYPE_NONE] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfStartCardCenterGO);
            m_sceneDZData.m_cardCenterGOArr[(int)EnDZPlayer.ePlayerEnemy, (int)CardArea.CARDCELLTYPE_NONE] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyStartCardCenterGO);

            m_sceneDZData.m_cardCenterGOArr[(int)EnDZPlayer.ePlayerSelf, (int)CardArea.CARDCELLTYPE_COMMON] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfOutCardCenterGO);
            m_sceneDZData.m_cardCenterGOArr[(int)EnDZPlayer.ePlayerEnemy, (int)CardArea.CARDCELLTYPE_COMMON] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyOutCardCenterGO);

            m_sceneDZData.m_cardCenterGOArr[(int)EnDZPlayer.ePlayerSelf, (int)CardArea.CARDCELLTYPE_HAND] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfCardCenterGO);
            m_sceneDZData.m_cardCenterGOArr[(int)EnDZPlayer.ePlayerEnemy, (int)CardArea.CARDCELLTYPE_HAND] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyCardCenterGO);

            m_sceneDZData.m_cardCenterGOArr[(int)EnDZPlayer.ePlayerSelf, (int)CardArea.CARDCELLTYPE_EQUIP] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfEquipGO);
            m_sceneDZData.m_cardCenterGOArr[(int)EnDZPlayer.ePlayerEnemy, (int)CardArea.CARDCELLTYPE_EQUIP] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyEquipGO);

            m_sceneDZData.m_cardCenterGOArr[(int)EnDZPlayer.ePlayerSelf, (int)CardArea.CARDCELLTYPE_SKILL] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfSkillGO);
            m_sceneDZData.m_cardCenterGOArr[(int)EnDZPlayer.ePlayerEnemy, (int)CardArea.CARDCELLTYPE_SKILL] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemySkillGO);

            m_sceneDZData.m_attackArrowGO = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.ArrowStartPosGO);
            m_sceneDZData.m_arrowListGO = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.ArrowListGO);

            m_sceneDZData.m_textArr[(int)EnSceneDZText.eSelfMp] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfMpText).GetComponent<Text>();
            m_sceneDZData.m_textArr[(int)EnSceneDZText.eEnemyMp] = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyMpText).GetComponent<Text>();

            m_sceneDZData.m_mpGridArr[(int)EnDZPlayer.ePlayerSelf] = new UIGrid();
            m_sceneDZData.m_mpGridArr[(int)EnDZPlayer.ePlayerSelf].setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfMpList));
            m_sceneDZData.m_mpGridArr[(int)EnDZPlayer.ePlayerSelf].maxPerLine = 10;
            m_sceneDZData.m_mpGridArr[(int)EnDZPlayer.ePlayerSelf].cellWidth = 0.284f;
            m_sceneDZData.m_mpGridArr[(int)EnDZPlayer.ePlayerSelf].cellHeight = 0.284f;
            m_sceneDZData.m_mpGridArr[(int)EnDZPlayer.ePlayerEnemy] = new UIGrid();
            m_sceneDZData.m_mpGridArr[(int)EnDZPlayer.ePlayerEnemy].setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyMpList));
            m_sceneDZData.m_mpGridArr[(int)EnDZPlayer.ePlayerEnemy].maxPerLine = 10;
            m_sceneDZData.m_mpGridArr[(int)EnDZPlayer.ePlayerEnemy].cellWidth = 0.284f;
            m_sceneDZData.m_mpGridArr[(int)EnDZPlayer.ePlayerEnemy].cellHeight = 0.284f;
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.LuckyCoin), onBtnTurnClk);       // 结束本回合
            UtilApi.addEventHandle(m_sceneDZData.m_startGO, onStartBtnClk);       // 开始游戏

            UtilApi.addHoverHandle(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.MyCardDeap), onSelfStartHover);
            UtilApi.addHoverHandle(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyCardDeap), onEnemyStartHover);
        }

        // 结束回合
        protected void onBtnTurnClk(GameObject go)
        {
            // 只有是自己出牌的时候才能结束
            if (Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
            {
                stReqEndMyRoundUserCmd cmd = new stReqEndMyRoundUserCmd();
                UtilMsg.sendMsg(cmd);
            }
        }

        protected void onStartBtnClk(GameObject go)
        {
            stReqFightPrepareOverUserCmd cmd = new stReqFightPrepareOverUserCmd();
            UtilMsg.sendMsg(cmd);

            //m_sceneDZData.m_startGO.SetActive(false);
            //m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].m_inSceneCardList.startCardMoveTo();
        }

        protected void onSelfStartHover(GameObject go, bool state)
        {
            if(state)
            {
                UISceneTips tips = Ctx.m_instance.m_uiSceneMgr.loadAndShowForm(UISceneFormID.eUISceneTips) as UISceneTips;
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
                UISceneTips tips = Ctx.m_instance.m_uiSceneMgr.loadAndShowForm(UISceneFormID.eUISceneTips) as UISceneTips;
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
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].updateMp();
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].updateMp();            
        }

        // 刷新状态
        public void psstRetRefreshBattleStateUserCmd(stRetRefreshBattleStateUserCmd msg)
        {
            // ChallengeState.CHALLENGE_STATE_BATTLE 状态或者是刚开始，或者是中间掉线，然后重新上线
            if(Ctx.m_instance.m_dataPlayer.m_dzData.m_state == (int)ChallengeState.CHALLENGE_STATE_BATTLE)
            {
                if (Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].bHaveStartCard())    // 如果自己有初始化的牌
                {
                    m_sceneDZData.m_startGO.SetActive(false);
                    m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].m_inSceneCardList.startCardMoveTo();      // 一定初始化卡牌到卡牌列表
                }
            }
        }

        public void psstRetRefreshBattlePrivilegeUserCmd(stRetRefreshBattlePrivilegeUserCmd msg)
        {
            // 显示各种提示和动画
            if(Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
            {
                m_sceneDZData.m_selfTurnTip.turnBegin();
                m_sceneDZData.m_dzturn.myturn();
                m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].updateCardGreenFrame(true);
            }
            else 
            {
                m_sceneDZData.m_dzturn.enemyTurn();
                m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].updateCardGreenFrame(false);
                m_sceneDZData.m_gameOpState.quitAttackOp();
            }
        }

        public void psstAddBattleCardPropertyUserCmd(stAddBattleCardPropertyUserCmd msg, SceneCardItem sceneItem)
        {
            // 判断攻击处理
            if (msg.byActionType == 2)
            {
                attackHandle(msg);
            }

            m_sceneDZAreaArr[msg.who - 1].psstAddBattleCardPropertyUserCmd(msg, sceneItem);
        }

        protected void attackHandle(stAddBattleCardPropertyUserCmd msg)
        {
            SceneCardEntityBase att = m_sceneDZData.getSceneCardByThisID(msg.pAttThisID);
            SceneCardEntityBase def = m_sceneDZData.getSceneCardByThisID(msg.pDefThisID);
            int num = 0;

            if (att != null && def != null)
            {
                if ((int)EnAttackType.ATTACK_TYPE_NORMAL == msg.attackType || (int)EnAttackType.ATTACK_TYPE_S_MAGIC == msg.attackType)  // 只有单攻才会有移动的动画
                {
                    if (msg.pDefThisID == att.sceneCardItem.m_svrCard.qwThisID)        // 只有发送给被击者的信息的时候，做一次动画，发送给攻击者的时候就不用了
                    {
                        att.playAttackAni(def.transform.localPosition);     // 播放动画
                    }
                }

                // 播放 Fly 数字
                if (msg.pAttThisID == att.sceneCardItem.m_svrCard.qwThisID)      // 如果是攻击者的信息
                {
                    num = (int)(att.sceneCardItem.m_svrCard.hp - msg.mobject.hp);
                    if (num > 0)
                    {
                        att.playFlyNum(num);
                    }
                }
                else        // 发送给受伤者的信息
                {
                    num = (int)(def.sceneCardItem.m_svrCard.hp - msg.mobject.hp);
                    if (num > 0)
                    {
                        att.playFlyNum(num);
                    }
                }
            }
        }

        public void psstNotifyFightEnemyInfoUserCmd(stNotifyFightEnemyInfoUserCmd msg)
        {
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].m_hero.setclasss((EnPlayerCareer)Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroOccupation);   // 设置职业
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].m_hero.setclasss((EnPlayerCareer)Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroOccupation);   // 设置职业
        }

        public void psstRetFirstHandCardUserCmd(stRetFirstHandCardUserCmd cmd)
        {
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].m_inSceneCardList.addInitCard();
        }

        public void psstRetMoveGameCardUserCmd(stRetMoveGameCardUserCmd msg)
        {
            if (msg.side != 1 && msg.side != 2 && msg.success == 1)
            {
                Ctx.m_instance.m_log.log(msg.side.ToString());
            }

            // 只有有效值的时候才处理
            if (msg.side == 1 || msg.side == 2)
            {
                m_sceneDZAreaArr[msg.side - 1].m_outSceneCardList.removeWhiteCard();       // 将占位的牌移除

                if (msg.success == 1)     // 如果成功，就放进出牌位置
                {
                    m_sceneDZAreaArr[msg.side - 1].changeSceneCard(msg);

                    if ((msg.side - 1) == (int)EnDZPlayer.ePlayerSelf)
                    {
                        m_sceneDZData.m_curDragItem = null;
                    }
                }
                else                    // 退回到原来的位置
                {
                    m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].moveDragBack();

                    m_sceneDZData.m_curDragItem = null;
                }
            }
        }

        public void psstAddEnemyHandCardPropertyUserCmd()
        {
            ++Ctx.m_instance.m_dataPlayer.m_dzData.m_enemyCardCount;
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].psstAddEnemyHandCardPropertyUserCmd();
        }

        public void psstRetEnemyHandCardNumUserCmd(stRetEnemyHandCardNumUserCmd msg)
        {
            Ctx.m_instance.m_dataPlayer.m_dzData.m_enemyCardCount = msg.count;
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].m_inSceneCardList.addInitCard();
        }

        public void psstDelEnemyHandCardPropertyUserCmd(stDelEnemyHandCardPropertyUserCmd msg)
        {
            (m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].m_inSceneCardList as EnemyInSceneCardList).removeEmptyCard();
        }

        public void psstRetRemoveBattleCardUserCmd(stRetRemoveBattleCardUserCmd cmd)
        {
            // 获取卡牌属于哪一方的
            int side = 0;
            SceneCardItem sceneItem = null;
            Ctx.m_instance.m_dataPlayer.m_dzData.getCardSideAndItemByThisID(cmd.dwThisID, ref side, ref sceneItem);

            if(side != 2)   // 如果查找到
            {
                m_sceneDZAreaArr[side].delOneCard(sceneItem);
            }
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
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].psstRetCardAttackFailUserCmd(cmd);
        }
    }
}