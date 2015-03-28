using SDK.Common;
using SDK.Lib;
namespace Game.Msg
{
    public class stServerReturnLoginSuccessCmd : stLogonUserCmd
    {
        public uint dwUserID;
        public uint loginTempID;
        public string pstrIP;
        public ushort wdPort;
        public ByteBuffer keyAux;
        public uint state;

        public byte[] key;  // 客户端自己使用

        public stServerReturnLoginSuccessCmd()
        {
            byParam = SERVER_RETURN_LOGIN_OK;
        }

        public override void derialize(ByteBuffer ba)
        {
            base.derialize(ba);
            dwUserID = ba.readUnsignedInt32();
            loginTempID = ba.readUnsignedInt32();
            pstrIP = ba.readMultiByte(CVMsg.MAX_IP_LENGTH, GkEncode.UTF8);
            wdPort = ba.readUnsignedInt16();
            keyAux = new ByteBuffer();
            keyAux.writeBytes(ba.readBytes(256), 0, 256);
            keyAux.position = 58;
            keyAux.position = (uint)(keyAux.readUnsignedInt8());
            key = keyAux.readBytes(8);
            state = ba.readUnsignedInt32();
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