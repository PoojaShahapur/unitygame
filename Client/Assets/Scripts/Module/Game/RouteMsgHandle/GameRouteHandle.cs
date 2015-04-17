﻿using SDK.Common;

namespace Game.Game
{
    public class GameRouteHandle : MsgRouteHandleBase
    {
        public GameRouteHandle()
        {
            m_id2HandleDic[(int)MsgRouteID.eMRIDLoadedWebRes] = loadedWebRes;
            m_id2HandleDic[(int)MsgRouteID.eMRIDThreadLog] = threadLog;
        }

        protected void loadedWebRes(MsgRouteBase msg)
        {
            (msg as LoadedWebResMR).m_task.handleResult();
        }

        protected void threadLog(MsgRouteBase msg)
        {
            Ctx.m_instance.m_log.log((msg as ThreadLogMR).m_log);
        }
    }
}