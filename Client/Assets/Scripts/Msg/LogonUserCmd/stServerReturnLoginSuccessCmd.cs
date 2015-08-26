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
            ba.readUnsignedInt32(ref dwUserID);
            ba.readUnsignedInt32(ref loginTempID);
            ba.readMultiByte(ref pstrIP, CVMsg.MAX_IP_LENGTH, GkEncode.UTF8);
            ba.readUnsignedInt16(ref wdPort);
            keyAux = new ByteBuffer();
            byte[] ret = new byte[256];
            ba.readBytes(ref ret, 256);
            keyAux.writeBytes(ret, 0, 256);
            keyAux.position = 58;
            byte index = 0;
            keyAux.readUnsignedInt8(ref index);
            keyAux.position = index;
            key = new byte[8];
            keyAux.readBytes(ref key, 8);
            ba.readUnsignedInt32(ref state);
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