using SDK.Lib;

namespace Game.Game
{
    public class GameRouteCB : MsgRouteDispHandle
    {
        public GameRouteCB()
        {
            m_id2DispDic[(int)MsgRouteType.eMRT_BASIC] = new GameRouteHandle();
        }
    }
}