using SDK.Lib;

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

        public override void derialize(ByteBuffer ba)
        {
            base.derialize(ba);

            ba.readUnsignedInt32(ref dwType);
            ba.readUnsignedInt32(ref dwSysInfoType);
            ba.readMultiByte(ref pstrName, CVMsg.MAX_NAMESIZE, GkEncode.UTF8);
            ba.readMultiByte(ref pstrChat, CVMsg.MAX_CHATINFO, GkEncode.UTF8);
            ba.readUnsignedInt32(ref dwFromID);
            ba.readUnsignedInt32(ref dwChannelID);
        }

        public override void serialize(ByteBuffer ba)
        {
            base.serialize(ba);

            ba.writeUnsignedInt32(dwType);
            ba.writeUnsignedInt32(dwSysInfoType);
            ba.writeMultiByte(pstrName, GkEncode.UTF8, CVMsg.MAX_NAMESIZE);
            ba.writeMultiByte(pstrChat, GkEncode.UTF8, CVMsg.MAX_CHATINFO);
            ba.writeUnsignedInt32(dwFromID);
            ba.writeUnsignedInt32(dwChannelID);
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