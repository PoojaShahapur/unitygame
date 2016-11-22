using SDK.Lib;

namespace Game.Game
{
    public class GameRouteHandle : MsgRouteHandleBase
    {
        public GameRouteHandle()
        {
            this.addMsgRouteHandle(MsgRouteID.eMRIDThreadLog, threadLog);
            this.addMsgRouteHandle(MsgRouteID.eMRIDSocketOpened, onSocketOpened);
        }

        protected void threadLog(IDispatchObject dispObj)
        {
            MsgRouteBase msg = dispObj as MsgRouteBase;
            Ctx.m_instance.m_logSys.log((msg as ThreadLogMR).mLogSys);
        }

        protected void onSocketOpened(IDispatchObject dispObj)
        {
            Ctx.m_instance.m_luaSystem.onSocketConnected();
        }
    }
}