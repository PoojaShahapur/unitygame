using Game.Msg;
using SDK.Lib;
namespace SDK.Lib
{
    public class TimerMsgHandle
    {
        public uint m_loginTempID;
        public ulong qwGameTime;

        // 步骤 7 ，接收消息
        public void receiveMsg7f(ByteBuffer msg)
        {
            Ctx.m_instance.m_logSys.log(Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem10));

            stGameTimeTimerUserCmd cmd = new stGameTimeTimerUserCmd();
            cmd.derialize(msg);
            qwGameTime = cmd.qwGameTime;
        }

        // 步骤 8 ，接收消息
        public void receiveMsg8f(ByteBuffer msg)
        {
            Ctx.m_instance.m_logSys.log(Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem11));

            stRequestUserGameTimeTimerUserCmd cmd = new stRequestUserGameTimeTimerUserCmd();
            cmd.derialize(msg);

            sendMsg9f();
        }

        // 步骤 9 ，发送消息
        public void sendMsg9f()
        {
            Ctx.m_instance.m_logSys.log(Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog0, LangItemID.eItem12));

            stUserGameTimeTimerUserCmd cmd = new stUserGameTimeTimerUserCmd();
            cmd.qwGameTime = UtilApi.getUTCSec() + qwGameTime;
            cmd.dwUserTempID = m_loginTempID;
            UtilMsg.sendMsg(cmd);

            // 加载游戏模块
            //Ctx.m_instance.m_moduleSys.loadModule(ModuleName.GAMEMN);
        }
    }
}