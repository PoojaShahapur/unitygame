using SDK.Lib;
using Game.UI;

namespace Game.Game
{
    public class GameSys : IGameSys
    {
        public GameRouteCB mGameRouteCB;
        public GameNetHandleCB mGameNetHandleCB;
        protected GotoScene mGotoScene;

        public void Start()
        {
            this.registerScriptType();
            this.initGVar();
            this.loadGameScene();
        }

        public void initGVar()
        {
            this.mGotoScene = new GotoScene();

            // 游戏逻辑处理
            Ctx.mInstance.mCbUIEvent = new GameUIEventCB();

            this.mGameNetHandleCB = new GameNetHandleCB();
            this.mGameNetHandleCB.init();
            Ctx.mInstance.mNetCmdNotify.addOneDisp(this.mGameNetHandleCB);
            this.mGameNetHandleCB.init_GB();

            this.mGameRouteCB = new GameRouteCB();
            this.mGameRouteCB.init();
            Ctx.mInstance.mMsgRouteNotify.addOneDisp(this.mGameRouteCB);

            Ctx.mInstance.mNetCmdNotify.isStopNetHandle = false;     // 停止网络消息处理
            Ctx.mInstance.mSceneEventCB = new GameSceneEventCB();
            Ctx.mInstance.mSceneLogic = new GameSceneLogic();

            this.mGotoScene.addSceneHandle();
        }

        public void loadGameScene()
        {
            //this.mGotoScene.loadScene("TestScene.unity");
            this.mGotoScene.loadScene("NewWorldTest.unity");
        }

        protected void registerScriptType()
        {
            Ctx.mInstance.mScriptDynLoad.registerScriptType("Game.UI.UIBlurBg", typeof(UIBlurBg));
            Ctx.mInstance.mScriptDynLoad.registerScriptType("Game.UI.UITest", typeof(UITest));
            Ctx.mInstance.mScriptDynLoad.registerScriptType("Game.UI.UITerrainEdit", typeof(UITerrainEdit));
            Ctx.mInstance.mScriptDynLoad.registerScriptType("Game.UI.UIPack", typeof(UIPack));
            Ctx.mInstance.mScriptDynLoad.registerScriptType("Game.UI.UIJoyStick", typeof(UIJoyStick));
            Ctx.mInstance.mScriptDynLoad.registerScriptType("Game.UI.UIForwardForce", typeof(UIForwardForce));
        }
    }
}