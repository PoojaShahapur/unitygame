using SDK.Lib;

namespace Game.Login
{
    /**
     * @brief 登陆网络处理
     */
    public class LoginNetHandleCB : NetModuleDispHandle
    {
        private string ip = "192.168.93.187";
        private int port = 8002;

        public LoginNetHandleCB()
        {
            NetCmdDispHandle cmdHandle = null;
            cmdHandle = new LoginTimerUserCmdHandle();
            this.addCmdHandle(stNullUserCmd.TIME_USERCMD, cmdHandle, cmdHandle.call);
            cmdHandle = new LoginDataUserCmdHandle();
            this.addCmdHandle(stNullUserCmd.DATA_USERCMD, cmdHandle, cmdHandle.call);
            cmdHandle = new LoginSelectUserCmdHandle();
            this.addCmdHandle(stNullUserCmd.SELECT_USERCMD, cmdHandle, cmdHandle.call);
            cmdHandle = new LoginLogonUserCmdHandle();
            this.addCmdHandle(stNullUserCmd.LOGON_USERCMD, cmdHandle, cmdHandle.call);
        }

        // 如果要调试可以重载，方便调试
        //public override void handleMsg(ByteBuffer msg)
        //{

        //}

        public void OnLogin(string method, rpc.LoginResponse resp)
        {
            if (resp.result == rpc.LoginResponse.LoginResult.OK)
            {
                //_saveConfig();
                Ctx.mInstance.mServerHandler_GB.RequestSend("plane.Plane", "EnterRoom", new rpc.EmptyMsg());
            }
            else
            {
                switch (resp.result)
                {
                    case rpc.LoginResponse.LoginResult.ERR_MULTI_LOGIN:
                        //ShowMessage("该账号已经被其他玩家登陆!");
                        break;
                    case rpc.LoginResponse.LoginResult.ERR_SERVER_FULL:
                        //ShowMessage("服务器满员!");
                        break;
                    case rpc.LoginResponse.LoginResult.ERR_VERIFY_FAIL:
                        //ShowMessage("密码错误!");
                        break;
                }
            }
        }

        public void OnEnterRoom(plane.EnterRoomResponse response)
        {
            Ctx.mInstance.mDataPlayer.mDataHero.setRoomInfo(response);

            Ctx.mInstance.mUiMgr.exitForm(SDK.Lib.UIFormId.eUILogin);
            Ctx.mInstance.mUiMgr.exitForm(SDK.Lib.UIFormId.eUISelectRole);

            Ctx.mInstance.mModuleSys.unloadModule(ModuleId.LOGINMN);
            bool relogin = false;
            if (relogin)
                Ctx.mInstance.mSceneEventCB.onLevelLoaded();
            else
                Ctx.mInstance.mModuleSys.loadModule(SDK.Lib.ModuleId.GAMEMN);
        }

        public void setServerIP()
        {
            if (SDK.Lib.Ctx.mInstance.mSystemSetting.hasKey("ServerAddr"))
            {
                if (SDK.Lib.Ctx.mInstance.mSystemSetting.getInt("ServerAddr") == 1)
                {
                    this.ip = "192.168.96.15";
                    this.port = 8002;
                }
                else if (SDK.Lib.Ctx.mInstance.mSystemSetting.getInt("ServerAddr") == 2)
                {
                    this.ip = "192.168.93.187";
                    this.port = 8002;
                }
                else
                {
                    this.ip = SDK.Lib.Ctx.mInstance.mSystemSetting.getString("ip");
                    this.port = SDK.Lib.Ctx.mInstance.mSystemSetting.getInt("port");
                }
            }
            else
            {
                this.ip = "192.168.96.15";
                this.port = 8002;
            }
        }

        private void _connectedToServer(System.Action onConnected = null)
        {
            Ctx.mInstance.mLightServer_GB.Connect(ip, port, Ctx.mInstance.mServerHandler_GB);
            Ctx.mInstance.mServerHandler_GB.OnConnectHandler = onConnected;
        }

        public void _doLogin()
        {
            var cmd = new rpc.LoginRequest();
            cmd.account = Ctx.mInstance.mSystemSetting.getString(SystemSetting.NICKNAME);
            cmd.password = "1111111";
            Ctx.mInstance.mProxy_GB.Call("Login", cmd);
        }

        public void login()
        {
            if (Ctx.mInstance.mLightServer_GB.Connected)
            {
                this._doLogin();
            }
            else
            {
                this._connectedToServer(_doLogin);
            }
        }

        public void relogin()
        {

        }
    }
}