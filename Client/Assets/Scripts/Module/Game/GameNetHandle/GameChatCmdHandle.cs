using Game.Msg;
using Game.UI;
using SDK.Common;
using SDK.Lib;

namespace Game.Game
{
    public class GameChatCmdHandle : NetCmdHandleBase
    {
        public GameChatCmdHandle()
        {
            m_id2HandleDic[stChatUserCmd.CHAT_USERCMD_PARAMETER] = psstKokChatUserCmd;
        }

        public void psstKokChatUserCmd(ByteBuffer msg)
        {
            stKokChatUserCmd cmd = new stKokChatUserCmd();
            cmd.derialize(msg);

            UIChat uiChat = Ctx.m_instance.m_uiMgr.getForm<UIChat>(UIFormID.UIChat);
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