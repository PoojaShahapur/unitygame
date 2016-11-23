using SDK.Lib;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 登陆界面
     */
    public class UILogin : Form, IUILogin
    {
        protected SpriteAni m_spriteAni;        //登陆界面中间的动画
        protected GameObject m_imageEffect;

        override public void onShow()
        {
            base.onShow();
        }

        // 初始化控件
        override public void onReady()
        {
            base.onReady();
            findWidget();
            addEventHandle();
        }

        override public void onExit()
        {
            base.onExit();
        }

        // 关联窗口
        protected void findWidget()
        {
            AuxInputField lblName = new AuxInputField(m_guiWin.m_uiRoot, LoginComPath.PathLblName);
            lblName.text = "111111";      //zhanghao01---zhanghao09

            if(Ctx.mInstance.mSystemSetting.getString(SystemSetting.USERNAME) != default(string))
            {
                lblName.text = Ctx.mInstance.mSystemSetting.getString(SystemSetting.USERNAME);
            }

            AuxInputField lblPassWord = new AuxInputField(m_guiWin.m_uiRoot, LoginComPath.PathLblPassWord);
            lblPassWord.text = "1";

            if (Ctx.mInstance.mSystemSetting.getString(SystemSetting.PASSWORD) != default(string))
            {
                lblPassWord.text = Ctx.mInstance.mSystemSetting.getString(SystemSetting.PASSWORD);
            }
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_guiWin.m_uiRoot, LoginComPath.PathBtnLogin, onBtnClkLogin);
            UtilApi.addEventHandle(m_guiWin.m_uiRoot, LoginComPath.ButtonRegister, createAccount);
        }

        // 点击登陆处理
        protected void onBtnClkLogin()
        {
            if (Ctx.mInstance.mLoginSys.getLoginState() != LoginState.eLoginingLoginServer && Ctx.mInstance.mLoginSys.getLoginState() != LoginState.eLoginingGateServer)    // 如果没有正在登陆登陆服务器和网关服务器
            {
                AuxInputField lblName = new AuxInputField(m_guiWin.m_uiRoot, LoginComPath.PathLblName);
                AuxInputField lblPassWord = new AuxInputField(m_guiWin.m_uiRoot, LoginComPath.PathLblPassWord);

                Ctx.mInstance.mSystemSetting.setString(SystemSetting.USERNAME, lblName.text);
                Ctx.mInstance.mSystemSetting.setString(SystemSetting.PASSWORD, lblPassWord.text);

                if (!MacroDef.DEBUG_NOTNET)
                {
                    if (Ctx.mInstance.mLoginSys.getLoginState() != LoginState.eLoginNone)        // 先关闭之前的 socket
                    {
                        Ctx.mInstance.mNetMgr.closeSocket(Ctx.mInstance.mCfg.mIp, Ctx.mInstance.mCfg.mPort);
                    }
                    Ctx.mInstance.mLoginSys.connectLoginServer(lblName.text, lblPassWord.text);
                }
                else
                {
                    Ctx.mInstance.mModuleSys.loadModule(ModuleID.GAMEMN);
                }
            }
        }

        public void createAccount()
        {

        }
    }
}