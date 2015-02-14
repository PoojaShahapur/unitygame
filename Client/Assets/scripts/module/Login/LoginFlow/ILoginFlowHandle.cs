using SDK.Common;

namespace Game.Login
{
    public interface ILoginFlowHandle
    {
        void connectLoginServer(string name, string passwd);
        void sendMsg1f();
        // 步骤 2 ，接收返回的消息
        void receiveMsg2f(IByteArray msg);
        // 步骤 3 ，发送消息
        void sendMsg3f();
        // 步骤 4 ，服务器返回消息
        void receiveMsg4f(IByteArray msg);
        // 步骤 5 ，发送消息
        void sendMsg5f();
        // 步骤 6 ，接收消息
        void receiveMsg6f(IByteArray msg);
        // 步骤 7 ，接收消息
        //void receiveMsg7f(IByteArray msg);
        // 步骤 8 ，接收消息
        //void receiveMsg8f(IByteArray msg);
        // 步骤 9 ，发送消息
        //void sendMsg9f();

        void psstServerReturnLoginFailedCmd(IByteArray msg);
        void psstUserInfoUserCmd(IByteArray ba);
        void psstLoginSelectSuccessUserCmd(IByteArray ba);
    }
}