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
            Ctx.m_instance.m_netDispList.addOneDisp(m_gameNetHandleCB);
            m_gameRouteCB = new GameRouteCB();
            Ctx.m_instance.m_msgRouteList.addOneDisp(m_gameRouteCB);
            Ctx.m_instance.m_netDispList.bStopNetHandle = false;     // 停止网络消息处理
            Ctx.m_instance.m_sceneEventCB = new GameSceneEventCB();
            Ctx.m_instance.m_sceneLogic = new GameSceneLogic();

            m_gotoScene.addSceneHandle();

            //Ctx.m_instance.m_meshMgr.loadSkinInfo();
            //Ctx.m_instance.m_aiSystem.getBehaviorTreeMgr().loadBT();

            // 立即加载 UIBlurBg 界面
            Ctx.m_instance.m_uiMgr.loadForm(UIFormID.eUIBlurBg);
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
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UIChat", typeof(UIChat));
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UIDZ", typeof(UIDZ));
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UIExtraOp", typeof(UIExtraOp));

            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UIGM", typeof(UIGM));
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UIHero", typeof(UIHero));
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UIHeroSelect", typeof(UIHeroSelect));
            //Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UIInfo", typeof(UIInfo));

            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UIJobSelect", typeof(UIJobSelect));
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UILogicTest", typeof(UILogicTest));
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UIMain", typeof(UIMain));
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UIOpenPack", typeof(UIOpenPack));

            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UIPack", typeof(UIPack));
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UIShop", typeof(UIShop));
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UITest", typeof(UITest));
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UITuJian", typeof(UITuJian));

            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UITuJianTop", typeof(UITuJianTop));
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UITask", typeof(UITask));
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UIMaze", typeof(UIMaze));
            Ctx.m_instance.m_scriptDynLoad.registerScriptType("Game.UI.UITerrainEdit", typeof(UITerrainEdit));
        }
    }
}