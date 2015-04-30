using System;
using UnityEngine;
using System.Collections.Generic;
using SDK.Common;
using Game.UI;
using SDK.Lib;

namespace Game.AutoUpdate
{
    public class AutoUpdateSys : IAutoUpdate
    {
        public void Start()
        {
            initGVar();
            startAutoUpdate();
        }

        public void initGVar()
        {
            // 游戏逻辑处理
            Ctx.m_instance.m_cbUIEvent = new AutoUpdateUIEventCB();
        }

        protected void startAutoUpdate()
        {
            Ctx.m_instance.m_pAutoUpdateSys.m_onUpdateEndDisp = onAutoUpdateEnd;
            Ctx.m_instance.m_pAutoUpdateSys.startUpdate();
            //onAutoUpdateEnd();
        }

        public void onAutoUpdateEnd()
        {
            Ctx.m_instance.m_moduleSys.loadModule(ModuleID.LOGINMN);
        }

        // 卸载模块
        public void unload()
        {
            
        }
    }
}