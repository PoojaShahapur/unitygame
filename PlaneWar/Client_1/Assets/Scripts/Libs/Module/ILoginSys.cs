namespace SDK.Lib
{
    /**
    * @brief 登陆模块接口
    */
    public interface ILoginSys
    {
        LoginState getLoginState();
        void setLoginState(LoginState state);
        void connectLoginServer(string name, string passwd, SelectEnterMode selectEnterMode);
        void unload();
        uint getUserID();
    }
}