using Game.Msg;
using SDK.Common;

namespace Game.Login
{
    /**
     * @brief 登陆网络处理
     */
    public class LoginNetHandleCB : NetDispHandle
    {
        public LoginNetHandleCB()
        {
            m_id2DispDic[stNullUserCmd.TIME_USERCMD] = new LoginTimerUserCmdHandle();
            m_id2DispDic[stNullUserCmd.DATA_USERCMD] = new LoginDataUserCmdHandle();
            m_id2DispDic[stNullUserCmd.SELECT_USERCMD] = new LoginSelectUserCmdHandle();
            m_id2DispDic[stNullUserCmd.LOGON_USERCMD] = new LoginLogonUserCmdHandle();
        }

        // 如果要调试可以重载，方便调试
        //public override void handleMsg(IByteArray msg)
        //{

        //}
    }
}