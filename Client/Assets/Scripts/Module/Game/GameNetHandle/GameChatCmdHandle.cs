using Game.Msg;
using SDK.Lib;

namespace Game.Game
{
    public class GameChatCmdHandle : NetCmdHandleBase
    {
        public GameChatCmdHandle()
        {
            this.addParamHandle(stChatUserCmd.CHAT_USERCMD_PARAMETER,  psstKokChatUserCmd);
        }

        public void psstKokChatUserCmd(IDispatchObject dispObj)
        {
            ByteBuffer msg = dispObj as ByteBuffer;

            stKokChatUserCmd cmd = new stKokChatUserCmd();
            cmd.derialize(msg);

            IUIChat uiChat = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIChat) as IUIChat;
            if(uiChat != null)
            {
                cmd.pstrChat = cmd.pstrChat.TrimEnd('\0');
                uiChat.outMsg(cmd.pstrChat);
            }
            else
            {
                Ctx.m_instance.m_dataPlayer.m_chatData.appendStr(cmd.pstrChat);
            }
        }
    }
}