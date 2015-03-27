using SDK.Common;

namespace Game.Login
{
    public interface ILoginFlowHandle
    {
        void connectLoginServer(string name, string passwd);
        void sendMsg1f();
        // 步骤 2 ，接收返回的消息
        void receiveMsg2f(ByteArray msg);
        // 步骤 3 ，发送消息
        void sendMsg3f();
        // 步骤 4 ，服务器返回消息
        void receiveMsg4f(ByteArray msg);
        // 步骤 5 ，发送消息
        void sendMsg5f();
        // 步骤 6 ，接收消息
        void receiveMsg6f(ByteArray msg);
        // 步骤 7 ，接收消息
        //void receiveMsg7f(ByteArray msg);
        // 步骤 8 ，接收消息
        //void receiveMsg8f(ByteArray msg);
        // 步骤 9 ，发送消息
        //void sendMsg9f();

        void psstServerReturnLoginFailedCmd(ByteArray msg);
        void psstUserInfoUserCmd(ByteArray ba);
        void psstLoginSelectSuccessUserCmd(ByteArray ba);
    }
}