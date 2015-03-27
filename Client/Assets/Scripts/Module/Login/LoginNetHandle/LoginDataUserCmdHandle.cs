using Game.Msg;
using SDK.Common;

namespace Game.Login
{
    public class LoginDataUserCmdHandle : NetCmdHandleBase
    {
        public LoginDataUserCmdHandle()
        {
            m_id2HandleDic[stDataUserCmd.MERGE_VERSION_CHECK_USERCMD_PARA] = LoginSys.m_instance.m_loginFlowHandle.receiveMsg6f;
        }

        // 如果要调试，可以重载，方便调试
        //public override void handleMsg(ByteArray ba, byte byCmd, byte byParam)
        //{
        //
        //}
    }
}