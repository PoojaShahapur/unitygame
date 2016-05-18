using SDK.Lib;

namespace Game.Login
{
    /**
     * @brief 登陆网络处理
     */
    public class LoginNetHandleCB : NetDispHandle
    {
        public LoginNetHandleCB()
        {
            this.addCmdHandle(stNullUserCmd.TIME_USERCMD, new LoginTimerUserCmdHandle());
            this.addCmdHandle(stNullUserCmd.DATA_USERCMD, new LoginDataUserCmdHandle());
            this.addCmdHandle(stNullUserCmd.SELECT_USERCMD, new LoginSelectUserCmdHandle());
            this.addCmdHandle(stNullUserCmd.LOGON_USERCMD, new LoginLogonUserCmdHandle());
        }

        // 如果要调试可以重载，方便调试
        //public override void handleMsg(ByteBuffer msg)
        //{

        //}
    }
}