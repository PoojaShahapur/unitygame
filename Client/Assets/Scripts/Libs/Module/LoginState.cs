namespace SDK.Lib
{
    public enum LoginState
    {
        eLoginNone,     // 没有登陆
        eLoginingLoginServer,      // 正在登陆登陆服务器
        eLoginSuccessLoginServer,  // 登陆登陆服务器登陆成功
        eLoginingGateServer,      // 正在登陆网关服务器
        eLoginSuccessGateServer,  // 登陆网关成功
        eLoginFailedGateServer,   // 登陆网关失败，需要重新连接登陆服务器
        eLoginInfoError,          // 信息错误，如账号密码错误、版本错误，需要重新连接登陆服务器
        eLoginNewCharError,       // 建立角色错误
    }
}