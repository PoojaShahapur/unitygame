using Game.Msg;
using SDK.Lib;

namespace Game.Login
{
    /**
     * @brief 登陆流程处理
     */
    public class LoginFlowHandle
    {
        protected string mGateIP;
        protected ushort mGatePort;

        protected uint mDwUserID;

        public string mName;
        public string mPassword;
        protected byte[] m_cryptKey;
        protected MAction4<string, int, bool, object> mGateWayAction;
        protected SelectEnterMode mSelectEnterMode;

        public LoginFlowHandle()
        {
            
        }

        public uint getDwUserID()
        {
            return mDwUserID;
        }

        public void connectLoginServer(string name, string passwd, SelectEnterMode selectEnterMode)
        {
            Ctx.mInstance.mLoginSys.setLoginState(LoginState.eLoginingLoginServer);     // 设置登陆状态

            this.mName = name;
            this.mPassword = passwd;
            this.mSelectEnterMode = selectEnterMode;
            Ctx.mInstance.mDataPlayer.m_accountData.m_account = name;
            Ctx.mInstance.mLogSys.registerFileLogDevice();

            Ctx.mInstance.mLogSys.log(Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem0));
            // 连接 web 服务器
            //Ctx.mInstance.m_pWebSocketMgr.openSocket(Ctx.mInstance.mCfg.m_webIP, Ctx.mInstance.mCfg.m_webPort);
            // 连接游戏服务器
            Ctx.mInstance.mNetMgr.openSocket(Ctx.mInstance.mCfg.mIp, Ctx.mInstance.mCfg.mPort);
        }

        // socket 打开
        public void onLoginServerSocketOpened()
        {
            Ctx.mInstance.mLoginSys.setLoginState(LoginState.eLoginSuccessLoginServer);     // 设置登陆状态
            Ctx.mInstance.mLogSys.log(Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem1));

