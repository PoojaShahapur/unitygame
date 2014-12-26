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
            m_id2DispDic[stNullUserCmd.TIME_USERCMD] = new TimerUserCmdHandle();
            m_id2DispDic[stNullUserCmd.LOGON_USERCMD] = new DataUserCmdHandle();
            m_id2DispDic[stNullUserCmd.SELECT_USERCMD] = new SelectUserCmdHandle();
            m_id2DispDic[stNullUserCmd.LOGON_USERCMD] = new LogonUserCmdHandle();
            m_id2DispDic[stNullUserCmd.PROPERTY_USERCMD] = new PropertyUserCmdHandle();
        }

        // 如果要调试可以重载，方便调试
        //public override void handleMsg(IByteArray msg)
        //{

        //}
    }
}