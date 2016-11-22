using SDK.Lib;

namespace Game.Login
{
    /**
     * @brief KBEngine 登陆网络处理
     */
    public class KBELoginNetHandleCB : NetModuleDispHandle
    {
        public KBELoginNetHandleCB()
        {
            NetCmdDispHandle cmdHandle = null;
            cmdHandle = new LoginTimerUserCmdHandle();
            this.addCmdHandle(stNullUserCmd.TIME_USERCMD, cmdHandle, cmdHandle.call);
        }

        // 如果要调试可以重载，方便调试
        //public override void handleMsg(ByteBuffer msg)
        //{

        //}
    }
}