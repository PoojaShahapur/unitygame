using Game.Login;
using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 登陆界面
     */
    public class UILogin : Form, IUILogin
    {
        protected bool m_bLogined = false;      // 是否登陆过

        // 初始化控件
        override public void onReady()
        {
            addEventHandle();
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, LoginComPath.PathBtnLogin, onBtnClkLogin);
        }

        // 点击登陆处理
        protected void onBtnClkLogin()
        {
            if (!m_bLogined)
            {
                m_bLogined = true;
                Text lblName = UtilApi.getComByP<Text>(m_GUIWin.m_uiRoot, LoginComPath.PathLblName);
                Text lblPassWord = UtilApi.getComByP<Text>(m_GUIWin.m_uiRoot, LoginComPath.PathLblPassWord);

                LoginSys.m_instance.m_loginFlowHandle.connectLoginServer();
                //Ctx.m_instance.m_moduleSys.loadModule(ModuleID.GAMEMN);
            }
        }
    }
}