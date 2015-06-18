using Fight;
using FightCore;
using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 测试界面
     */
    public class UITest : Form
    {
        public AuxLabel m_logText;

        public override void onInit()
        {
            exitMode = false;         // 直接隐藏
            //hideOnCreate = true;
            base.onInit();
        }

        override public void onShow()
        {
            base.onShow();
        }
        
        // 初始化控件
        override public void onReady()
        {
            base.onReady();
            findWidget();
            addEventHandle();
        }

        // 每一次隐藏都会调用一次
        override public void onHide()
		{
            base.onHide();
		}

        // 每一次关闭都会调用一次
        override public void onExit()
        {
            base.onExit();
        }

        protected void findWidget()
        {
            m_logText = new AuxLabel(m_GUIWin.m_uiRoot, "LogText");
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, "BtnTest", onBtnClkTest);
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, "BtnTest1f", onBtnClkTest1f);
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, "BtnTest2f", onBtnClkTest2f);
        }

        protected void onBtnClkTest()
        {
            //testSceneCard();
            //addOneCard();

            //testEnemyCard();
            //testMp();

            //testDelEnemyHandleCard();
            //addHistoryItem();
            //testQuipDZScene();
            //testPrepareTime();
            //testMsg();
            //testAddCard();
            //testCommonFight();
            //testSkillFight();
            testMoveEffect();
            //testChangeModel();
            //testAni();
            //testLinkEffect();
        }

        protected void onBtnClkTest1f()
        {
            //testStart();
            //testAttackAni();
            //testFlyNum();
            //testSendToSelf();
            //changeModeOut();
            downAni();
        }

        protected void onBtnClkTest2f()
        {
            //UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            //uiDZ.m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].putHandFromOut();
            changeModeHandle();
        }

        public void logOut(string str)
        {
            m_logText.text += str;
            m_logText.text += "\n";
        }

        protected void testDZCam()
        {
            Ctx.m_instance.m_camSys.m_dzCam.draw();
        }

        protected void testGetWidget()
        {
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            if (uiDZ != null)
            {
                uiDZ.findWidget();
            }
        }

        protected void testSceneCard()
        {
            Ctx.m_instance.m_dataPlayer.m_dzData.m_priv = 1;
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);

            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList = new uint[1];
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[0] = 10001;
            //Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[1] = 10002;
            //Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[2] = 10003;
            //Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[3] = 10004;
            //Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[4] = 10004;
            //Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[5] = 10004;
            //Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[6] = 10004;
            //Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[7] = 10004;
            //Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[8] = 10004;
            //Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_startCardList[9] = 10004;

            uiDZ.m_sceneDZData.bHeroAniEnd = true;
            uiDZ.psstRetFirstHandCardUserCmd(null);
        }

        protected void addOneCard()
        {
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].sceneCardList = new List<SceneCardItem>();
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].sceneCardList.Add(new SceneCardItem());
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].sceneCardList.Add(new SceneCardItem());
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].sceneCardList.Add(new SceneCardItem());
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].sceneCardList.Add(new SceneCardItem());

            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            uiDZ.psstAddBattleCardPropertyUserCmd(null, null);
        }

        protected void testStart()
        {
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            uiDZ.m_sceneDZData.m_startGO.SetActive(false);
            uiDZ.m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].inSceneCardList.startCardMoveTo();
        }

        protected void testEnemyCard()
        {
            Ctx.m_instance.m_dataPlayer.m_dzData.m_enemyCardCount = 5;
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            uiDZ.m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].inSceneCardList.addInitCard();
        }

        protected void testMp()
        {
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroMagicPoint = new t_MagicPoint();
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroMagicPoint.mp = 4;
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroMagicPoint.maxmp = 9;

            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            uiDZ.m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].updateMp();
        }

        protected void testAttackAni()
        {
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            //uiDZ.m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].outSceneCardList.getCardByIdx(0).behaviorControl.playAttackAni(uiDZ.m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].outSceneCardList.getCardByIdx(0).transform.localPosition, null);
        }

        protected void testFlyNum()
        {
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            uiDZ.m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].outSceneCardList.getCardByIdx(0).playFlyNum(123);
        }

        protected void testSendToSelf()
        {
            stRetCardAttackFailUserCmd cmd = new stRetCardAttackFailUserCmd();
            UtilMsg.sendMsg(cmd, false);
        }

        // 测试删除对方手里一张卡牌
        protected void testDelEnemyHandleCard()
        {
            stDelEnemyHandCardPropertyUserCmd cmd = new stDelEnemyHandCardPropertyUserCmd();
            UtilMsg.sendMsg(cmd, false);
        }

        protected void addHistoryItem()
        {
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            stRetBattleHistoryInfoUserCmd cmd = new stRetBattleHistoryInfoUserCmd();
            cmd.maincard = new t_Card();
            cmd.maincard.dwObjectID = 10001;
            uiDZ.m_sceneDZData.m_historyArea.psstRetBattleHistoryInfoUserCmd(cmd);
        }

        protected void testQuipDZScene()
        {
            //stRetBattleGameResultUserCmd cmd = new stRetBattleGameResultUserCmd();
            //UtilMsg.sendMsg(cmd, false);
            Ctx.m_instance.m_gameSys.loadGameScene();        // 加载游戏场景
        }

        protected void testPrepareTime()
        {
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            uiDZ.m_sceneDZData.startInitCardTimer();
        }

        protected void testMsg()
        {
            stAddBattleCardPropertyUserCmd cmd = new stAddBattleCardPropertyUserCmd();
            //cmd.attackType = 2;
            //cmd.pAttThisID = 0;
            //cmd.pDefThisID = 0;
            UtilMsg.sendMsg(cmd);
        }

        protected void testAddCard()
        {
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            SceneCardBase testCard = null;
            // 测试[随从卡]
            testCard = Ctx.m_instance.m_sceneCardMgr.createCardById(230000, EnDZPlayer.ePlayerSelf, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND, uiDZ.m_sceneDZData);
            // 测试[武器卡]
            //testCard = Ctx.m_instance.m_sceneCardMgr.createCardById(240000, EnDZPlayer.ePlayerSelf, CardArea.CARDCELLTYPE_EQUIP, CardType.CARDTYPE_EQUIP, uiDZ.m_sceneDZData);
            // 测试[英雄卡]
            //testCard = Ctx.m_instance.m_sceneCardMgr.createCardById(250000, EnDZPlayer.ePlayerSelf, CardArea.CARDCELLTYPE_HERO, CardType.CARDTYPE_HERO, uiDZ.m_sceneDZData);
            // 测试[英雄技能卡]
            //testCard = Ctx.m_instance.m_sceneCardMgr.createCardById(260000, EnDZPlayer.ePlayerSelf, CardArea.CARDCELLTYPE_SKILL, CardType.CARDTYPE_SKILL, uiDZ.m_sceneDZData);
            testCard.moveControl.moveToDest(new Vector3(-4, 0, 0), new Vector3(4, 0, 0), 0.3f, testCard.behaviorControl.onMove2DestEnd);
            testCard.moveControl.moveToDest(new Vector3(-4, 0, 0), new Vector3(4, 0, 0), 0.3f, testCard.behaviorControl.onMove2DestEnd);
            //testCard.updateCardOutState(true);
            //testCard.effectControl.linkEffect.play();
        }

        protected void testCommonFight()
        {
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            SceneCardBase selfCard = null;
            SceneCardBase enemyCard = null;
            // 测试[随从卡]
            selfCard = Ctx.m_instance.m_sceneCardMgr.createCardById(230000, EnDZPlayer.ePlayerSelf, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND, uiDZ.m_sceneDZData);
            UtilApi.setPos(selfCard.transform(), new UnityEngine.Vector3(-4, 0, 0));
            SceneCardItem sceneCardItem = null;
            sceneCardItem = new SceneCardItem();
            sceneCardItem.svrCard = new t_Card();
            sceneCardItem.svrCard.qwThisID = 0;
            sceneCardItem.svrCard.dwObjectID = 230000;
            sceneCardItem.m_cardTableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, sceneCardItem.svrCard.dwObjectID).m_itemBody as TableCardItemBody;
            selfCard.sceneCardItem = sceneCardItem;

            enemyCard = Ctx.m_instance.m_sceneCardMgr.createCardById(230000, EnDZPlayer.ePlayerEnemy, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND, uiDZ.m_sceneDZData);
            UtilApi.setPos(enemyCard.transform(), new UnityEngine.Vector3(4, 0, 0));
            sceneCardItem = new SceneCardItem();
            sceneCardItem.svrCard = new t_Card();
            sceneCardItem.svrCard.qwThisID = 1;
            sceneCardItem.svrCard.dwObjectID = 230000;
            sceneCardItem.m_cardTableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, sceneCardItem.svrCard.dwObjectID).m_itemBody as TableCardItemBody;
            enemyCard.sceneCardItem = sceneCardItem;

            AttackItemBase attItem = selfCard.fightData.attackData.createItem(EAttackType.eCommon);
            (attItem as ComAttackItem).hurterId = 1;
            (attItem as ComAttackItem).attackEffectId = 4;
            (attItem as ComAttackItem).moveTime = 2;
            attItem.damage = 10;

            // 受伤
            HurtItemBase hurtItem = enemyCard.fightData.hurtData.createItem(EHurtType.eCommon);
            (hurtItem as ComHurtItem).hurtEffectId = 4;
            (hurtItem as ComHurtItem).delayTime = (attItem as ComAttackItem).getMoveTime();
            hurtItem.damage = 20;
        }

        protected void testSkillFight()
        {
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            SceneCardBase selfCard = null;
            SceneCardBase enemyCard = null;
            // 测试[随从卡]
            selfCard = Ctx.m_instance.m_sceneCardMgr.createCardById(230000, EnDZPlayer.ePlayerSelf, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND, uiDZ.m_sceneDZData);
            UtilApi.setPos(selfCard.transform(), new UnityEngine.Vector3(-4, 0, 0));
            SceneCardItem sceneCardItem = null;
            sceneCardItem = new SceneCardItem();
            sceneCardItem.svrCard = new t_Card();
            sceneCardItem.svrCard.qwThisID = 0;
            sceneCardItem.svrCard.dwObjectID = 230000;
            sceneCardItem.m_cardTableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, sceneCardItem.svrCard.dwObjectID).m_itemBody as TableCardItemBody;
            selfCard.sceneCardItem = sceneCardItem;

            enemyCard = Ctx.m_instance.m_sceneCardMgr.createCardById(230000, EnDZPlayer.ePlayerEnemy, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND, uiDZ.m_sceneDZData);
            UtilApi.setPos(enemyCard.transform(), new UnityEngine.Vector3(4, 0, 0));
            sceneCardItem = new SceneCardItem();
            sceneCardItem.svrCard = new t_Card();
            sceneCardItem.svrCard.qwThisID = 1;
            sceneCardItem.svrCard.dwObjectID = 230000;
            sceneCardItem.m_cardTableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, sceneCardItem.svrCard.dwObjectID).m_itemBody as TableCardItemBody;
            enemyCard.sceneCardItem = sceneCardItem;

            // 技能攻击攻击特效在技能表中配置
            AttackItemBase attItem = selfCard.fightData.attackData.createItem(EAttackType.eSkill);
            (attItem as SkillAttackItem).skillId = 3;
            (attItem as SkillAttackItem).hurtIdList.Add(1);
            attItem.damage = 10;

            // 受伤
            HurtItemBase hurtItem = enemyCard.fightData.hurtData.createItem(EHurtType.eSkill);
            // 技能攻击没有被击特效
            (hurtItem as SkillHurtItem).delayTime = (attItem as SkillAttackItem).skillTableItem.m_effectMoveTime;
            (hurtItem as SkillHurtItem).bDamage = true;
            hurtItem.damage = 20;
        }


        // 创建一个卡牌就会放到 (-4, 0, 0) 位置
        protected SceneCardBase m_linkEffectModel;
        public void testLinkEffect()
        {
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            // 测试[随从卡]
            m_linkEffectModel = Ctx.m_instance.m_sceneCardMgr.createCardById(230000, EnDZPlayer.ePlayerSelf, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND, uiDZ.m_sceneDZData);
            UtilApi.setPos(m_linkEffectModel.transform(), new UnityEngine.Vector3(-4, 0, 0));
            SceneCardItem sceneCardItem = null;
            sceneCardItem = new SceneCardItem();
            sceneCardItem.svrCard = new t_Card();
            sceneCardItem.svrCard.qwThisID = 0;
            sceneCardItem.svrCard.dwObjectID = 230000;
            sceneCardItem.m_cardTableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, sceneCardItem.svrCard.dwObjectID).m_itemBody as TableCardItemBody;
            m_linkEffectModel.sceneCardItem = sceneCardItem;

            m_linkEffectModel.effectControl.addLinkEffect(4, true, true, true);
        }

        public void testMoveEffect()
        {
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);

            MoveEffect effect = Ctx.m_instance.m_sceneEffectMgr.createAndAdd(EffectType.eMoveEffect, EffectRenderType.eSpriteEffectRender) as MoveEffect;

            effect.setPnt(uiDZ.m_sceneDZData.m_centerGO);
            effect.setLoop(false);
            effect.setTableID(4);
            effect.srcPos = new Vector3(-4, 0, 0);
            effect.destPos = new Vector3(4, 0, 0);
            effect.effectMoveTime = 5;
            effect.play();
        }


        protected SceneCardBase m_changeModel;
        protected void testChangeModel()
        {
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            // 测试[随从卡]
            m_changeModel = Ctx.m_instance.m_sceneCardMgr.createCardById(230000, EnDZPlayer.ePlayerSelf, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND, uiDZ.m_sceneDZData);
            UtilApi.setPos(m_changeModel.transform(), new UnityEngine.Vector3(-4, 0, 0));
            SceneCardItem sceneCardItem = null;
            sceneCardItem = new SceneCardItem();
            sceneCardItem.svrCard = new t_Card();
            sceneCardItem.svrCard.qwThisID = 0;
            sceneCardItem.svrCard.dwObjectID = 230000;
            sceneCardItem.m_cardTableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, sceneCardItem.svrCard.dwObjectID).m_itemBody as TableCardItemBody;
            m_changeModel.sceneCardItem = sceneCardItem;
        }

        protected void changeModeOut()
        {
            m_changeModel.convOutModel();
        }

        protected void changeModeHandle()
        {
            m_changeModel.convHandleModel();
        }

        protected SceneCardBase m_aniModel;
        protected void testAni()
        {
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            // 测试[随从卡]
            m_aniModel = Ctx.m_instance.m_sceneCardMgr.createCardById(230000, EnDZPlayer.ePlayerSelf, CardArea.CARDCELLTYPE_HAND, CardType.CARDTYPE_ATTEND, uiDZ.m_sceneDZData);
            m_aniModel.setStartIdx(2);
            UtilApi.setPos(m_aniModel.transform(), new UnityEngine.Vector3(-4, 0, 0));
            SceneCardItem sceneCardItem = null;
            sceneCardItem = new SceneCardItem();
            sceneCardItem.svrCard = new t_Card();
            sceneCardItem.svrCard.qwThisID = 0;
            sceneCardItem.svrCard.dwObjectID = 230000;
            sceneCardItem.m_cardTableItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, sceneCardItem.svrCard.dwObjectID).m_itemBody as TableCardItemBody;
            m_aniModel.sceneCardItem = sceneCardItem;
            m_aniModel.faPai2MinAni();
        }

        protected void downAni()
        {
            m_aniModel.min2HandleAni();
        }
    }
}