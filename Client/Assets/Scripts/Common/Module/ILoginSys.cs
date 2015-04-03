namespace SDK.Common
{
    /**
    * @brief 登陆模块接口
    */
    public interface ILoginSys
    {
        LoginState get_LoginState();
        void set_LoginState(LoginState state);
        void connectLoginServer(string name, string passwd);
        void unload();
    }
}