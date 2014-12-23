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
        protected uint m_loginTempID;

        public LoginFlowHandle()
        {
            Ctx.m_instance.m_sysMsgRoute.m_socketOpenedCB += onLoginServerSocketOpened;
        }

        public void connectLoginServer()
        {
            Ctx.m_instance.m_log.log("开始连接登陆服务器");
            // 连接服务器
            Ctx.m_instance.m_netMgr.openSocket(Ctx.m_instance.m_cfg.m_ip, Ctx.m_instance.m_cfg.m_port);
        }

        // socket 打开
        protected void onLoginServerSocketOpened()
        {
            Ctx.m_instance.m_log.log("连接登陆服务器成功");
            Ctx.m_instance.m_sysMsgRoute.m_socketOpenedCB -= onLoginServerSocketOpened;
            sendMsg1f();
        }

        // 登陆登录服务器
        // 步骤 1 ，发送登陆消息
        public void sendMsg1f()
        {
            Ctx.m_instance.m_log.log("发送到登陆服务器第一条消息");

            stUserVerifyVerCmd cmdVerify = new stUserVerifyVerCmd();
            UtilMsg.sendMsg(cmdVerify);

            stRequestClientIP cmdReqIP = new stRequestClientIP();
            UtilMsg.sendMsg(cmdReqIP);
        }

        // 步骤 2 ，接收返回的消息
        public void receiveMsg2f(IByteArray msg)
        {
            Ctx.m_instance.m_log.log("接收到登陆服务器发送回来的第一条消息");
            stReturnClientIP cmd = new stReturnClientIP();
            cmd.derialize(msg);
            Ctx.m_instance.m_log.log(cmd.pstrIP);

            sendMsg3f();
        }

        // 步骤 3 ，发送消息
        public void sendMsg3f()
        {
            // 测试数据
            //send.game = 10;
            //send.zone = 30;
            //zhanghao01---zhanghao09
            Ctx.m_instance.m_log.log("发送登录服务器消息 stUserRequestLoginCmd");
            stUserRequestLoginCmd cmd = new stUserRequestLoginCmd();
            cmd.pstrName = "zhanghao01";
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
            m_gatePort = cmd.wdPort;

            m_dwUserID = cmd.dwUserID;
            m_loginTempID = cmd.loginTempID;

            string str = string.Format("网关信息:  网关IP: {0}, 网关端口: {1}, 用户ID: {2}, 用户临时 ID: {3}", m_gateIP, m_gatePort, m_dwUserID, m_loginTempID);
            Ctx.m_instance.m_log.log(str);

            Ctx.m_instance.m_netMgr.closeSocket(Ctx.m_instance.m_cfg.m_ip, Ctx.m_instance.m_cfg.m_port);            // 关闭之前的 socket
            connectGateServer();
        }

        // 登陆网关服务器
        public void connectGateServer()
        {
            Ctx.m_instance.m_log.log("开始连接网关");
            Ctx.m_instance.m_sysMsgRoute.m_socketOpenedCB += onGateServerSocketOpened;
            Ctx.m_instance.m_netMgr.openSocket(m_gateIP, m_gatePort);
        }

        protected void onGateServerSocketOpened()
        {
            Ctx.m_instance.m_log.log("连接网关成功");
            Ctx.m_instance.m_sysMsgRoute.m_socketOpenedCB -= onGateServerSocketOpened;
            sendMsg5f();
        }
        
        // 登陆网关服务器
        // 步骤 5 ，发送消息
        public void sendMsg5f()
        {
            Ctx.m_instance.m_log.log("发送到网关第一条消息");
            stUserVerifyVerCmd cmdVerify = new stUserVerifyVerCmd();
            UtilMsg.sendMsg(cmdVerify);

            stPasswdLogonUserCmd cmd = new stPasswdLogonUserCmd();
            cmd.dwUserID = m_dwUserID;
            cmd.loginTempID = m_loginTempID;
            UtilMsg.sendMsg(cmd);
        }

        // 步骤 6 ，接收消息
        public void receiveMsg6f(IByteArray msg)
        {
            Ctx.m_instance.m_log.log("接收到网关发送回来的第一条消息");
            stMergeVersionCheckUserCmd cmd = new stMergeVersionCheckUserCmd();
            cmd.derialize(msg);
        }

        // 步骤 7 ，接收消息
        public void receiveMsg7f(IByteArray msg)
        {
            Ctx.m_instance.m_log.log("接收消息 stGameTimeTimerUserCmd");
            stGameTimeTimerUserCmd cmd = new stGameTimeTimerUserCmd();
            cmd.derialize(msg);
        }

        // 步骤 8 ，接收消息
        public void receiveMsg8f(IByteArray msg)
        {
            Ctx.m_instance.m_log.log("接收消息 stRequestUserGameTimeTimerUserCmd");
            stRequestUserGameTimeTimerUserCmd cmd = new stRequestUserGameTimeTimerUserCmd();
            cmd.derialize(msg);

            sendMsg9f();
        }

        // 步骤 9 ，发送消息
        public void sendMsg9f()
        {
            Ctx.m_instance.m_log.log("发送消息 stUserGameTimeTimerUserCmd");
            stUserGameTimeTimerUserCmd cmd = new stUserGameTimeTimerUserCmd();
            UtilMsg.sendMsg(cmd);

            // 加载游戏模块
            //Ctx.m_instance.m_moduleSys.loadModule(ModuleName.GAMEMN);
        }
    }
}