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
                        case stLogonUserCmd.SERVER_RETURN_LOGIN_FAILED:         // 如果没有角色，服务器就会返回这条消息，弹出创建角色界面
                        {
                            LoginSys.m_instance.m_loginFlowHandle.psstServerReturnLoginFailedCmd(msg);
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
                case stNullUserCmd.SELECT_USERCMD:
                {
                    switch (byParam)
                    {
                        case stSelectUserCmd.USERINFO_SELECT_USERCMD_PARA:
                            {
                                LoginSys.m_instance.m_loginFlowHandle.psstUserInfoUserCmd(msg);
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