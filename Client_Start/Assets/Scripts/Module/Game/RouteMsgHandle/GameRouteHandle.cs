using SDK.Lib;

namespace Game.Game
{
    public class GameRouteHandle : MsgRouteHandleBase
    {
        public GameRouteHandle()
        {
            m_id2HandleDic[(int)MsgRouteID.eMRIDThreadLog] = threadLog;
        }

        protected void threadLog(MsgRouteBase msg)
        {
            Ctx.m_instance.m_logSys.log((msg as ThreadLogMR).m_logSys);
        }
    }
}