using Game.App;
using Game.Msg;
using SDK.Common;

namespace Game.Login
{
    /**
     * @brief 登陆流程处理
     */
    public class LoginFlowHandle : ILoginFlowHandle
    {
        protected string m_gateIP;
        protected ushort m_gatePort;

        protected uint m_dwUserID;

        protected string m_name;
        protected string m_passwd;

        public LoginFlowHandle()
        {
            Ctx.m_instance.m_sysMsgRoute.m_socketOpenedCB += onLoginServerSocketOpened;
        }

        public void connectLoginServer(string name, string passwd)
        {
            m_name = name;
            m_passwd = passwd;

            Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eLLog0f);
            Ctx.m_instance.m_log.log(Ctx.m_instance.m_shareMgr.m_retLangStr);
            // 连接 web 服务器
            //Ctx.m_instance.m_pWebSocketMgr.openSocket(Ctx.m_instance.m_cfg.m_webIP, Ctx.m_instance.m_cfg.m_webPort);
            // 连接游戏服务器
            Ctx.m_instance.m_netMgr.openSocket(Ctx.m_instance.m_cfg.m_ip, Ctx.m_instance.m_cfg.m_port);
        }

        // socket 打开
        protected void onLoginServerSocketOpened()
        {
            Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eLLog1f);
            Ctx.m_instance.m_log.log(Ctx.m_instance.m_shareMgr.m_retLangStr);
            Ctx.m_instance.m_sysMsgRoute.m_socketOpenedCB -= onLoginServerSocketOpened;
            sendMsg1f();
        }

        // 登陆登录服务器
        // 步骤 1 ，发送登陆消息
        public void sendMsg1f()
        {
            Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eLLog2f);
            Ctx.m_instance.m_log.log(Ctx.m_instance.m_shareMgr.m_retLangStr);

            stUserVerifyVerCmd cmdVerify = new stUserVerifyVerCmd();
            UtilMsg.sendMsg(cmdVerify);

            stRequestClientIP cmdReqIP = new stRequestClientIP();
            UtilMsg.sendMsg(cmdReqIP);
        }

        // 步骤 2 ，接收返回的消息
        public void receiveMsg2f(IByteArray msg)
        {
            Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eLLog3f);
            Ctx.m_instance.m_log.log(Ctx.m_instance.m_shareMgr.m_retLangStr);
            stReturnClientIP cmd = new stReturnClientIP();
            cmd.derialize(msg);

            Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eLLog15f);
            cmd.pstrIP = cmd.pstrIP.TrimEnd('\0');
            string str = string.Format(Ctx.m_instance.m_shareMgr.m_retLangStr, cmd.pstrIP, cmd.port);
            Ctx.m_instance.m_log.log(str);

            sendMsg3f();
        }

        // 步骤 3 ，发送消息
        public void sendMsg3f()
        {
            // 测试数据
            //send.game = 10;
            //send.zone = 30;
            //zhanghao01---zhanghao09
            Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eLLog4f);
            Ctx.m_instance.m_log.log(Ctx.m_instance.m_shareMgr.m_retLangStr);

            stUserRequestLoginCmd cmd = new stUserRequestLoginCmd();
            //cmd.pstrName = "zhanghao01";
            //cmd.pstrPassword = "1";
            cmd.pstrName = m_name;
            cmd.pstrPassword = m_passwd;
            cmd.game = 10;
            cmd.zone = 30;
            UtilMsg.sendMsg(cmd);
        }

        // 步骤 4 ，服务器返回消息
        public void receiveMsg4f(IByteArray msg)
        {
            stServerReturnLoginSuccessCmd cmd = new stServerReturnLoginSuccessCmd();
            cmd.derialize(msg);

            m_gateIP = cmd.pstrIP;
            m_gateIP = m_gateIP.TrimEnd('\0');     // 剔除结尾 '\0' 字符
            m_gatePort = cmd.wdPort;

            m_dwUserID = cmd.dwUserID;
            Ctx.m_instance.m_pTimerMsgHandle.m_loginTempID = cmd.loginTempID;
            Ctx.m_instance.m_dataPlayer.m_dataMain.m_dwUserTempID = cmd.loginTempID;

            Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eLLog5f);
            string str = string.Format(Ctx.m_instance.m_shareMgr.m_retLangStr, m_gateIP, m_gatePort, m_dwUserID, Ctx.m_instance.m_pTimerMsgHandle.m_loginTempID);
            Ctx.m_instance.m_log.log(str);

            Ctx.m_instance.m_netMgr.closeSocket(Ctx.m_instance.m_cfg.m_ip, Ctx.m_instance.m_cfg.m_port);            // 关闭之前的 socket
            connectGateServer();
        }

        // 登陆网关服务器
        public void connectGateServer()
        {
            Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eLLog6f);
            Ctx.m_instance.m_log.log(Ctx.m_instance.m_shareMgr.m_retLangStr);

            Ctx.m_instance.m_sysMsgRoute.m_socketOpenedCB += onGateServerSocketOpened;
            Ctx.m_instance.m_netMgr.openSocket(m_gateIP, m_gatePort);
        }

        protected void onGateServerSocketOpened()
        {
            Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eLLog7f);
            Ctx.m_instance.m_log.log(Ctx.m_instance.m_shareMgr.m_retLangStr);
            Ctx.m_instance.m_sysMsgRoute.m_socketOpenedCB -= onGateServerSocketOpened;
            sendMsg5f();
        }
        
        // 登陆网关服务器
        // 步骤 5 ，发送消息
        public void sendMsg5f()
        {
            Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eLLog8f);
            Ctx.m_instance.m_log.log(Ctx.m_instance.m_shareMgr.m_retLangStr);

            stUserVerifyVerCmd cmdVerify = new stUserVerifyVerCmd();
            UtilMsg.sendMsg(cmdVerify);

            stPasswdLogonUserCmd cmd = new stPasswdLogonUserCmd();
            cmd.dwUserID = m_dwUserID;
            cmd.loginTempID = Ctx.m_instance.m_pTimerMsgHandle.m_loginTempID;
            UtilMsg.sendMsg(cmd);
        }

        // 步骤 6 ，接收消息
        public void receiveMsg6f(IByteArray msg)
        {
            Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eLLog9f);
            Ctx.m_instance.m_log.log(Ctx.m_instance.m_shareMgr.m_retLangStr);

            stMergeVersionCheckUserCmd cmd = new stMergeVersionCheckUserCmd();
            cmd.derialize(msg);
        }

        // 收到这条消息，就说明客户端没有创建角色，弹出创建角色界面
        public void psstServerReturnLoginFailedCmd(IByteArray msg)
        {
            stServerReturnLoginFailedCmd cmd = new stServerReturnLoginFailedCmd();
            cmd.derialize(msg);

            // 关闭登陆界面
            Ctx.m_instance.m_uiMgr.exitForm(UIFormID.UILogin);

            // 弹出创建角色界面
            if((byte)ERetResult.LOGIN_RETURN_USERDATANOEXIST == cmd.byReturnCode)
            {
                Ctx.m_instance.m_uiMgr.loadForm(UIFormID.UIHeroSelect);
            }
            else if((byte)ERetResult.LOGIN_RETURN_IDINUSE == cmd.byReturnCode)
            {
                Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eLLog13f);
                Ctx.m_instance.m_log.log(Ctx.m_instance.m_shareMgr.m_retLangStr);
            }
            else if((byte)ERetResult.LOGIN_RETURN_CHARNAMEREPEAT == cmd.byReturnCode)
            {
                Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eLLog14f);
                Ctx.m_instance.m_log.log(Ctx.m_instance.m_shareMgr.m_retLangStr);
            }
            else
            {
                Ctx.m_instance.m_log.log("Login Exception Error");
            }
        }

        // 返回基本角色信息
        public void psstUserInfoUserCmd(IByteArray ba)
        {
            // 发送选择角色登陆进入游戏
            stLoginSelectUserCmd cmd1f = new stLoginSelectUserCmd();
            cmd1f.charNo = 0;
            UtilMsg.sendMsg(cmd1f);

            stUserInfoUserCmd cmd = new stUserInfoUserCmd();
            cmd.derialize(ba);
        }

        // 终于登陆成功了
        public void psstLoginSelectSuccessUserCmd(IByteArray ba)
        {
            stLoginSelectSuccessUserCmd cmd = new stLoginSelectSuccessUserCmd();
            cmd.derialize(ba);

            Ctx.m_instance.m_bStopNetHandle = true;     // 停止网络消息处理
            // 进入场景
            Ctx.m_instance.m_moduleSys.loadModule(ModuleID.GAMEMN);
        }
    }
}