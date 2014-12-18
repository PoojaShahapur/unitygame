using Game.Msg;
using SDK.Common;

namespace Game.Login
{
    /**
     * @brief 登陆网络处理
     */
    public class LoginNetHandleCB : INetHandle
    {
        public void handleMsg(IByteArray msg)
        {
            byte byCmd = msg.readByte();
            byte byParam = msg.readByte();
            msg.setPos(0);

            switch (byCmd)        // 登陆消息
            {
                case stNullUserCmd.LOGON_USERCMD:
                {
                    switch(byParam)
                    {
                        case stLogonUserCmd.RETURN_CLIENT_IP_PARA:
                        {
                            LoginSys.m_instance.m_loginFlowHandle.receiveMsg2f(msg);
                            break;
                        }
                        case stLogonUserCmd.SERVER_RETURN_LOGIN_OK:
                        {
                            LoginSys.m_instance.m_loginFlowHandle.receiveMsg4f(msg);
                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                    break;
                }
                case stNullUserCmd.DATA_USERCMD:
                {
                    switch (byParam)
                    {
                        case stDataUserCmd.MERGE_VERSION_CHECK_USERCMD_PARA:
                        {
                            LoginSys.m_instance.m_loginFlowHandle.receiveMsg6f(msg);
                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                    break;
                }
                case stNullUserCmd.TIME_USERCMD:
                {
                    switch (byParam)
                    {
                        case stTimerUserCmd.GAMETIME_TIMER_USERCMD_PARA:
                        {
                            LoginSys.m_instance.m_loginFlowHandle.receiveMsg7f(msg);
                            break;
                        }
                        case stTimerUserCmd.REQUESTUSERGAMETIME_TIMER_USERCMD_PARA:
                        {
                            LoginSys.m_instance.m_loginFlowHandle.receiveMsg8f(msg);
                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
    }
}