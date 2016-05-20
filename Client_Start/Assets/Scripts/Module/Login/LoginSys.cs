﻿using SDK.Lib;
using Game.UI;

namespace Game.Login
{
    public class LoginSys : ILoginSys
    {
        public LoginFlowHandle m_loginFlowHandle;       // 整个登陆流程处理
        public LoginState m_loginState;                 // 登陆状态
        public LoginRouteCB m_loginRouteCB;
        public LoginNetHandleCB m_loginNetHandleCB;

        public void Start()
        {
            Ctx.m_instance.m_moduleSys.unloadModule(ModuleID.AUTOUPDATEMN);
            registerScriptType();
            initGVar();
            //loadScene();
            onResLoadScene(null);
        }

        public void initGVar()
        {
            // 游戏逻辑处理
            Ctx.m_instance.m_cbUIEvent = new LoginUIEventCB();
            m_loginNetHandleCB = new LoginNetHandleCB();
            Ctx.m_instance.m_netCmdNotify.addOneDisp(m_loginNetHandleCB);
            m_loginRouteCB = new LoginRouteCB();
            Ctx.m_instance.m_msgRouteNotify.addOneDisp(m_loginRouteCB);
        }

        // 加载登陆常见
        public void loadScene()
        {
            Ctx.m_instance.m_sceneSys.loadScene("login.unity", onResLoadScene);
        }

        public void onResLoadScene(Scene scene)
        {
            Ctx.m_instance.m_gameRunStage.toggleGameStage(EGameStage.eStage_Login);
            Ctx.m_instance.m_logSys.log("加载场景");
        }

        public void connectLoginServer(string name, string passwd)
        {
            m_loginFlowHandle.connectLoginServer(name, passwd);
        }

        public LoginState get_LoginState()
        {
            return m_loginState;
        }

        public void set_LoginState(LoginState state)
        {
            m_loginState = state;
        }

        // 卸载模块
        public void unload()
        {
            Ctx.m_instance.m_netCmdNotify.removeOneDisp(m_loginNetHandleCB);
            Ctx.m_instance.m_msgRouteNotify.removeOneDisp(m_loginRouteCB);
        }

        public uint getUserID()
        {
            return m_loginFlowHandle.getDwUserID();
        }

        protected void registerScriptType()
        {

        }
    }
}