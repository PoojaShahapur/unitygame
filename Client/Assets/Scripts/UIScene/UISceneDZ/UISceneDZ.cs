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
        public HistoryArea m_historyArea;
        public TimerItemBase m_timer;   // 回合开始的时候开始回合倒计时，进入对战，每一回合倒计时

        public bool m_bNeedTipsInfo = true;     // 是否需要弹出提示框
        public int m_clkTipsCnt = 0;               // 点击提示框次数

        public override void onReady()
        {
            base.onReady();
            // 加载xml配置文件
            m_sceneDZData.m_DZDaoJiShiXmlLimit = Ctx.m_instance.m_xmlCfgMgr.getXmlCfg<DZDaoJiShiXml>(XmlCfgID.eXmlDZCfg).m_list[0] as DZDaoJiShiXmlLimit;
            startInitCardTimer();           // 启动定时器

            m_sceneDZData.m_gameRunState = new GameRunState(m_sceneDZData);

            //Ctx.m_instance.m_camSys.m_dzcam.setGameObject(UtilApi.GoFindChildByPObjAndName("Main Camera"));
            getWidget();
            addEventHandle();

            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf] = new SelfDZArea(m_sceneDZData, EnDZPlayer.ePlayerSelf);
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy] = new EnemyDZArea(m_sceneDZData, EnDZPlayer.ePlayerEnemy);

            m_sceneDZData.m_sceneDZAreaArr = new SceneDZArea[(int)EnDZPlayer.ePlayerTotal];
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf] = m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf];
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy] = m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy];

            m_historyArea = new HistoryArea(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.HistoryGo));
            m_historyArea.m_sceneDZData = m_sceneDZData;
            m_sceneDZData.m_attackArrow = new AttackArrow(m_sceneDZData);
            m_sceneDZData.m_gameOpState = new GameOpState(m_sceneDZData);

            // 设置 hero 动画结束后的处理
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].centerHero.heroAniEndDisp = heroAniEndDisp;
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
            UtilApi.SetActive(m_sceneDZData.m_startGO, false);      // 默认是隐藏的

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
            m_sceneDZData.m_timerGo = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.TimerGo);

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
                stReqEndMyRoundUserCmd cmd;
                if (!m_bNeedTipsInfo)
                {
                    cmd = new stReqEndMyRoundUserCmd();
                    UtilMsg.sendMsg(cmd);
                }
                else
                {
                    ++m_clkTipsCnt;
                    if (m_clkTipsCnt == 1)
                    {
                        if (!hasLeftMagicPtCanUse())
                        {
                            cmd = new stReqEndMyRoundUserCmd();
                            UtilMsg.sendMsg(cmd);
                        }
                        else    // 你还有可操作的随从
                        {
                            Ctx.m_instance.m_langMgr.getText(LangTypeId.eDZ, (int)LangLogID.eItem0);
                            InfoBoxParam param = Ctx.m_instance.m_poolSys.newObject<InfoBoxParam>();
                            param.m_midDesc = Ctx.m_instance.m_shareData.m_retLangStr;
                            param.m_btnClkDisp = onInfoBoxBtnClk;
                            Ctx.m_instance.m_langMgr.getText(LangTypeId.eDZ, (int)LangLogID.eItem1);
                            param.m_btnOkCap = Ctx.m_instance.m_shareData.m_retLangStr;
                            param.m_formID = UIFormID.UIInfo_1;     // 这里提示使用这个 id
                            UIInfo.showMsg(param);
                        }
                    }
                    else
                    {
                        m_clkTipsCnt = 0;
                        cmd = new stReqEndMyRoundUserCmd();
                        UtilMsg.sendMsg(cmd);
                    }
                }
            }
        }

        public void onInfoBoxBtnClk(InfoBoxBtnType type)
        {
            if(type == InfoBoxBtnType.eBTN_OK)
            {
                m_bNeedTipsInfo = false;
            }
        }

        // 检查是否还有剩余的点数，如果还有，给出提示
        protected bool hasLeftMagicPtCanUse()
        {
            return m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].inSceneCardList.hasLeftMagicPtCanUse();
        }

        protected void onStartBtnClk(GameObject go)
        {
            stopTimer();        // 停止定时器
            // 点击后直接隐藏按钮
            m_sceneDZData.m_startGO.SetActive(false);

            // 卸载可能加载的叉号资源
            string resPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "ChaHao.prefab");
            Ctx.m_instance.m_modelMgr.unload(resPath);

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

            //m_sceneDZData.m_startGO.SetActive(false);
            //m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].m_inSceneCardList.startCardMoveTo();
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
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].updateMp();
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].updateMp();            
        }

        // 刷新状态
        public void psstRetRefreshBattleStateUserCmd(stRetRefreshBattleStateUserCmd msg)
        {
            // ChallengeState.CHALLENGE_STATE_BATTLE 状态或者是刚开始，或者是中间掉线，然后重新上线
            if(Ctx.m_instance.m_dataPlayer.m_dzData.m_state == (int)ChallengeState.CHALLENGE_STATE_BATTLE)
            {
                // 停止各种倒计时
                stopTimer();
                if (m_sceneDZData.m_DJSTimer != null)
                {
                    m_sceneDZData.m_DJSTimer.stopTimer();
                }

                if (Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].bHaveStartCard())    // 如果自己有初始化的牌
                {
                    m_sceneDZData.m_startGO.SetActive(false);
                    m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].inSceneCardList.startCardMoveTo();      // 一定初始化卡牌到卡牌列表
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
                m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].updateInCardGreenFrame(true);
            }
            else 
            {
                m_sceneDZData.m_dzturn.enemyTurn();
                m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].updateInCardGreenFrame(false);
                m_sceneDZData.m_gameOpState.quitAttackOp();
            }

            // 停止倒计时定时器
            if (m_sceneDZData.m_DJSTimer != null)
            {
                m_sceneDZData.m_DJSTimer.stopTimer();
            }
            // 开始定时器
            if (m_timer == null)        // 如果定时器没有
            {
                startDZTimer();
            }
            else
            {
                changeTimer();
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
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].centerHero.setclasss((EnPlayerCareer)Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroOccupation);   // 设置职业
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].centerHero.setclasss((EnPlayerCareer)Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroOccupation);   // 设置职业
        }

        // 自己第一次获得的卡牌的处理，如果换牌，还是会再次发送这个消息
        public void psstRetFirstHandCardUserCmd(stRetFirstHandCardUserCmd cmd)
        {
            if (m_sceneDZData.bAddselfCard)     // 如果是换牌后发送过来的数据
            {
                // 直接替换掉卡牌就行了
                m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].inSceneCardList.replaceInitCard();
            }
            else        // 第一次发送过来的卡牌数据
            {
                m_sceneDZData.bAddselfCard = true;
                if (m_sceneDZData.bHeroAniEnd)       // 如果 hero 动画已经结束
                {
                    addSelfFirstCard();
                }
            }
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
                m_sceneDZAreaArr[msg.side - 1].outSceneCardList.removeWhiteCard();       // 将占位的牌移除

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
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].inSceneCardList.addInitCard();
        }

        public void psstDelEnemyHandCardPropertyUserCmd(stDelEnemyHandCardPropertyUserCmd msg)
        {
            (m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].inSceneCardList as EnemyInSceneCardList).removeEmptyCard();
        }

        // side 删除的某一方的一个卡牌
        public void psstRetRemoveBattleCardUserCmd(stRetRemoveBattleCardUserCmd cmd, int side, SceneCardItem sceneItem)
        {
            m_sceneDZAreaArr[side].delOneCard(sceneItem);
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

        public void psstRetBattleHistoryInfoUserCmd(stRetBattleHistoryInfoUserCmd cmd)
        {
            m_historyArea.psstRetBattleHistoryInfoUserCmd(cmd);
        }

        public void heroAniEndDisp()
        {
            m_sceneDZData.bHeroAniEnd = true;
            UtilApi.SetActive(m_sceneDZData.m_startGO, true);      // 主角动画完成，需要显示开始按钮
            if (m_sceneDZData.bAddselfCard)
            {
                addSelfFirstCard();
            }
        }

        protected void addSelfFirstCard()
        {
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].inSceneCardList.addInitCard();
        }

        // 启动定时器
        public void startInitCardTimer()
        {
            if (m_timer == null)
            {
                m_timer = new TimerItemBase();
                m_timer.m_internal = m_sceneDZData.m_DZDaoJiShiXmlLimit.m_preparetime - m_sceneDZData.m_DZDaoJiShiXmlLimit.m_lastpreparetime;
                m_timer.m_totalCount = m_timer.m_internal;
                m_timer.m_timerDisp = onTimerInitCardHandle;

                Ctx.m_instance.m_timerMgr.addObject(m_timer);
            }
            else
            {
                m_timer.reset();
            }
        }

        // 开始对战定时器
        public void startDZTimer()
        {
            if (m_timer == null)
            {
                m_timer = new TimerItemBase();
                m_timer.m_internal = m_sceneDZData.m_DZDaoJiShiXmlLimit.m_roundTimes - m_sceneDZData.m_DZDaoJiShiXmlLimit.m_lastroundtime;
                m_timer.m_totalCount = m_timer.m_internal;
                m_timer.m_timerDisp = onTimerDZHandle;

                Ctx.m_instance.m_timerMgr.addObject(m_timer);
            }
            else
            {
                m_timer.reset();
            }
        }

        // 改变定时器参数
        protected void changeTimer()
        {
            if (m_timer != null)
            {
                m_timer.reset();
                m_timer.m_internal = m_sceneDZData.m_DZDaoJiShiXmlLimit.m_roundTimes - m_sceneDZData.m_DZDaoJiShiXmlLimit.m_lastroundtime;
                m_timer.m_totalCount = m_timer.m_internal;
                m_timer.m_timerDisp = onTimerDZHandle;

                Ctx.m_instance.m_timerMgr.addObject(m_timer);
            }
        }

        // 重置定时器
        protected void resetTimer()
        {
            m_timer.reset();
        }

        // 停止定时器
        public void stopTimer()
        {
            if (m_timer != null)
            {
                Ctx.m_instance.m_timerMgr.delObject(m_timer);
            }
        }

        public void onTimerInitCardHandle(TimerItemBase timer)
        {
            // 开始显示倒计时数据
            if(m_sceneDZData.m_DJSTimer == null)
            {
                m_sceneDZData.m_DJSTimer = new DJSTimer(m_sceneDZData.m_timerGo);
            }

            m_sceneDZData.m_DJSTimer.startTimer();
        }

        public void onTimerDZHandle(TimerItemBase timer)
        {
            // 开始显示倒计时数据
            if (m_sceneDZData.m_DJSTimer == null)
            {
                m_sceneDZData.m_DJSTimer = new DJSTimer(m_sceneDZData.m_timerGo);
            }

            m_sceneDZData.m_DJSTimer.startTimer();
        }
    }
}