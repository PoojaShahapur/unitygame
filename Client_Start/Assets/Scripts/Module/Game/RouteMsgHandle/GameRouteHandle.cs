using SDK.Lib;

namespace Game.Game
{
    public class GameRouteHandle : MsgRouteHandleBase
    {
        public GameRouteHandle()
        {
            this.addMsgRouteHandle(MsgRouteID.eMRIDThreadLog, threadLog);
        }

        protected void threadLog(IDispatchObject dispObj)
        {
            MsgRouteBase msg = dispObj as MsgRouteBase;
            Ctx.m_instance.m_logSys.log((msg as ThreadLogMR).m_logSys);
        }
    }
}