using Game.UI;
using SDK.Common;
using SDK.Lib;

namespace Game.Game
{
    /**
     * @brief 处理各种场景切换
     */
    public class GotoScene
    {
        public void addSceneHandle()
        {
            Ctx.m_instance.m_gameRunStage.addQuitingAndEnteringDisp(quitingAndEnteringStageHandle);
            Ctx.m_instance.m_gameRunStage.addQuitedAndEnteredDisp(quitedAndEnteredStageHandle);
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
            Ctx.m_instance.m_gameRunStage.quitedAndEnteredCurStage();
        }

        // 这个是对战场景资源加载完成回调
        public void onDZResLoadScene(Scene scene)
        {
            Ctx.m_instance.m_gameRunStage.quitedAndEnteredCurStage();
        }

        // 加载 Main Scene UI
        protected void loadAllUIScene()
        {
            Ctx.m_instance.m_uiMgr.loadAndShow<UIMain>(UIFormID.eUIMain);

            Ctx.m_instance.m_uiMgr.m_UIAttrs.m_dicAttr[UIFormID.eUIGM].addUISceneType(UISceneType.eUIScene_Game);
            Ctx.m_instance.m_uiMgr.loadAndShow<UIGM>(UIFormID.eUIGM);
        }

        protected void loadAllDZUIScene()
        {
            Ctx.m_instance.m_uiMgr.loadForm<UITest>(UIFormID.eUITest);
            Ctx.m_instance.m_uiMgr.loadForm<UIDZ>(UIFormID.eUIDZ);      // 显示对战场景界面
            Ctx.m_instance.m_uiMgr.loadForm<UIChat>(UIFormID.eUIChat);      // 显示聊天
            Ctx.m_instance.m_uiSceneMgr.loadAndShowForm<UISceneDZ>(UISceneFormID.eUISceneDZ);      // 显示对战场景界面
            Ctx.m_instance.m_uiMgr.m_UIAttrs.m_dicAttr[UIFormID.eUIGM].addUISceneType(UISceneType.eUIScene_DZ);
            Ctx.m_instance.m_uiMgr.loadAndShow<UIGM>(UIFormID.eUIGM);
        }

        // 第一次进入游戏场景初始化
        protected void initOnFirstEnterGameScene()
        {
            if (Ctx.m_instance.m_gameRunStage.ePreGameStage == EGameStage.eStage_Login)
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
        }

        // 进入场景，但是场景还没有加载完成
        public void quitingAndEnteringStageHandle(EGameStage srcGameState, EGameStage destGameState)
        {
            //Ctx.m_instance.m_soundMgr.unloadAll();          // 卸载所有的音频

            if (EGameStage.eStage_Game == srcGameState)
            {
                // 必然是从游戏场景进入战斗场景
                Ctx.m_instance.m_uiMgr.unloadUIBySceneType(UISceneType.eUIScene_Game, UISceneType.eUIScene_DZ);
            }
            else if (EGameStage.eStage_DZ == srcGameState)
            {
                Ctx.m_instance.m_uiMgr.unloadUIBySceneType(UISceneType.eUIScene_DZ, UISceneType.eUIScene_Game);        // 退出测试
                Ctx.m_instance.m_uiSceneMgr.unloadAll();
            }
        }

        // 进入场景，场景资源加载成功
        public void quitedAndEnteredStageHandle(EGameStage srcGameState, EGameStage destGameState)
        {
            // 播放音乐
            //SoundParam param = Ctx.m_instance.m_poolSys.newObject<SoundParam>();
            //param.m_path = "ZuiZhenDeMeng.mp3";
            //Ctx.m_instance.m_soundMgr.play(param);
            //Ctx.m_instance.m_poolSys.deleteObj(param);

            if (EGameStage.eStage_Login == srcGameState)
            {
                initOnFirstEnterGameScene();
            }

            if (EGameStage.eStage_Game == destGameState)
            {
                Ctx.m_instance.m_logSys.log("场景加载成功");
                loadAllUIScene();
                Ctx.m_instance.m_camSys.m_boxCam.setGameObject(UtilApi.GoFindChildByPObjAndName("mcam"));
                Ctx.m_instance.m_sceneEventCB.onLevelLoaded();
                Ctx.m_instance.m_camSys.setSceneCamera2UICamera();
            }
            else if (EGameStage.eStage_DZ == destGameState)
            {
                Ctx.m_instance.m_camSys.setSceneCamera2MainCamera();

                Ctx.m_instance.m_dataPlayer.m_dzData.clear();
                Ctx.m_instance.m_dataPlayer.m_dzData.m_canReqDZ = true;         // 进入对战就设置这个标示位为可以继续战斗
                Ctx.m_instance.m_camSys.m_dzCam = new DzCam();
                loadAllDZUIScene();
            }
        }
    }
}