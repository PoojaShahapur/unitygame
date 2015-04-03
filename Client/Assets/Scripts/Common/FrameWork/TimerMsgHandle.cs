using Game.Msg;
using SDK.Lib;
namespace SDK.Common
{
    public class TimerMsgHandle
    {
        public uint m_loginTempID;
        public ulong qwGameTime;

        // 步骤 7 ，接收消息
        public void receiveMsg7f(ByteBuffer msg)
        {
            Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eItem10);
            Ctx.m_instance.m_log.log(Ctx.m_instance.m_shareData.m_retLangStr);

            stGameTimeTimerUserCmd cmd = new stGameTimeTimerUserCmd();
            cmd.derialize(msg);
            qwGameTime = cmd.qwGameTime;
        }

        // 步骤 8 ，接收消息
        public void receiveMsg8f(ByteBuffer msg)
        {
            Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eItem11);
            Ctx.m_instance.m_log.log(Ctx.m_instance.m_shareData.m_retLangStr);

            stRequestUserGameTimeTimerUserCmd cmd = new stRequestUserGameTimeTimerUserCmd();
            cmd.derialize(msg);

            sendMsg9f();
        }

        // 步骤 9 ，发送消息
        public void sendMsg9f()
        {
            Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eItem12);
            Ctx.m_instance.m_log.log(Ctx.m_instance.m_shareData.m_retLangStr);

            stUserGameTimeTimerUserCmd cmd = new stUserGameTimeTimerUserCmd();
            cmd.qwGameTime = UtilApi.getUTCSec() + qwGameTime;
            cmd.dwUserTempID = m_loginTempID;
            UtilMsg.sendMsg(cmd);

            // 加载游戏模块
            //Ctx.m_instance.m_moduleSys.loadModule(ModuleName.GAMEMN);
        }
    }
}