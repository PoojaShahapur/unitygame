using SDK.Common;

namespace Game.Game
{
    public class GameRouteHandle : MsgRouteHandleBase
    {
        public GameRouteHandle()
        {
            m_id2HandleDic[(int)MsgRouteID.eMRIDSocketOpened] = handleSocketOpened;
        }

        protected void handleSocketOpened(MsgRouteBase msg)
        {
            
        }
    }
}