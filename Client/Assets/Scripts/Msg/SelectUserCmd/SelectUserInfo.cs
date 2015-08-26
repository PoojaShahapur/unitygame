using SDK.Lib;

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

        public void derialize(ByteBuffer ba)
        {
            ba.readUnsignedInt32(ref id);
            ba.readMultiByte(ref name, CVMsg.MAX_NAMESIZE + 1, GkEncode.UTF8);
            ba.readUnsignedInt16(ref type);
            ba.readUnsignedInt16(ref level);
            ba.readUnsignedInt32(ref mapid);
            ba.readMultiByte(ref mapName, CVMsg.MAX_NAMESIZE + 1, GkEncode.UTF8);
            ba.readUnsignedInt16(ref country);

            ba.readMultiByte(ref countryName, CVMsg.MAX_NAMESIZE + 1, GkEncode.UTF8);
            ba.readUnsignedInt32(ref bitmask);
            ba.readUnsignedInt32(ref zone_state);
            ba.readUnsignedInt32(ref target_zone);
            ba.readUnsignedInt32(ref model1);
            ba.readUnsignedInt32(ref model2);
            ba.readUnsignedInt32(ref model3);
            ba.readUnsignedInt32(ref model4);
            ba.readUnsignedInt32(ref model5);
            ba.readUnsignedInt32(ref model6);
            ba.readUnsignedInt32(ref model7);
            ba.readUnsignedInt32(ref model8);
            ba.readUnsignedInt32(ref model9);
            ba.readUnsignedInt32(ref model10);
            ba.readUnsignedInt32(ref model11);
            ba.readUnsignedInt32(ref model12);
            ba.readUnsignedInt32(ref model13);
            ba.readUnsignedInt32(ref model14);
            ba.readUnsignedInt32(ref model15);
            ba.readUnsignedInt32(ref model16);

            ba.readUnsignedInt8(ref effect11);
            ba.readUnsignedInt8(ref effect12);
            ba.readUnsignedInt8(ref effect13);
            ba.readUnsignedInt8(ref effect14);
            ba.readUnsignedInt8(ref effect15);
            ba.readUnsignedInt8(ref effect16);
            ba.readUnsignedInt8(ref effect17);
            ba.readUnsignedInt8(ref effect18);

            ba.readUnsignedInt16(ref job);
            ba.readUnsignedInt8(ref height);
            ba.readUnsignedInt8(ref weight);
            ba.readUnsignedInt16(ref picbindloginclearnum);
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