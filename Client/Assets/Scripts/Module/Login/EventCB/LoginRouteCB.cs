using SDK.Common;

namespace Game.Login
{
    public class LoginRouteCB : MsgRouteDispHandle
    {
        public LoginRouteCB()
        {
            m_id2DispDic[(int)MsgRouteID.eMRIDSocketOpened] = new LoginRouteHandle();
        }
    }
}