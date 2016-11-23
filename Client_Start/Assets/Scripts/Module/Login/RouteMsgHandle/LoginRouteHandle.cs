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
            if (Ctx.mInstance.mLoginSys.getLoginState() == LoginState.eLoginingLoginServer)
            {
                (Ctx.mInstance.mLoginSys as LoginSys).mLoginFlowHandle.onLoginServerSocketOpened();
            }
            else if(Ctx.mInstance.mLoginSys.getLoginState() == LoginState.eLoginingGateServer)
            {
                (Ctx.mInstance.mLoginSys as LoginSys).mLoginFlowHandle.onGateServerSocketOpened();
            }
        }

        protected void threadLog(IDispatchObject dispObj)
        {
            MsgRouteBase msg = dispObj as MsgRouteBase;
            Ctx.mInstance.mLogSys.log((msg as ThreadLogMR).mLogSys);
        }
    }
}