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

        public override void derialize(ByteBuffer bu)
        {
            base.derialize(bu);

            bu.readUnsignedInt32(ref dwType);
            bu.readUnsignedInt32(ref dwSysInfoType);
            bu.readMultiByte(ref pstrName, CVMsg.MAX_NAMESIZE, GkEncode.UTF8);
            bu.readMultiByte(ref pstrChat, CVMsg.MAX_CHATINFO, GkEncode.UTF8);
            bu.readUnsignedInt32(ref dwFromID);
            bu.readUnsignedInt32(ref dwChannelID);
        }

        public override void serialize(ByteBuffer bu)
        {
            base.serialize(bu);

            bu.writeUnsignedInt32(dwType);
            bu.writeUnsignedInt32(dwSysInfoType);
            bu.writeMultiByte(pstrName, GkEncode.UTF8, CVMsg.MAX_NAMESIZE);
            bu.writeMultiByte(pstrChat, GkEncode.UTF8, CVMsg.MAX_CHATINFO);
            bu.writeUnsignedInt32(dwFromID);
            bu.writeUnsignedInt32(dwChannelID);
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