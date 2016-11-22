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
            Ctx.m_instance.m_moduleSys.unloadModule(ModuleID.AUTOUPDATEMN);
            this.registerScriptType();
            this.initGVar();
            //this.loadScene();
            this.onResLoadScene(null);
        }

        public void initGVar()
        {
            // 游戏逻辑处理
            Ctx.m_instance.m_cbUIEvent = new LoginUIEventCB();
            this.mLoginNetHandleCB = new LoginNetHandleCB();
            Ctx.m_instance.m_netCmdNotify.addOneDisp(this.mLoginNetHandleCB);
            this.mLoginRouteCB = new LoginRouteCB();
            Ctx.m_instance.m_msgRouteNotify.addOneDisp(this.mLoginRouteCB);
        }

        // 加载登陆常见
        public void loadScene()
        {
            Ctx.m_instance.m_sceneSys.loadScene("login.unity", onResLoadScene);
        }

        public void onResLoadScene(IDispatchObject dispObj)
        {
            Scene scene = dispObj as Scene;
            Ctx.m_instance.m_gameRunStage.toggleGameStage(EGameStage.eStage_Login);
            Ctx.m_instance.m_logSys.log("加载场景");
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
            Ctx.m_instance.m_netCmdNotify.removeOneDisp(this.mLoginNetHandleCB);
            Ctx.m_instance.m_msgRouteNotify.removeOneDisp(this.mLoginRouteCB);
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