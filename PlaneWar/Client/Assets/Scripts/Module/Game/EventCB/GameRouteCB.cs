using SDK.Lib;

namespace Game.Game
{
    public class GameRouteCB : MsgRouteDispHandle
    {
        public GameRouteCB()
        {
            GameRouteHandle gameRouteHandle = new GameRouteHandle();
            this.addRouteHandle((int)MsgRouteType.eMRT_BASIC, gameRouteHandle, gameRouteHandle.handleMsg);
        }
    }
}