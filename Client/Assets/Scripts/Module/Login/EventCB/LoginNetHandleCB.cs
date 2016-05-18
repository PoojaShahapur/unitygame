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
            NetCmdHandleBase cmdHandle;
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
    }
}