﻿using System;
using UnityEngine;
using System.Collections.Generic;
using SDK.Common;
using Game.UI;

namespace Game.Login
{
    public class LoginSys
    {
        static public LoginSys m_instance;
        public Ctx m_ctx;
        public ILoginFlowHandle m_loginFlowHandle;       // 整个登陆流程处理

        public void Start()
        {
            initGVar();
            loadScene();
        }

        public void initGVar()
        {
            // 获取全局变量
            GameObject nodestroy = UtilApi.GoFindChildByPObjAndName(NotDestroyPath.ND_CV_App);
            AppRoot approot = nodestroy.GetComponent<AppRoot>();
            LoginSys.m_instance.m_ctx = approot.getCtx();

            // 场景逻辑处理逻辑
            LoginSys.m_instance.m_ctx.m_uiMgr.SetIUIFactory(new LoginUIFactory());
            // 游戏逻辑处理
            Ctx.m_instance.m_cbUIEvent = new LoginUIEventCB();
            Ctx.m_instance.m_netHandle = new LoginNetHandleCB();
        }

        // 加载登陆常见
        public void loadScene()
        {
            Ctx.m_instance.m_sceneSys.loadScene("login", onResLoadScene);
        }

        public void onResLoadScene(IScene scene)
        {
            Ctx.m_instance.m_log.log("aaa");
            // 加载登陆界面
            Ctx.m_instance.m_uiMgr.loadForm(UIFormID.UILogin);
        }
    }
}