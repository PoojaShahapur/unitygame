using Game.UI;
using SDK.Lib;

namespace Game.Login
{
    public class LoginSys : ILoginSys
    {
        public LoginFlowHandle mLoginFlowHandle;       // 整个登陆流程处理
        public LoginState mLoginState;                 // 登陆状态
        public LoginRouteCB mLoginRouteCB;
        public LoginNetHandleCB mLoginNetHandleCB;

        public LoginSys()
        {
            this.mLoginState = LoginState.eLoginNone;
        }

        public void Start()
        {
            this.initCore();
        }

        // 初始化核心组件，并且等待组件加载完成
        protected void initCore()
        {
            new GameBox.Framework.ServicesTask(new string[] {
                "com.giant.service.giantlightserver",
            }).Start().Continue(task =>
            {
                this.startImpl();
                return null;
            });
        }

        protected void startImpl()
        {
            Ctx.mInstance.mModuleSys.unloadModule(ModuleId.AUTOUPDATEMN);
            this.registerScriptType();
            this.initGVar();
            //this.loadScene();
            this.initGB();
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

            //Ctx.mInstance.mUiMgr.loadAndShow(UIFormID.eUILogin);
            //Ctx.mInstance.mUiMgr.loadAndShow(UIFormID.eUISelectRole);
            Ctx.mInstance.mLuaSystem.openForm((int)UIFormId.eUIStartGame_Lua);//参数：UIFormID.lua中genNewId()
            //Ctx.mInstance.mUiMgr.loadAndShow(UIFormID.eUITest);
        }

        protected void initGB()
        {
            Ctx.mInstance.mLightServer_GB = GameBox.Framework.ServiceCenter.GetService<GameBox.Service.GiantLightServer.IGiantLightServer>();

            var proxy = Ctx.mInstance.mLightServer_GB.CreateProxy("rpc.Login", GameBox.Service.GiantLightServer.ServiceType.PULL);
            proxy.Register<rpc.LoginResponse>("Login", this.mLoginNetHandleCB.OnLogin);


            Ctx.mInstance.mProxy_GB = proxy;

            var handler = new Giant.GiantLightServerHandler(Ctx.mInstance.mLightServer_GB);
            handler.OnDisconnectHandler = Ctx.mInstance.mNetEventHandle.OnDisconnect;
            Ctx.mInstance.mServerHandler_GB = handler;
            handler.BeginService("plane.Plane");
            handler.Register<rpc.EmptyMsg, plane.EnterRoomResponse>("EnterRoom", this.mLoginNetHandleCB.OnEnterRoom);
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

        protected void leave_GB()
        {
            var handler = Ctx.mInstance.mServerHandler_GB;
            if (handler != null)
                handler.EndService("rpc.Login");
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