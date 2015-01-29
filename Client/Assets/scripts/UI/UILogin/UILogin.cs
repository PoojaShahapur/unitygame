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

        override public void onShow()
        {

        }

        // 初始化控件
        override public void onReady()
        {
            getWidget();
            addEventHandle();
        }

        // 关联窗口
        protected void getWidget()
        {
            InputField lblName = UtilApi.getComByP<InputField>(m_GUIWin.m_uiRoot, LoginComPath.PathLblName);
            lblName.text = "zhanghao06";      //zhanghao01---zhanghao09

            if(Ctx.m_instance.m_systemSetting.getString(SystemSetting.USERNAME) != default(string))
            {
                lblName.text = Ctx.m_instance.m_systemSetting.getString(SystemSetting.USERNAME);
            }

            InputField lblPassWord = UtilApi.getComByP<InputField>(m_GUIWin.m_uiRoot, LoginComPath.PathLblPassWord);
            lblPassWord.text = "1";

            if (Ctx.m_instance.m_systemSetting.getString(SystemSetting.PASSWORD) != default(string))
            {
                lblPassWord.text = Ctx.m_instance.m_systemSetting.getString(SystemSetting.PASSWORD);
            }
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
                InputField lblName = UtilApi.getComByP<InputField>(m_GUIWin.m_uiRoot, LoginComPath.PathLblName);
                InputField lblPassWord = UtilApi.getComByP<InputField>(m_GUIWin.m_uiRoot, LoginComPath.PathLblPassWord);

                Ctx.m_instance.m_systemSetting.setString(SystemSetting.USERNAME, lblName.text);
                Ctx.m_instance.m_systemSetting.setString(SystemSetting.PASSWORD, lblPassWord.text);

                if (Ctx.m_instance.m_cfg.m_bNeedNet)
                {
                    LoginSys.m_instance.m_loginFlowHandle.connectLoginServer(lblName.text, lblPassWord.text);
                }
                else
                {
                    Ctx.m_instance.m_moduleSys.loadModule(ModuleID.GAMEMN);
                }
            }
        }
    }
}