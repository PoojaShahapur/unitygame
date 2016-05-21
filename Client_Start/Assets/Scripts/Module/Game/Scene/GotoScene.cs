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

        // 这个是操作场景资源加载完成回调
        public void onGameResLoadScene(IDispatchObject dispObj)
        {
            Scene scene = dispObj as Scene;
            Ctx.m_instance.m_gameRunStage.quitedAndEnteredCurStage();
        }

        public void loadScene(string sceneName)
        {
            Ctx.m_instance.m_sceneSys.loadScene(sceneName, onLoadScene);
        }

        public void onLoadScene(IDispatchObject dispObj)
        {
            Scene scene = dispObj as Scene;
            Ctx.m_instance.m_sceneEventCB.onLevelLoaded();
        }

        // 加载 Main Scene UI
        protected void loadAllUIScene()
        {
            Ctx.m_instance.m_uiMgr.loadAndShow(UIFormID.eUITest);
        }

        protected void loadAllDZUIScene()
        {
            Ctx.m_instance.m_uiMgr.loadAndShow(UIFormID.eUITest);
        }

        // 第一次进入游戏场景初始化
        protected void initOnFirstEnterGameScene()
        {
            if (Ctx.m_instance.m_gameRunStage.ePreGameStage == EGameStage.eStage_Login)
            {
                // 卸载登陆模块，关闭登陆界面
                Ctx.m_instance.m_moduleSys.unloadModule(ModuleID.LOGINMN);
            }
        }

        // 进入场景，但是场景还没有加载完成
        public void quitingAndEnteringStageHandle(EGameStage srcGameState, EGameStage destGameState)
        {
            //Ctx.m_instance.m_soundMgr.unloadAll();          // 卸载所有的音频
        }

        // 进入场景，场景资源加载成功
        public void quitedAndEnteredStageHandle(EGameStage srcGameState, EGameStage destGameState)
        {
            // 播放音乐
            //SoundParam param = Ctx.m_instance.m_poolSys.newObject<SoundParam>();
            //param.m_path = Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathAudio], "ZuiZhenDeMeng.mp3");
            //Ctx.m_instance.m_soundMgr.play(param);
            //Ctx.m_instance.m_poolSys.deleteObj(param);

            if (EGameStage.eStage_Login == srcGameState)
            {
                initOnFirstEnterGameScene();
            }
        }
    }
}