            this.sendMsg1f();
        }

        // 登陆登录服务器
        // 步骤 1 ，发送登陆消息
        public void sendMsg1f()
        {
            if (SelectEnterMode.eLoginAccount == mSelectEnterMode)
            {
                KBEngine.Event.fireIn("login", this.mName, this.mPassword, System.Text.Encoding.UTF8.GetBytes("kbengine_unity3d_demo"));
            }
            else
            {
                KBEngine.Event.fireIn("createAccount", this.mName, this.mPassword, System.Text.Encoding.UTF8.GetBytes("kbengine_unity3d_demo"));
            }
        }

        // 步骤 2 ，接收返回的消息
        public void receiveMsg2f(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;
            Ctx.mInstance.mLogSys.log(Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem3));
            stReturnClientIP cmd = new stReturnClientIP();
            cmd.derialize(msg);

            cmd.pstrIP = cmd.pstrIP.TrimEnd('\0');
            string str = string.Format(Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem15), cmd.pstrIP, cmd.port);
            Ctx.mInstance.mLogSys.log(str);

            sendMsg3f();
        }

        // 步骤 3 ，发送消息
        public void sendMsg3f()
        {
            Ctx.mInstance.mLogSys.log(Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem4));

            stUserRequestLoginCmd cmd = new stUserRequestLoginCmd();
            cmd.pstrName = this.mName;
            cmd.pstrPassword = this.mPassword;
            cmd.game = 10;
            cmd.zone = Ctx.mInstance.mCfg.mZone;
            UtilMsg.sendMsg(cmd);
        }

        // 步骤 4 ，服务器返回消息
        public void receiveMsg4f(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stServerReturnLoginSuccessCmd cmd = new stServerReturnLoginSuccessCmd();
            cmd.derialize(msg);

            // 登陆成功开始加密解密数据包，在后面的消息里面设置
            m_cryptKey = cmd.key;
            Ctx.mInstance.mLogSys.log(string.Format("crypt key: {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", m_cryptKey[0], m_cryptKey[1], m_cryptKey[2], m_cryptKey[3], m_cryptKey[4], m_cryptKey[5], m_cryptKey[6], m_cryptKey[7]));

            this.mGateIP = cmd.pstrIP;
            this.mGateIP = this.mGateIP.TrimEnd('\0');     // 剔除结尾 '\0' 字符
            this.mGatePort = cmd.wdPort;

            this.mDwUserID = cmd.dwUserID;
            Ctx.mInstance.mTimerMsgHandle.m_loginTempID = cmd.loginTempID;
            Ctx.mInstance.mDataPlayer.m_dataMain.m_dwUserTempID = cmd.loginTempID;

            string str = string.Format(Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem5), this.mGateIP, this.mGatePort, this.mDwUserID, Ctx.mInstance.mTimerMsgHandle.m_loginTempID);
            Ctx.mInstance.mLogSys.log(str);

            Ctx.mInstance.mNetMgr.closeSocket(Ctx.mInstance.mCfg.mIp, Ctx.mInstance.mCfg.mPort);            // 关闭之前的 socket
            connectGateServer();
        }

        // 登陆网关服务器
        public void connectGateServer()
        {
            Ctx.mInstance.mLoginSys.setLoginState(LoginState.eLoginingGateServer);     // 设置登陆状态
            Ctx.mInstance.mLogSys.log(Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem6));
            Ctx.mInstance.mNetMgr.openSocket(this.mGateIP, this.mGatePort);
        }

        public void connectGateServer_KBE(string ip, ushort port, MAction4<string, int, bool, object> action)
        {
            Ctx.mInstance.mNetMgr.closeSocket(Ctx.mInstance.mCfg.mIp, Ctx.mInstance.mCfg.mPort);            // 关闭之前的 socket

            this.mGateIP = ip;
            this.mGatePort = port;
            this.mGateWayAction = action;

            Ctx.mInstance.mLoginSys.setLoginState(LoginState.eLoginingGateServer);     // 设置登陆状态
            Ctx.mInstance.mLogSys.log(Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem6));
            Ctx.mInstance.mNetMgr.openSocket(this.mGateIP, this.mGatePort);
        }

        public void onGateServerSocketOpened()
        {
            Ctx.mInstance.mLoginSys.setLoginState(LoginState.eLoginSuccessGateServer);     // 设置登陆状态

            // 登陆成功开始加密解密数据包
            if (MacroDef.MSG_ENCRIPT)
            {
                Ctx.mInstance.mNetMgr.setCryptKey(m_cryptKey);
            }
            Ctx.mInstance.mLogSys.log(Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem7));
            this.sendMsg5f();
        }
        
        // 登陆网关服务器
        // 步骤 5 ，发送消息
        public void sendMsg5f()
        {
            this.mGateWayAction(this.mGateIP, this.mGatePort, true, null);
        }

        // 步骤 6 ，接收消息
        public void receiveMsg6f(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            Ctx.mInstance.mLogSys.log(Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem9));

            stMergeVersionCheckUserCmd cmd = new stMergeVersionCheckUserCmd();
            cmd.derialize(msg);
        }

        // 收到这条消息，就说明客户端没有创建角色，弹出创建角色界面
        public void psstServerReturnLoginFailedCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stServerReturnLoginFailedCmd cmd = new stServerReturnLoginFailedCmd();
            cmd.derialize(msg);

            // 弹出创建角色界面
            if((byte)ERetResult.LOGIN_RETURN_USERDATANOEXIST == cmd.byReturnCode)           // 没有角色也是从这里建立角色的，其实这个不是个错误
            {

            }
            else if((byte)ERetResult.LOGIN_RETURN_IDINUSE == cmd.byReturnCode)              // 账号在使用
            {
                Ctx.mInstance.mLogSys.log(Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem13));
                // 重新登陆
                Ctx.mInstance.mLoginSys.setLoginState(LoginState.eLoginInfoError);
            }
            else if ((byte)ERetResult.LOGIN_RETURN_PASSWORDERROR == cmd.byReturnCode)   // 用户名或者密码错误
            {
                Ctx.mInstance.mLoginSys.setLoginState(LoginState.eLoginInfoError);
                InfoBoxParam param = Ctx.mInstance.mPoolSys.newObject<InfoBoxParam>();
                param.m_midDesc = Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem16);
            }
            else if ((byte)ERetResult.LOGIN_RETURN_VERSIONERROR == cmd.byReturnCode)        // 版本错误，重新登陆
            {
                Ctx.mInstance.mLoginSys.setLoginState(LoginState.eLoginInfoError);
                InfoBoxParam param = Ctx.mInstance.mPoolSys.newObject<InfoBoxParam>();
                param.m_midDesc = Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem17);
            }
            else if ((byte)ERetResult.LOGIN_RETURN_CHARNAMEREPEAT == cmd.byReturnCode)       // 建立角色名字重复
            {
                Ctx.mInstance.mLoginSys.setLoginState(LoginState.eLoginNewCharError);
                Ctx.mInstance.mLogSys.log(Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem14));
                InfoBoxParam param = Ctx.mInstance.mPoolSys.newObject<InfoBoxParam>();
                param.m_midDesc = Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem14);
            }
            else if((byte)ERetResult.LOGIN_RETURN_CHARNAME_FORBID == cmd.byReturnCode)  // 用户名字不符合要求
            {
                Ctx.mInstance.mLoginSys.setLoginState(LoginState.eLoginNewCharError);
                InfoBoxParam param = Ctx.mInstance.mPoolSys.newObject<InfoBoxParam>();
                param.m_midDesc = Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem19);
            }
            else if((byte)ERetResult.LOGIN_RETURN_CHARNAME_FORBID == cmd.byReturnCode)  // 用户满，从登陆服务器开始登陆
            {
                Ctx.mInstance.mLoginSys.setLoginState(LoginState.eLoginFailedGateServer);
                InfoBoxParam param = Ctx.mInstance.mPoolSys.newObject<InfoBoxParam>();
                param.m_midDesc = Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem20);
            }
            else if((byte)ERetResult.LOGIN_RETURN_CHARNAME_FORBID == cmd.byReturnCode)  // 网关未开，这个不用登了，服务器就没有启动
            {
                Ctx.mInstance.mLoginSys.setLoginState(LoginState.eLoginFailedGateServer);
                InfoBoxParam param = Ctx.mInstance.mPoolSys.newObject<InfoBoxParam>();
                param.m_midDesc = Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem21);
            }
            else if((byte)ERetResult.LOGIN_RETURN_USERMAX == cmd.byReturnCode)
            {
                Ctx.mInstance.mLoginSys.setLoginState(LoginState.eLoginFailedGateServer);
                InfoBoxParam param = Ctx.mInstance.mPoolSys.newObject<InfoBoxParam>();
                param.m_midDesc = Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem23);
            }
            else
            {
                Ctx.mInstance.mLogSys.log(Ctx.mInstance.mLangMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem18));
                // 重新登陆
                Ctx.mInstance.mLoginSys.setLoginState(LoginState.eLoginInfoError);
            }
        }

        // 返回基本角色信息
        public void psstUserInfoUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer byteBuffer = dispObj as ByteBuffer;
            // 发送选择角色登陆进入游戏
            stLoginSelectUserCmd cmd1f = new stLoginSelectUserCmd();
            cmd1f.charNo = 0;
            UtilMsg.sendMsg(cmd1f);

            stUserInfoUserCmd cmd = new stUserInfoUserCmd();
            cmd.derialize(byteBuffer);
        }

        // 终于登陆成功了
        public void psstLoginSelectSuccessUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer byteBuffer = dispObj as ByteBuffer;

            stLoginSelectSuccessUserCmd cmd = new stLoginSelectSuccessUserCmd();
            cmd.derialize(byteBuffer);

            Ctx.mInstance.mNetCmdNotify.isStopNetHandle = true;     // 停止网络消息处理
            // 进入场景
            Ctx.mInstance.mModuleSys.loadModule(ModuleID.GAMEMN);
        }
    }
}