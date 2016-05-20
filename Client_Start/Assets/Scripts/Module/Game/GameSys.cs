using SDK.Lib;
using Game.UI;

namespace Game.Game
{
    public class GameSys : IGameSys
    {
        public GameRouteCB m_gameRouteCB;
        public GameNetHandleCB m_gameNetHandleCB;
        protected GotoScene m_gotoScene;

        public void Start()
        {
            registerScriptType();
            initGVar();
            loadGameScene();
        }

        public void initGVar()
        {
            m_gotoScene = new GotoScene();

            // 游戏逻辑处理
            Ctx.m_instance.m_cbUIEvent = new GameUIEventCB();
            m_gameNetHandleCB = new GameNetHandleCB();
            Ctx.m_instance.m_netCmdNotify.addOneDisp(m_gameNetHandleCB);
            m_gameRouteCB = new GameRouteCB();
            Ctx.m_instance.m_msgRouteNotify.addOneDisp(m_gameRouteCB);
            Ctx.m_instance.m_netCmdNotify.bStopNetHandle = false;     // 停止网络消息处理
            Ctx.m_instance.m_sceneEventCB = new GameSceneEventCB();
            Ctx.m_instance.m_sceneLogic = new GameSceneLogic();

            m_gotoScene.addSceneHandle();
        }

        public void loadGameScene()
        {
            //m_gotoScene.loadGameScene();
            m_gotoScene.loadScene("TestAnimScene.unity");
        }

        public void loadDZScene(uint sceneNumber)
        {
            m_gotoScene.loadDZScene(sceneNumber);
        }

        protected void registerScriptType()
        {
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UIBlurBg", typeof(UIBlurBg));
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UITest", typeof(UITest));
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UITerrainEdit", typeof(UITerrainEdit));
        }
    }
}