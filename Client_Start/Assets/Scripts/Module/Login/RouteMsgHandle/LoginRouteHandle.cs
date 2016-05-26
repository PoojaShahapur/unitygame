using SDK.Lib;

namespace Game.Login
{
    public class LoginRouteHandle : MsgRouteHandleBase
    {
        public LoginRouteHandle()
        {
            this.addMsgRouteHandle(MsgRouteID.eMRIDSocketOpened, handleSocketOpened);
            this.addMsgRouteHandle(MsgRouteID.eMRIDThreadLog, threadLog);
        }

        protected void handleSocketOpened(IDispatchObject dispObj)
        {
            MsgRouteBase msg = dispObj as MsgRouteBase;
            if (Ctx.m_instance.m_loginSys.get_LoginState() == LoginState.eLoginingLoginServer)
            {
                (Ctx.m_instance.m_loginSys as LoginSys).m_loginFlowHandle.onLoginServerSocketOpened();
            }
            else if(Ctx.m_instance.m_loginSys.get_LoginState() == LoginState.eLoginingGateServer)
            {
                (Ctx.m_instance.m_loginSys as LoginSys).m_loginFlowHandle.onGateServerSocketOpened();
            }
        }

        protected void threadLog(IDispatchObject dispObj)
        {
            MsgRouteBase msg = dispObj as MsgRouteBase;
            Ctx.m_instance.m_logSys.log((msg as ThreadLogMR).m_logSys);
        }
    }
}