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
    public class UILogin : Form
    {
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

            // 或略鼠标事件
            UtilApi.getComByP<Image>(m_GUIWin.m_uiRoot, "ImageName").maskable = false;
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, LoginComPath.PathBtnLogin, onBtnClkLogin);
        }

        // 点击登陆处理
        protected void onBtnClkLogin()
        {
            if (Ctx.m_instance.m_loginSys.get_LoginState() != LoginState.eLoginingLoginServer && Ctx.m_instance.m_loginSys.get_LoginState() != LoginState.eLoginingGateServer)    // 如果没有正在登陆登陆服务器和网关服务器
            {
                InputField lblName = UtilApi.getComByP<InputField>(m_GUIWin.m_uiRoot, LoginComPath.PathLblName);
                InputField lblPassWord = UtilApi.getComByP<InputField>(m_GUIWin.m_uiRoot, LoginComPath.PathLblPassWord);

                if (validStr(lblName.text, lblPassWord.text))
                {
                    Ctx.m_instance.m_systemSetting.setString(SystemSetting.USERNAME, lblName.text);
                    Ctx.m_instance.m_systemSetting.setString(SystemSetting.PASSWORD, lblPassWord.text);

#if !DEBUG_NOTNET
                    if (Ctx.m_instance.m_loginSys.get_LoginState() != LoginState.eLoginNone)        // 先关闭之前的 socket
                    {
                        Ctx.m_instance.m_netMgr.closeSocket(Ctx.m_instance.m_cfg.m_ip, Ctx.m_instance.m_cfg.m_port);
                    }
                    Ctx.m_instance.m_loginSys.connectLoginServer(lblName.text, lblPassWord.text);
#else
                    Ctx.m_instance.m_moduleSys.loadModule(ModuleID.GAMEMN);
#endif
                }
            }
        }

        // 验证字符串
        protected bool validStr(string name, string passwd)
        {
            if(name.Length == 0)
            {
                Ctx.m_instance.m_langMgr.getText(LangTypeId.eLogin, (int)LangLogID.eItem2);
                UIInfo.showMsg(Ctx.m_instance.m_shareData.m_retLangStr);
                return false;
            }
            else if (UtilApi.IsIncludeChinese(name))
            {
                Ctx.m_instance.m_langMgr.getText(LangTypeId.eLogin, (int)LangLogID.eItem0);
                UIInfo.showMsg(Ctx.m_instance.m_shareData.m_retLangStr);
                return false;
            }

            if (name.Length == 0)
            {
                Ctx.m_instance.m_langMgr.getText(LangTypeId.eLogin, (int)LangLogID.eItem3);
                UIInfo.showMsg(Ctx.m_instance.m_shareData.m_retLangStr);
                return false;
            }
            else if (UtilApi.IsIncludeChinese(passwd))
            {
                Ctx.m_instance.m_langMgr.getText(LangTypeId.eLogin, (int)LangLogID.eItem1);
                UIInfo.showMsg(Ctx.m_instance.m_shareData.m_retLangStr);
                return false;
            }

            return true;
        }
    }
}