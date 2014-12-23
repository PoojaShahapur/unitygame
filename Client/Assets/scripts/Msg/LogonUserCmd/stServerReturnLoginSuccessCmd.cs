using SDK.Common;
namespace Game.Msg
{
    public class stServerReturnLoginSuccessCmd : stLogonUserCmd
    {
        public uint dwUserID;
        public uint loginTempID;
        public string pstrIP;
        public ushort wdPort;
        public string key;
        public uint state;

        public stServerReturnLoginSuccessCmd()
        {
            byParam = SERVER_RETURN_LOGIN_OK;
        }

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);
            dwUserID = ba.readUnsignedInt();
            loginTempID = ba.readUnsignedInt();
            pstrIP = ba.readMultiByte(CVMsg.MAX_IP_LENGTH, GkEncode.UTF8);
            wdPort = ba.readUnsignedShort();
            key = ba.readMultiByte(256, GkEncode.UTF8);
            state = ba.readUnsignedInt();
        }
    }
}


//const BYTE SERVER_RETURN_LOGIN_OK = 4;
//struct stServerReturnLoginSuccessCmd : public stLogonUserCmd 
//{
//  stServerReturnLoginSuccessCmd()
//  {
//    byParam = SERVER_RETURN_LOGIN_OK;
//    bzero(pstrIP, sizeof(pstrIP));
//  }

//  DWORD dwUserID;
//  DWORD loginTempID;
//  char pstrIP[MAX_IP_LENGTH];
//  WORD wdPort;

//  union{
//    struct{
//      BYTE randnum[58];
//      BYTE keyOffset;  // 密匙在 key 中的偏移
//    };
//    BYTE key[256];  // 保存密匙，整个数组用随机数填充
//  };
//  DWORD state;
//};