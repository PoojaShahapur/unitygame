using Game.UI;
using KBEngine;
using SDK.Lib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Login
{
    /**
     * @brief KBEngine 登陆网络处理
     */
    public class LoginNetHandleCB_KBE
    {
        public int ui_state = 0;
        private string stringAccount = "";
        private string stringPasswd = "";
        private string labelMsg = "";
        private Color labelColor = Color.green;

        private Dictionary<UInt64, Dictionary<string, object>> ui_avatarList = null;

        private string stringAvatarName = "";
        private bool startCreateAvatar = false;

        private UInt64 selAvatarDBID = 0;
        public bool showReliveGUI = false;

        public LoginNetHandleCB_KBE()
        {
            
        }

        public void setAccountAndPasswd(string account, string passwd)
        {
            stringAccount = account;
            stringPasswd = passwd;
        }

        public void init()
        {
            this.installEvents();
        }

        void installEvents()
        {
            // common
            KBEngine.Event.registerOut("onKicked", this, "onKicked");
            KBEngine.Event.registerOut("onDisableConnect", this, "onDisableConnect");
            KBEngine.Event.registerOut("onConnectStatus", this, "onConnectStatus");

            // login
            KBEngine.Event.registerOut("onCreateAccountResult", this, "onCreateAccountResult");
            KBEngine.Event.registerOut("onLoginFailed", this, "onLoginFailed");
            KBEngine.Event.registerOut("onVersionNotMatch", this, "onVersionNotMatch");
            KBEngine.Event.registerOut("onScriptVersionNotMatch", this, "onScriptVersionNotMatch");
            KBEngine.Event.registerOut("onLoginBaseappFailed", this, "onLoginBaseappFailed");
            KBEngine.Event.registerOut("onLoginSuccessfully", this, "onLoginSuccessfully");
            KBEngine.Event.registerOut("onLoginBaseapp", this, "onLoginBaseapp");
            KBEngine.Event.registerOut("Loginapp_importClientMessages", this, "Loginapp_importClientMessages");
            KBEngine.Event.registerOut("Baseapp_importClientMessages", this, "Baseapp_importClientMessages");
            KBEngine.Event.registerOut("Baseapp_importClientEntityDef", this, "Baseapp_importClientEntityDef");

            // select-avatars
            KBEngine.Event.registerOut("onReqAvatarList", this, "onReqAvatarList");
            KBEngine.Event.registerOut("onCreateAvatarResult", this, "onCreateAvatarResult");
            KBEngine.Event.registerOut("onRemoveAvatar", this, "onRemoveAvatar");
        }

        public void dispose()
        {
            KBEngine.Event.deregisterOut(this);
        }

        public void err(string s)
        {
            labelColor = Color.red;
            labelMsg = s;

            UILogin uiLogin = Ctx.mInstance.mUiMgr.getForm(UIFormID.eUILogin) as UILogin;
            if(null != uiLogin)
            {
                uiLogin.err(s);
            }

            UISelectRole uiSelectRole = Ctx.mInstance.mUiMgr.getForm(UIFormID.eUISelectRole) as UISelectRole;
            if(null != uiSelectRole)
            {
                uiSelectRole.err(s);
            }
        }

        public void info(string s)
        {
            labelColor = Color.green;
            labelMsg = s;

            UILogin uiLogin = Ctx.mInstance.mUiMgr.getForm(UIFormID.eUILogin) as UILogin;
            if (null != uiLogin)
            {
                uiLogin.info(s);
            }

            UISelectRole uiSelectRole = Ctx.mInstance.mUiMgr.getForm(UIFormID.eUISelectRole) as UISelectRole;
            if (null != uiSelectRole)
            {
                uiSelectRole.info(s);
            }
        }

        public void login()
        {
            info("connect to server...(连接到服务端...)");
            KBEngine.Event.fireIn("login", stringAccount, stringPasswd, System.Text.Encoding.UTF8.GetBytes("kbengine_unity3d_demo"));
        }

        public void createAccount()
        {
            info("connect to server...(连接到服务端...)");

            KBEngine.Event.fireIn("createAccount", stringAccount, stringPasswd, System.Text.Encoding.UTF8.GetBytes("kbengine_unity3d_demo"));
        }

        public void onCreateAccountResult(UInt16 retcode, byte[] datas)
        {
            if (retcode != 0)
            {
                err("createAccount is error(注册账号错误)! err=" + KBEngineApp.app.serverErr(retcode));
                return;
            }

            if (KBEngineApp.validEmail(stringAccount))
            {
                info("createAccount is successfully, Please activate your Email!(注册账号成功，请激活Email!)");
            }
            else
            {
                info("createAccount is successfully!(注册账号成功!)");
            }
        }

        public void onConnectStatus(bool success)
        {
            if (!success)
                err("connect(" + KBEngineApp.app.getInitArgs().ip + ":" + KBEngineApp.app.getInitArgs().port + ") is error! (连接错误)");
            else
                info("connect successfully, please wait...(连接成功，请等候...)");
        }

        public void onLoginFailed(UInt16 failedcode)
        {
            if (failedcode == 20)
            {
                err("login is failed(登陆失败), err=" + KBEngineApp.app.serverErr(failedcode) + ", " + System.Text.Encoding.ASCII.GetString(KBEngineApp.app.serverdatas()));
            }
            else
            {
                err("login is failed(登陆失败), err=" + KBEngineApp.app.serverErr(failedcode));
            }
        }

        public void onVersionNotMatch(string verInfo, string serVerInfo)
        {
            err("");
        }

        public void onScriptVersionNotMatch(string verInfo, string serVerInfo)
        {
            err("");
        }

        public void onLoginBaseappFailed(UInt16 failedcode)
        {
            err("loginBaseapp is failed(登陆网关失败), err=" + KBEngineApp.app.serverErr(failedcode));
        }

        public void onLoginBaseapp()
        {
            info("connect to loginBaseapp, please wait...(连接到网关， 请稍后...)");
        }

        public void onLoginSuccessfully(UInt64 rndUUID, Int32 eid, Account accountEntity)
        {
            info("login is successfully!(登陆成功!)");

            //Ctx.mInstance.mUiMgr.exitForm(UIFormID.eUILogin);
            Ctx.mInstance.mLuaSystem.exitForm(10001);
            Ctx.mInstance.mUiMgr.loadAndShow(UIFormID.eUISelectRole);

            ui_state = 1;
            //Application.LoadLevel("selavatars");
        }

        public void onKicked(UInt16 failedcode)
        {
            err("kick, disconnect!, reason=" + KBEngineApp.app.serverErr(failedcode));
            //Application.LoadLevel("login");

            ui_state = 0;
            Ctx.mInstance.mUiMgr.loadAndShow(UIFormID.eUILogin);
        }

        public void Loginapp_importClientMessages()
        {
            info("Loginapp_importClientMessages ...");
        }

        public void Baseapp_importClientMessages()
        {
            info("Baseapp_importClientMessages ...");
        }

        public void Baseapp_importClientEntityDef()
        {
            info("importClientEntityDef ...");
        }

        public void onReqAvatarList(Dictionary<UInt64, Dictionary<string, object>> avatarList)
        {
            ui_avatarList = avatarList;

            UISelectRole uiSelectRole = Ctx.mInstance.mUiMgr.getForm(UIFormID.eUISelectRole) as UISelectRole;
            if (null != uiSelectRole)
            {
                uiSelectRole.setAvatarList(ui_avatarList);
            }
        }

        public void onCreateAvatarResult(Byte retcode, object info, Dictionary<UInt64, Dictionary<string, object>> avatarList)
        {
            if (retcode != 0)
            {
                err("Error creating avatar, errcode=" + retcode);
                return;
            }

            onReqAvatarList(avatarList);
        }

        public void onRemoveAvatar(UInt64 dbid, Dictionary<UInt64, Dictionary<string, object>> avatarList)
        {
            if (dbid == 0)
            {
                err("Delete the avatar error!(删除角色错误!)");
                return;
            }

            onReqAvatarList(avatarList);
        }

        public void onDisableConnect()
        {

        }
    }
}