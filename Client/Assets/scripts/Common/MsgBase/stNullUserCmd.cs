namespace SDK.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class stNullUserCmd
    {
        public const byte TIME_USERCMD = 2;
        public const byte DATA_USERCMD = 3;
        public const byte PROPERTY_USERCMD = 4;
        public const byte CHAT_USERCMD = 14;
        public const byte SELECT_USERCMD = 24;
        public const byte LOGON_USERCMD = 104;
        public const byte HERO_CARD_USERCMD = 162;

        public byte byCmd;
        public byte byParam;
        public uint dwTimestamp;

        public virtual void serialize(ByteArray ba)
        {
            ba.writeByte(byCmd);
            ba.writeByte(byParam);
            dwTimestamp = UtilApi.getUTCSec();
            ba.writeUnsignedInt(dwTimestamp);
        }

        public virtual void derialize(ByteArray ba)
        {
            byCmd = ba.readByte();
            byParam = ba.readByte();
            dwTimestamp = ba.readUnsignedInt();
        }
    }
}


//const BYTE NULL_USERCMD_PARA = 0;
//struct stNullUserCmd{
//  stNullUserCmd()
//  {
//    dwTimestamp=0;
//  }
//  union{
//    struct {
//      BYTE  byCmd;
//      BYTE  byParam;
//    };
//    struct {
//      BYTE  byCmdType;
//      BYTE  byParameterType;
//    };
//  };
//  DWORD  dwTimestamp;
//};