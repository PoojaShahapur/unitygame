using SDK.Common;

namespace Game.Msg
{
    public struct SelectUserInfo
    {
        uint id;            /// 角色编号
        string name;    /// 角色名称
        ushort type;            /// 角色类型
        ushort level;            /// 角色等级
        uint mapid;          /// 角色所在地图编号
        string mapName;  /// 角色所在地图名称
        ushort country;          ///  国家ID
                               ///  
        string countryName;  /// 国家名称
        uint bitmask;          /// 角色掩码
        uint zone_state;
        uint target_zone;
        uint model1;
        uint model2;
        uint model3;
        uint model4;
        uint model5;
        uint model6;
        uint model7;
        uint model8;
        uint model9;
        uint model10;
        uint model11;
        uint model12;
        uint model13;
        uint model14;
        uint model15;
        uint model16;

        byte effect11;
        byte effect12;
        byte effect13;
        byte effect14;
        byte effect15;
        byte effect16;
        byte effect17;
        byte effect18;

        ushort job;
        byte height;
        byte weight;
        ushort picbindloginclearnum;

        public void derialize(IByteArray ba)
        {
            id = ba.readUnsignedInt();
            name = ba.readMultiByte(CVMsg.MAX_NAMESIZE + 1, GkEncode.UTF8);
            type = ba.readUnsignedShort();
            level = ba.readUnsignedShort();
            mapid = ba.readUnsignedInt();
            mapName = ba.readMultiByte(CVMsg.MAX_NAMESIZE + 1, GkEncode.UTF8);
            country = ba.readUnsignedShort();

            countryName = ba.readMultiByte(CVMsg.MAX_NAMESIZE + 1, GkEncode.UTF8);
            bitmask = ba.readUnsignedInt();
            zone_state = ba.readUnsignedInt();
            target_zone = ba.readUnsignedInt();
            model1 = ba.readUnsignedInt();
            model2 = ba.readUnsignedInt();
            model3 = ba.readUnsignedInt();
            model4 = ba.readUnsignedInt();
            model5 = ba.readUnsignedInt();
            model6 = ba.readUnsignedInt();
            model7 = ba.readUnsignedInt();
            model8 = ba.readUnsignedInt();
            model9 = ba.readUnsignedInt();
            model10 = ba.readUnsignedInt();
            model11 = ba.readUnsignedInt();
            model12 = ba.readUnsignedInt();
            model13 = ba.readUnsignedInt();
            model14 = ba.readUnsignedInt();
            model15 = ba.readUnsignedInt();
            model16 = ba.readUnsignedInt();

            effect11 = ba.readUnsignedByte();
            effect12 = ba.readUnsignedByte();
            effect13 = ba.readUnsignedByte();
            effect14 = ba.readUnsignedByte();
            effect15 = ba.readUnsignedByte();
            effect16 = ba.readUnsignedByte();
            effect17 = ba.readUnsignedByte();
            effect18 = ba.readUnsignedByte();

            job = ba.readUnsignedShort();
            height = ba.readUnsignedByte();
            weight = ba.readUnsignedByte();
            picbindloginclearnum = ba.readUnsignedShort();
        }
    }
}


/// 角色信息
//struct SelectUserInfo
//{
//    DWORD id;            /// 角色编号
//    char  name[MAX_NAMESIZE+1];    /// 角色名称
//    //  WORD	JobType;			//[shx Add 职业]
//    WORD type;            /// 角色类型
//    WORD level;            /// 角色等级
//    DWORD mapid;          /// 角色所在地图编号
//    char  mapName[MAX_NAMESIZE+1];  /// 角色所在地图名称
//    WORD country;          ///  国家ID
//    // WORD face;
//    // WORD hair;             /// [shx Add] 发型
//    char  countryName[MAX_NAMESIZE+1];  /// 国家名称
//    DWORD bitmask;          /// 角色掩码
//    DWORD zone_state;
//    DWORD target_zone;
//    DWORD model1;
//    DWORD model2;
//    DWORD model3;
//    DWORD model4;
//    DWORD model5;
//    DWORD model6;
//    DWORD model7;
//    DWORD model8;
//    DWORD model9;
//    DWORD model10;
//    DWORD model11;
//    DWORD model12;
//    DWORD model13;
//    DWORD model14;
//    DWORD model15;
//    DWORD model16;

//    BYTE effect11;
//    BYTE effect12;
//    BYTE effect13;
//    BYTE effect14;
//    BYTE effect15;
//    BYTE effect16;
//    BYTE effect17;
//    BYTE effect18;

//    WORD job;
//    BYTE height;
//    BYTE weight;
//    WORD picbindloginclearnum;
//};