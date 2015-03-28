using SDK.Common;
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

        public override void derialize(SDK.Common.ByteBuffer ba)
        {
            base.derialize(ba);

            dwType = ba.readUnsignedInt32();
            dwSysInfoType = ba.readUnsignedInt32();
            pstrName = ba.readMultiByte(CVMsg.MAX_NAMESIZE, GkEncode.UTF8);
            pstrChat = ba.readMultiByte(CVMsg.MAX_CHATINFO, GkEncode.UTF8);
            dwFromID = ba.readUnsignedInt32();
            dwChannelID = ba.readUnsignedInt32();
        }

        public override void serialize(SDK.Common.ByteBuffer ba)
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