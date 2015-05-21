﻿using Game.UI;
using SDK.Common;
using SDK.Lib;

namespace Game.Game
{
    /**
     * @brief 处理各种场景切换
     */
    public class GotoScene
    {
        protected bool m_isFirstEnterGame = true;           // 是否是第一次进入场景

        public void addSceneHandle()
        {
            Ctx.m_instance.m_gameRunStage.addEnteringDisp(enteringStageHandle);
            Ctx.m_instance.m_gameRunStage.addEnteredDisp(enteredStageHandle);
            Ctx.m_instance.m_gameRunStage.addQuitDisp(quitStageHandle);
        }

        public void loadGameScene()
        {
            if (!Ctx.m_instance.m_gameRunStage.isCurInStage(EGameStage.eStage_Game))
            {
                Ctx.m_instance.m_gameRunStage.toggleGameStage(EGameStage.eStage_Game);
                Ctx.m_instance.m_sceneSys.loadScene("Game.unity", onGameResLoadScene);
            }
        }

        public void loadDZScene(uint sceneNumber)
        {
            // 查找场景配置文件
            MapXmlItem xmlItem = Ctx.m_instance.m_mapCfg.getXmlItem(sceneNumber);

            if (xmlItem != null)
            {
                if (!Ctx.m_instance.m_gameRunStage.isCurInStage(EGameStage.eStage_DZ))
                {
                    Ctx.m_instance.m_gameRunStage.toggleGameStage(EGameStage.eStage_DZ);
                    Ctx.m_instance.m_sceneSys.loadScene(xmlItem.m_levelName, onDZResLoadScene);
                }
            }
            else
            {
                Ctx.m_instance.m_logSys.log(string.Format("xml 中没有匹配的场景 id {0} ", sceneNumber));
            }
        }

        // 这个是操作场景资源加载完成回调
        public void onGameResLoadScene(Scene scene)
        {
            Ctx.m_instance.m_gameRunStage.enteredCurStage();
        }

        // 这个是对战场景资源加载完成回调
        public void onDZResLoadScene(Scene scene)
        {
            Ctx.m_instance.m_gameRunStage.enteredCurStage();
        }

        // 加载 Main Scene UI
        protected void loadAllUIScene()
        {
            Ctx.m_instance.m_uiMgr.loadAndShow<UIMain>(UIFormID.eUIMain);

            Ctx.m_instance.m_uiSceneMgr.loadSceneForm<UISceneBg>(UISceneFormID.eUISceneBg);
            Ctx.m_instance.m_uiSceneMgr.readySceneForm(UISceneFormID.eUISceneBg);
        }

        protected void loadAllDZUIScene()
        {
            Ctx.m_instance.m_uiMgr.loadForm<UITest>(UIFormID.eUITest);
            Ctx.m_instance.m_uiMgr.loadForm<UIDZ>(UIFormID.eUIDZ);      // 显示对战场景界面
            Ctx.m_instance.m_uiMgr.loadForm<UIChat>(UIFormID.eUIChat);      // 显示聊天
            Ctx.m_instance.m_uiSceneMgr.loadAndShowForm<UISceneDZ>(UISceneFormID.eUISceneDZ);      // 显示对战场景界面
        }

        protected void unloadDZAllUIScene()
        {
            Ctx.m_instance.m_uiMgr.exitForm(UIFormID.eUITest, true);        // 退出测试
            Ctx.m_instance.m_uiMgr.exitForm(UIFormID.eUIDZ, true);          // 退出对战场景界面
            Ctx.m_instance.m_uiMgr.exitForm(UIFormID.eUIChat, true);        // 退出聊天
            Ctx.m_instance.m_uiMgr.exitForm(UIFormID.eUIExtraOp, true);     // 退出选项
            Ctx.m_instance.m_uiSceneMgr.unloadAll();
        }

        // 进入场景，但是场景还没有加载完成
        public void enteringStageHandle(EGameStage eGameStage)
        {
            if (EGameStage.eStage_Game == eGameStage)
            {

            }
            else if (EGameStage.eStage_DZ == eGameStage)
            {

            }
        }

        // 进入场景，场景资源加载成功
        public void enteredStageHandle(EGameStage eGameStage)
        {
            // 播放音乐
            //SoundParam param = Ctx.m_instance.m_poolSys.newObject<SoundParam>();
            //param.m_path = "ZuiZhenDeMeng.mp3";
            //Ctx.m_instance.m_soundMgr.play(param);
            //Ctx.m_instance.m_poolSys.deleteObj(param);

            if (EGameStage.eStage_Game == eGameStage)
            {
                if (m_isFirstEnterGame)
                {
                    Ctx.m_instance.m_camSys.m_boxCam = new SDK.Lib.BoxCam();

                    // 卸载登陆模块，关闭登陆界面
                    Ctx.m_instance.m_moduleSys.unloadModule(ModuleID.LOGINMN);
                    Ctx.m_instance.m_uiMgr.exitForm(UIFormID.eUILogin);
                    Ctx.m_instance.m_uiMgr.exitForm(UIFormID.eUIHeroSelect);
                    Ctx.m_instance.m_uiMgr.exitForm(UIFormID.eUIChat);      // 退出聊天

                    // 请求主角基本数据
                    Ctx.m_instance.m_dataPlayer.reqMainData();
                }

                Ctx.m_instance.m_logSys.log("场景加载成功");
                loadAllUIScene();
                Ctx.m_instance.m_camSys.m_boxCam.setGameObject(UtilApi.GoFindChildByPObjAndName("mcam"));
                Ctx.m_instance.m_sceneEventCB.onLevelLoaded();

                Ctx.m_instance.m_camSys.setSceneCamera2UICamera();

                m_isFirstEnterGame = false;
            }
            else if (EGameStage.eStage_DZ == eGameStage)
            {
                Ctx.m_instance.m_camSys.setSceneCamera2MainCamera();

                Ctx.m_instance.m_dataPlayer.m_dzData.clear();
                Ctx.m_instance.m_dataPlayer.m_dzData.m_canReqDZ = true;         // 进入对战就设置这个标示位为可以继续战斗
                Ctx.m_instance.m_camSys.m_dzCam = new DzCam();

                loadAllDZUIScene();
            }
        }

        // 退出场景
        public void quitStageHandle(EGameStage eGameStage)
        {
            //Ctx.m_instance.m_soundMgr.unloadAll();          // 卸载所有的音频

            if (EGameStage.eStage_Game == eGameStage)
            {
                Ctx.m_instance.m_uiSceneMgr.unloadAll();
                Ctx.m_instance.m_uiMgr.exitForm(UIFormID.eUIMain, true);        // 退出主界面
            }
            else if (EGameStage.eStage_DZ == eGameStage)
            {
                unloadDZAllUIScene();
            }
        }
    }
}