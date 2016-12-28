using Game.Login;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 登陆界面
     */
    public class UILogin : Form, IUILogin
    {
        protected AuxLabel mInfoLabel;

        public override void onInit()
        {
            base.onInit();

            this.mInfoLabel = new AuxLabel();
        }

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
            AuxInputField lblName = new AuxInputField(mGuiWin.mUiRoot, LoginComPath.PathLblName);
            lblName.text = "111111";      //zhanghao01---zhanghao09

            if(Ctx.mInstance.mSystemSetting.getString(SystemSetting.USERNAME) != default(string))
            {
                lblName.text = Ctx.mInstance.mSystemSetting.getString(SystemSetting.USERNAME);
            }

            AuxInputField lblPassWord = new AuxInputField(mGuiWin.mUiRoot, LoginComPath.PathLblPassWord);
            lblPassWord.text = "1";

            if (Ctx.mInstance.mSystemSetting.getString(SystemSetting.PASSWORD) != default(string))
            {
                lblPassWord.text = Ctx.mInstance.mSystemSetting.getString(SystemSetting.PASSWORD);
            }

            this.mInfoLabel.setSelfGo(mGuiWin.mUiRoot, LoginComPath.PathLabelInfo);
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(mGuiWin.mUiRoot, LoginComPath.PathBtnLogin, onLoginBtnClk);
            UtilApi.addEventHandle(mGuiWin.mUiRoot, LoginComPath.ButtonRegister, onCreateAccountBtnClk);
        }

        // 点击登陆处理
        protected void onLoginBtnClk()
        {
            this.loginOrCreateAccount(SelectEnterMode.eLoginAccount);
        }

        public void onCreateAccountBtnClk()
        {
            this.loginOrCreateAccount(SelectEnterMode.eCreateAccount);
        }

        protected void loginOrCreateAccount(SelectEnterMode selectEnterMode)
        {
            if (Ctx.mInstance.mLoginSys.getLoginState() != LoginState.eLoginingLoginServer && Ctx.mInstance.mLoginSys.getLoginState() != LoginState.eLoginingGateServer)    // 如果没有正在登陆登陆服务器和网关服务器
            {
                AuxInputField lblName = new AuxInputField(mGuiWin.mUiRoot, LoginComPath.PathLblName);
                AuxInputField lblPassWord = new AuxInputField(mGuiWin.mUiRoot, LoginComPath.PathLblPassWord);

                Ctx.mInstance.mSystemSetting.setString(SystemSetting.USERNAME, lblName.text);
                Ctx.mInstance.mSystemSetting.setString(SystemSetting.PASSWORD, lblPassWord.text);

                if (lblName.text.Length > 0 && lblPassWord.text.Length > 5)
                {
                    if (!MacroDef.DEBUG_NOTNET)
                    {
                        //if (Ctx.mInstance.mLoginSys.getLoginState() != LoginState.eLoginNone)        // 先关闭之前的 socket
                        //{
                        //    Ctx.mInstance.mNetMgr.closeSocket(Ctx.mInstance.mCfg.mIp, Ctx.mInstance.mCfg.mPort);
                        //}
                        //Ctx.mInstance.mLoginSys.connectLoginServer(lblName.text, lblPassWord.text, selectEnterMode);

                        (Ctx.mInstance.mLoginSys as LoginSys).mLoginNetHandleCB_KBE.setAccountAndPasswd(lblName.text, lblPassWord.text);

                        if (SelectEnterMode.eLoginAccount == selectEnterMode)
                        {
                            (Ctx.mInstance.mLoginSys as LoginSys).mLoginNetHandleCB_KBE.login();
                        }
                        else if(SelectEnterMode.eCreateAccount == selectEnterMode)
                        {
                            (Ctx.mInstance.mLoginSys as LoginSys).mLoginNetHandleCB_KBE.createAccount();
                        }
                    }
                    else
                    {
                        Ctx.mInstance.mModuleSys.loadModule(ModuleId.GAMEMN);
                    }
                }
                else
                {
                    err("account or password is error, length < 6!(账号或者密码错误，长度必须大于5!)");
                }
            }
        }

        public void err(string s)
        {
            this.mInfoLabel.setColor(Color.red);
            this.mInfoLabel.setText(s);
        }

        public void info(string s)
        {
            this.mInfoLabel.setColor(Color.green);
            this.mInfoLabel.setText(s);
        }
    }
}