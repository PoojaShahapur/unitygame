using SDK.Common;

namespace Game.Msg
{
    public class stKokChatUserCmd : stChatUserCmd
    {
        public uint dwType;      
        public uint dwSysInfoType;    
        public string pstrName;
        public string pstrChat;
        public uint dwFromID;
        public uint dwChannelID;

        public stKokChatUserCmd()
        {
            byParam = CHAT_USERCMD_PARAMETER;
        }

        public override void derialize(SDK.Common.IByteArray ba)
        {
            base.derialize(ba);

            dwType = ba.readUnsignedInt();
            dwSysInfoType = ba.readUnsignedInt();
            pstrName = ba.readMultiByte(CVMsg.MAX_NAMESIZE, GkEncode.UTF8);
            pstrChat = ba.readMultiByte(CVMsg.MAX_CHATINFO, GkEncode.UTF8);
            dwFromID = ba.readUnsignedInt();
            dwChannelID = ba.readUnsignedInt();
        }

        public override void serialize(SDK.Common.IByteArray ba)
        {
            base.serialize(ba);

            ba.writeUnsignedInt(dwType);
            ba.writeUnsignedInt(dwSysInfoType);
            ba.writeMultiByte(pstrName, GkEncode.UTF8, CVMsg.MAX_NAMESIZE);
            ba.writeMultiByte(pstrChat, GkEncode.UTF8, CVMsg.MAX_CHATINFO);
            ba.writeUnsignedInt(dwFromID);
            ba.writeUnsignedInt(dwChannelID);
        }
    }
    //const BYTE  CHAT_USERCMD_PARAMETER = 2;
    //struct stKokChatUserCmd : public stChatUserCmd
    //{
    //    stKokChatUserCmd()
    //    {    
    //        byParam = CHAT_USERCMD_PARAMETER;
    //        dwType= 0;
    //        dwSysInfoType= 0;
    //        bzero(pstrName, sizeof(pstrName));
    //        bzero(pstrChat, sizeof(pstrChat));
    //        dwFromID = 0; 
    //        dwChannelID = 0; 

    //    }    
    //    DWORD dwType;      
    //    DWORD dwSysInfoType;    
    //    char pstrName[MAX_NAMESIZE];
    //    char pstrChat[MAX_CHATINFO];
    //    DWORD dwFromID;
    //    DWORD dwChannelID;
    //};
}