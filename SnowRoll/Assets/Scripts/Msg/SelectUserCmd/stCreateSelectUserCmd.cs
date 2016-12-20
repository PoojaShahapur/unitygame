using SDK.Lib;

namespace Game.Msg
{
    public class stCreateSelectUserCmd : stSelectUserCmd
    {
        public string strUserName;
        public ushort gender;
        public ushort race;
        public uint hair;
        public uint face;
        public ushort career;
        public uint country;
        public byte height;
        public byte weight;

        public stCreateSelectUserCmd()
        {
            byParam = CREATE_SELECT_USERCMD_PARA;
        }

        public override void serialize(ByteBuffer bu)
        {
            base.serialize(bu);
            bu.writeMultiByte(strUserName, GkEncode.eUTF8, ProtoCV.MAX_NAMESIZE + 1);
            bu.writeUnsignedInt16(gender);
            bu.writeUnsignedInt16(race);

            bu.writeUnsignedInt32(hair);
            bu.writeUnsignedInt32(face);

            bu.writeUnsignedInt16(career);
            bu.writeUnsignedInt32(country);

            bu.writeUnsignedInt8(height);
            bu.writeUnsignedInt8(weight);
        }
    }
}


/// 请求创建用户档案
//const BYTE CREATE_SELECT_USERCMD_PARA = 2;
//struct stCreateSelectUserCmd : public stSelectUserCmd
//{
//  stCreateSelectUserCmd()
//  {
//    byParam = CREATE_SELECT_USERCMD_PARA;
//    bzero(strUserName, sizeof(strUserName));
//    gender = 0;
//    race = 0;
//    hair = 0;
//    face = 0;
//    career = 0;
//    country = 0;
//    height = 0;
//    weight = 0;
//  }

//  char strUserName[MAX_NAMESIZE+1];  /**< 用户名字  */
//  WORD gender;
//  WORD race;
//  DWORD hair;
//  DWORD face;
//  WORD career;
//  DWORD country;
//  BYTE height;
//  BYTE weight;
//#if 0
//  WORD	JobType;			//[shx Add 职业]
//  WORD  Face;               //[shx Add 头像]
//  WORD charType;
//  BYTE byHairType;      /**< 头发类型 */
//  DWORD byRGB;        /**< 颜色RGB */
//  WORD country;        /**< 国家ID */
//  WORD five;          /**< 五行主属性 */
//#endif
//};