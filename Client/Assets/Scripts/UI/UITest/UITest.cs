using Game.Msg;
using SDK.Common;
using System.Collections.Generic;
using UnityEngine.UI;

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
            testMsg();
        }

        protected void onBtnClkTest1f()
        {
            testStart();
            //testAttackAni();
            //testFlyNum();
            //testSendToSelf();
        }

        protected void onBtnClkTest2f()
        {
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            uiDZ.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].putHandFromOut();
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
            uiDZ.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].inSceneCardList.startCardMoveTo();
        }

        protected void testEnemyCard()
        {
            Ctx.m_instance.m_dataPlayer.m_dzData.m_enemyCardCount = 5;
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            uiDZ.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].inSceneCardList.addInitCard();
        }

        protected void testMp()
        {
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroMagicPoint = new t_MagicPoint();
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroMagicPoint.mp = 4;
            Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroMagicPoint.maxmp = 9;

            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            uiDZ.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].updateMp();
        }

        protected void testAttackAni()
        {
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            uiDZ.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].outSceneCardList.getCardByIdx(0).playAttackAni(uiDZ.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].outSceneCardList.getCardByIdx(0).transform.localPosition);
        }

        protected void testFlyNum()
        {
            UISceneDZ uiDZ = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneDZ>(UISceneFormID.eUISceneDZ);
            uiDZ.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].outSceneCardList.getCardByIdx(0).playFlyNum(123);
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
            uiDZ.m_historyArea.psstRetBattleHistoryInfoUserCmd(cmd);
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
            uiDZ.startInitCardTimer();
        }

        protected void testMsg()
        {
            stAddBattleCardPropertyUserCmd cmd = new stAddBattleCardPropertyUserCmd();
            //cmd.attackType = 2;
            //cmd.pAttThisID = 0;
            //cmd.pDefThisID = 0;
            UtilMsg.sendMsg(cmd);
        }
    }
}