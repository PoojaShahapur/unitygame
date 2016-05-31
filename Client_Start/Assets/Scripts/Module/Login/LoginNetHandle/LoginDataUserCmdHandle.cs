using Game.Msg;
using SDK.Lib;

namespace Game.Login
{
    public class LoginDataUserCmdHandle : NetCmdDispHandle
    {
        public LoginDataUserCmdHandle()
        {
            this.addParamHandle(stDataUserCmd.MERGE_VERSION_CHECK_USERCMD_PARA, ((Ctx.m_instance.m_loginSys) as LoginSys).m_loginFlowHandle.receiveMsg6f);
        }

        // 如果要调试，可以重载，方便调试
        //public override void handleMsg(ByteBuffer bu, byte byCmd, byte byParam)
        //{
        //
        //}
    }
}