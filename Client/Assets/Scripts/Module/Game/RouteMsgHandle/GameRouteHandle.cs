using SDK.Common;

namespace Game.Game
{
    public class GameRouteHandle : MsgRouteHandleBase
    {
        public GameRouteHandle()
        {
            m_id2HandleDic[(int)MsgRouteID.eMRIDLoadedWebRes] = loadedWebRes;
        }

        protected void loadedWebRes(MsgRouteBase msg)
        {
            (msg as LoadedWebResMR).m_task.handleResult();
        }
    }
}