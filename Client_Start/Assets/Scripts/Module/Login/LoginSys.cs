using SDK.Lib;
using Game.UI;

namespace Game.Login
{
    public class LoginSys : ILoginSys
    {
        public LoginFlowHandle mLoginFlowHandle;       // 整个登陆流程处理
        public LoginState mLoginState;                 // 登陆状态
        public LoginRouteCB mLoginRouteCB;
        public LoginNetHandleCB mLoginNetHandleCB;

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
            Ctx.mInstance.mLogSys.log("加载场景");
        }

        public void connectLoginServer(string name, string passwd)
        {
            this.mLoginFlowHandle.connectLoginServer(name, passwd);
        }

        public LoginState get_LoginState()
        {
            return this.mLoginState;
        }

        public void set_LoginState(LoginState state)
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

        }
    }
}