﻿using Game.UI;
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
        private string stringNickName = "";
        private string stringAccount = "";
        private string stringPasswd = "111111";
        private string labelMsg = "";
        private Color labelColor = Color.green;

        private Dictionary<UInt64, Dictionary<string, object>> ui_avatarList = null;

        //private string stringAvatarName = "";
        //private bool startCreateAvatar = false;

        //private UInt64 selAvatarDBID = 0;
        public bool showReliveGUI = false;

        private bool isrelogin = false;
        private static double lastReqLogin = 0.0f;

        public LoginNetHandleCB_KBE()
        {
            
        }

        public bool getIsRelogin()
        {
            return isrelogin;
        }

        public void setAccountAndPasswd(string account, string passwd)
        {
            stringAccount = account;
            stringPasswd = passwd;
        }

        public void setNickName(string nickname)
        {
            stringNickName = nickname;
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

            /*UILogin uiLogin = Ctx.mInstance.mUiMgr.getForm(UIFormId.eUILogin) as UILogin;
            if(null != uiLogin)
            {
                uiLogin.err(s);
            }

            UISelectRole uiSelectRole = Ctx.mInstance.mUiMgr.getForm(UIFormId.eUISelectRole) as UISelectRole;
            if(null != uiSelectRole)
            {
                uiSelectRole.err(s);
            }*/
            Ctx.mInstance.mLuaSystem.openForm((int)UIFormId.eUIStartGame_Lua);
        }

        public void info(string s)
        {
            labelColor = Color.green;
            labelMsg = s;

            /*UILogin uiLogin = Ctx.mInstance.mUiMgr.getForm(UIFormId.eUILogin) as UILogin;
            if (null != uiLogin)
            {
                uiLogin.info(s);
            }

            UISelectRole uiSelectRole = Ctx.mInstance.mUiMgr.getForm(UIFormId.eUISelectRole) as UISelectRole;
            if (null != uiSelectRole)
            {
                uiSelectRole.info(s);
            }*/
        }

        public void login()
        {
            if (UtilApi.getFloatUTCSec() - lastReqLogin < 1.5f)
            {
                //发送过快
                return;
            }

            isrelogin = false;
            info("connect to server...(连接到服务端...)");
            KBEngine.Event.fireIn("login", stringAccount, stringPasswd, System.Text.Encoding.UTF8.GetBytes("kbengine_unity3d_demo"));
            lastReqLogin = UtilApi.getFloatUTCSec();
        }

        public void relogin()
        {
            if (UtilApi.getFloatUTCSec() - lastReqLogin < 1.5f)
            {
                //发送过快
                return;
            }

            isrelogin = true;            
            info("connect to server...（重新连接到服务端...)");
            KBEngine.Event.fireIn("login", stringAccount, stringPasswd, System.Text.Encoding.UTF8.GetBytes("kbengine_unity3d_demo"));
            //KBEngineApp.app.login(stringAccount, stringPasswd, System.Text.Encoding.UTF8.GetBytes("kbengine_unity3d_demo"));
            //Ctx.mInstance.mSceneEventCB.onLevelLoaded();
            lastReqLogin = UtilApi.getFloatUTCSec();
        }

        public void closeNetwork()
        {
            Ctx.mInstance.mUiMgr.exitForm(UIFormId.eUIJoyStick);
            Ctx.mInstance.mUiMgr.exitForm(UIFormId.eUIForwardForce);
            if (Ctx.mInstance.mSystemSetting.hasKey("MusicModel"))
            {
                if (Ctx.mInstance.mSystemSetting.getInt("MusicModel") == 1)
                {
                    Ctx.mInstance.mSoundMgr.stop("Sound/Music/StudioEIM-myseabed.mp3");
                }
            }
            else
            {
                Ctx.mInstance.mSoundMgr.stop("Sound/Music/StudioEIM-myseabed.mp3");
            }

            KBEngineApp.app.networkInterface().close();
            lastReqLogin = UtilApi.getFloatUTCSec();
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
            /*if (failedcode == 20)
            {
                err("login is failed(登陆失败), err=" + KBEngineApp.app.serverErr(failedcode) + ", " + System.Text.Encoding.UTF8.GetString(KBEngineApp.app.serverdatas()));
            }
            else
            {
                err("login is failed(登陆失败), err=" + KBEngineApp.app.serverErr(failedcode));
            }

            if(23 == failedcode)//账号不存在
            {
                autoCreateAccount();
            }*/
        }

        private void autoCreateAccount()
        {
            string nickname = SDK.Lib.Ctx.mInstance.mSystemSetting.getString(SDK.Lib.SystemSetting.USERNAME);
            string password = "111111";
            KBEngine.Event.fireIn("createAccount", nickname, password, System.Text.Encoding.UTF8.GetBytes("kbengine_unity3d_demo"));
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
            //Ctx.mInstance.mLuaSystem.exitForm((int)UIFormID.eUIStartGame);
            //Ctx.mInstance.mUiMgr.loadAndShow(UIFormID.eUISelectRole);

            ui_state = 1;
            //Application.LoadLevel("selavatars");
        }

        public void onKicked(UInt16 failedcode)
        {
            Ctx.mInstance.mPlayerMgr.dispose();
            Ctx.mInstance.mSnowBlockMgr.dispose();
            Ctx.mInstance.mComputerBallMgr.dispose();
            Ctx.mInstance.mPlayerSnowBlockMgr.dispose();
            err("kick, disconnect!, reason=" + KBEngineApp.app.serverErr(failedcode));
            //Application.LoadLevel("login");

            ui_state = 0;
            //Ctx.mInstance.mUiMgr.loadAndShow(UIFormId.eUILogin);
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

            /*UISelectRole uiSelectRole = Ctx.mInstance.mUiMgr.getForm(UIFormId.eUISelectRole) as UISelectRole;
            if (null != uiSelectRole)
            {
                uiSelectRole.setAvatarList(ui_avatarList);
            }*/
            Ctx.mInstance.mLuaSystem.openForm((int)UIFormId.eUIStartGame_Lua);
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