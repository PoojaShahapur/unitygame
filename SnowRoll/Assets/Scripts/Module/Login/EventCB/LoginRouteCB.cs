using SDK.Lib;

namespace Game.Login
{
    public class LoginRouteCB : MsgRouteDispHandle
    {
        public LoginRouteCB()
        {
            LoginRouteHandle loginRouteHandle = new LoginRouteHandle();
            this.addRouteHandle((int)MsgRouteType.eMRT_BASIC, loginRouteHandle, loginRouteHandle.handleMsg);
        }
    }
}