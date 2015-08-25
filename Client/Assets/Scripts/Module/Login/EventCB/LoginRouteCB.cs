using SDK.Lib;

namespace Game.Login
{
    public class LoginRouteCB : MsgRouteDispHandle
    {
        public LoginRouteCB()
        {
            m_id2DispDic[(int)MsgRouteType.eMRT_BASIC] = new LoginRouteHandle();
        }
    }
}