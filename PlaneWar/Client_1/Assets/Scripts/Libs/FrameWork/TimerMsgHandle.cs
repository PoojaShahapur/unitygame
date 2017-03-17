using Game.Msg;

namespace SDK.Lib
{
    public class TimerMsgHandle
    {
        public uint m_loginTempID;
        public ulong qwGameTime;

        // 步骤 7 ，接收消息
        public void receiveMsg7f(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;
            
            stGameTimeTimerUserCmd cmd = new stGameTimeTimerUserCmd();
            cmd.derialize(msg);
            qwGameTime = cmd.qwGameTime;
        }

        // 步骤 8 ，接收消息
        public void receiveMsg8f(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stRequestUserGameTimeTimerUserCmd cmd = new stRequestUserGameTimeTimerUserCmd();
            cmd.derialize(msg);

            sendMsg9f();
        }

        // 步骤 9 ，发送消息
        public void sendMsg9f()
        {
            stUserGameTimeTimerUserCmd cmd = new stUserGameTimeTimerUserCmd();
            cmd.qwGameTime = (ulong)UtilApi.getUTCSec() + qwGameTime;
            cmd.dwUserTempID = m_loginTempID;
            UtilMsg.sendMsg(cmd);

            // 加载游戏模块
            //Ctx.mInstance.mModuleSys.loadModule(ModuleName.GAMEMN);
        }
    }
}