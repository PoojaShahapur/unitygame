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
            Ctx.m_instance.m_cbUIEvent = new GameUIEventCB();
            this.mGameNetHandleCB = new GameNetHandleCB();
            Ctx.m_instance.m_netCmdNotify.addOneDisp(mGameNetHandleCB);
            this.mGameRouteCB = new GameRouteCB();
            Ctx.m_instance.m_msgRouteNotify.addOneDisp(mGameRouteCB);
            Ctx.m_instance.m_netCmdNotify.bStopNetHandle = false;     // 停止网络消息处理
            Ctx.m_instance.m_sceneEventCB = new GameSceneEventCB();
            Ctx.m_instance.m_sceneLogic = new GameSceneLogic();

            this.mGotoScene.addSceneHandle();
        }

        public void loadGameScene()
        {
            this.mGotoScene.loadScene("TestScene.unity");
        }

        protected void registerScriptType()
        {
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UIBlurBg", typeof(UIBlurBg));
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UITest", typeof(UITest));
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UITerrainEdit", typeof(UITerrainEdit));
        }
    }
}