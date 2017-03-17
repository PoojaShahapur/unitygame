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
            Ctx.mInstance.mGameRunStage.addQuitingAndEnteringDisp(quitingAndEnteringStageHandle);
            Ctx.mInstance.mGameRunStage.addQuitedAndEnteredDisp(quitedAndEnteredStageHandle);
        }

        public void loadGameScene()
        {
            if (!Ctx.mInstance.mGameRunStage.isCurInStage(EGameStage.eStage_Game))
            {
                Ctx.mInstance.mGameRunStage.toggleGameStage(EGameStage.eStage_Game);
                Ctx.mInstance.mSceneSys.loadScene("Game.unity", onGameResLoadScene);
            }
        }

        // 这个是操作场景资源加载完成回调
        public void onGameResLoadScene(IDispatchObject dispObj)
        {
            Scene scene = dispObj as Scene;
            Ctx.mInstance.mGameRunStage.quitedAndEnteredCurStage();
        }

        public void loadScene(string sceneName)
        {
            Ctx.mInstance.mSceneSys.loadScene(sceneName, onLoadScene);
        }

        public void onLoadScene(IDispatchObject dispObj)
        {
            Scene scene = dispObj as Scene;
            Ctx.mInstance.mSceneEventCB.onLevelLoaded();
        }

        // 加载 Main Scene UI
        protected void loadAllUIScene()
        {
            Ctx.mInstance.mUiMgr.loadAndShow(UIFormId.eUITest);
        }

        protected void loadAllDZUIScene()
        {
            Ctx.mInstance.mUiMgr.loadAndShow(UIFormId.eUITest);
        }

        // 第一次进入游戏场景初始化
        protected void initOnFirstEnterGameScene()
        {
            if (Ctx.mInstance.mGameRunStage.ePreGameStage == EGameStage.eStage_Login)
            {
                // 卸载登陆模块，关闭登陆界面
                Ctx.mInstance.mModuleSys.unloadModule(ModuleId.LOGINMN);
            }
        }

        // 进入场景，但是场景还没有加载完成
        public void quitingAndEnteringStageHandle(EGameStage srcGameState, EGameStage destGameState)
        {
            //Ctx.mInstance.m_soundMgr.unloadAll();          // 卸载所有的音频
        }

        // 进入场景，场景资源加载成功
        public void quitedAndEnteredStageHandle(EGameStage srcGameState, EGameStage destGameState)
        {
            // 播放音乐
            //SoundParam param = Ctx.mInstance.mPoolSys.newObject<SoundParam>();
            //param.mPath = Path.Combine(Ctx.mInstance.mCfg.m_pathLst[(int)ResPathType.ePathAudio], "ZuiZhenDeMeng.mp3");
            //Ctx.mInstance.m_soundMgr.play(param);
            //Ctx.mInstance.mPoolSys.deleteObj(param);

            if (EGameStage.eStage_Login == srcGameState)
            {
                this.initOnFirstEnterGameScene();
            }
        }
    }
}