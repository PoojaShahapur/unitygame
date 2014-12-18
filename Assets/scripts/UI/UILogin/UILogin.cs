using Game.Login;
using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 登陆界面
     */
    public class UILogin : Form
    {
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
        protected void onBtnClkLogin(GameObject go)
        {
            UILabel lblName = UtilApi.getComByP<UILabel>(m_GUIWin.m_uiRoot, LoginComPath.PathLblName);
            UILabel lblPassWord = UtilApi.getComByP<UILabel>(m_GUIWin.m_uiRoot, LoginComPath.PathLblPassWord);

            LoginSys.m_instance.m_loginFlowHandle.connectLoginServer();
        }
    }
}