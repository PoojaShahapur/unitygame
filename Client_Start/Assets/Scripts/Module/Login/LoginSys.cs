using Game.UI;
using SDK.Lib;

namespace Game.Login
{
    public class LoginSys : ILoginSys
    {
        public LoginFlowHandle mLoginFlowHandle;       // 整个登陆流程处理
        public LoginState mLoginState;                 // 登陆状态
        public LoginRouteCB mLoginRouteCB;
        public LoginNetHandleCB_KBE mLoginNetHandleCB_KBE;    // KBEngine 引擎对接
        public LoginNetHandleCB mLoginNetHandleCB;

        public LoginSys()
        {
            this.mLoginState = LoginState.eLoginNone;
        }

        public void Start()
        {
            Ctx.mInstance.mModuleSys.unloadModule(ModuleID.AUTOUPDATEMN);
            this.registerScriptType();
            this.initGVar();
            //this.loadScene();
            this.onResLoadScene(null);
        }

        public void initGVar()
        {
            // 游戏逻辑处理
            Ctx.mInstance.mCbUIEvent = new LoginUIEventCB();
            this.mLoginNetHandleCB = new LoginNetHandleCB();
            Ctx.mInstance.mNetCmdNotify.addOneDisp(this.mLoginNetHandleCB);
            this.mLoginRouteCB = new LoginRouteCB();
            Ctx.mInstance.mMsgRouteNotify.addOneDisp(this.mLoginRouteCB);

            this.mLoginNetHandleCB_KBE = new LoginNetHandleCB_KBE();
            this.mLoginNetHandleCB_KBE.init();

            //Ctx.mInstance.mUiMgr.loadAndShow(UIFormID.eUILogin);
            //Ctx.mInstance.mUiMgr.loadAndShow(UIFormID.eUISelectRole);
            Ctx.mInstance.mLuaSystem.openForm((int)UIFormID.eUIStartGame);//参数：UIFormID.lua中genNewId()
            //Ctx.mInstance.mUiMgr.loadAndShow(UIFormID.eUITest);
        }

        // 加载登陆常见
        public void loadScene()
        {
            Ctx.mInstance.mSceneSys.loadScene("login.unity", onResLoadScene);
        }

        public void onResLoadScene(IDispatchObject dispObj)
        {
            Scene scene = dispObj as Scene;
            Ctx.mInstance.mGameRunStage.toggleGameStage(EGameStage.eStage_Login);
            //Ctx.mInstance.mLogSys.log("Login Scene is loaded");
        }

        public void connectLoginServer(string name, string passwd, SelectEnterMode selectEnterMode)
        {
            this.mLoginFlowHandle.connectLoginServer(name, passwd, selectEnterMode);
        }

        public LoginState getLoginState()
        {
            return this.mLoginState;
        }

        public void setLoginState(LoginState state)
        {
            this.mLoginState = state;
        }

        // 卸载模块
        public void unload()
        {
            Ctx.mInstance.mNetCmdNotify.removeOneDisp(this.mLoginNetHandleCB);
            Ctx.mInstance.mMsgRouteNotify.removeOneDisp(this.mLoginRouteCB);
        }

        public uint getUserID()
        {
            return this.mLoginFlowHandle.getDwUserID();
        }

        protected void registerScriptType()
        {
            Ctx.mInstance.mScriptDynLoad.registerScriptType("Game.UI.UILogin", typeof(UILogin));
            Ctx.mInstance.mScriptDynLoad.registerScriptType("Game.UI.UISelectRole", typeof(UISelectRole));
            Ctx.mInstance.mScriptDynLoad.registerScriptType("Game.UI.UITest", typeof(UITest));
        }
    }
}