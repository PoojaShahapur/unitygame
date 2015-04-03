using SDK.Common;

namespace Game.Login
{
    public class LoginRouteHandle : MsgRouteHandleBase
    {
        public LoginRouteHandle()
        {
            m_id2HandleDic[(int)MsgRouteID.eMRIDSocketOpened] = handleSocketOpened;
        }

        protected void handleSocketOpened(MsgRouteBase msg)
        {
            if (Ctx.m_instance.m_loginSys.get_LoginState() == LoginState.eLoginingLoginServer)
            {
                (Ctx.m_instance.m_loginSys as LoginSys).m_loginFlowHandle.onLoginServerSocketOpened();
            }
            else if(Ctx.m_instance.m_loginSys.get_LoginState() == LoginState.eLoginingGateServer)
            {
                (Ctx.m_instance.m_loginSys as LoginSys).m_loginFlowHandle.onGateServerSocketOpened();
            }
        }
    }
}