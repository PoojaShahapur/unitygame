using SDK.Lib;

namespace Game.Msg
{
    public class stUserRequestLoginCmd : stLogonUserCmd
    {
        public string pstrName;
        public string pstrPassword;
        public ushort game;
        public ushort zone;
        public string jpegPassport;
        public string mac_addr;
        public string uuid;
        public ushort wdNetType;
        public string passpodPwd;

        public stUserRequestLoginCmd()
        {
            byParam = USER_REQUEST_LOGIN_PARA;
        }

        public override void serialize(ByteBuffer bu)
        {
            base.serialize(bu);

            bu.writeMultiByte(pstrName, GkEncode.UTF8, CVMsg.MAX_ACCNAMESIZE);
            bu.writeMultiByte(pstrPassword, GkEncode.UTF8, 33);
            bu.writeUnsignedInt16(game);
            bu.writeUnsignedInt16(zone);
            bu.writeMultiByte(jpegPassport, GkEncode.UTF8, 7);
            bu.writeMultiByte(mac_addr, GkEncode.UTF8, 13);
            bu.writeMultiByte(uuid, GkEncode.UTF8, 25);
            bu.writeUnsignedInt16(wdNetType);
            bu.writeMultiByte(passpodPwd, GkEncode.UTF8, 9);
        }

        //public override void derialize(ByteBuffer bu)
        //{
        //    base.derialize(bu);

        //    pstrName = bu.readMultiByte(CVMsg.MAX_ACCNAMESIZE, GkEncode.UTF8);
        //    pstrPassword = bu.readMultiByte(33, GkEncode.UTF8);
        //    game = bu.readUnsignedShort();
        //    zone = bu.readUnsignedShort();
        //    jpegPassport = bu.readMultiByte(7, GkEncode.UTF8);
        //    mac_addr = bu.readMultiByte(13, GkEncode.UTF8);
        //    uuid = bu.readMultiByte(25, GkEncode.UTF8);
        //    wdNetType = bu.readUnsignedShort();
        //    passpodPwd = bu.readMultiByte(9, GkEncode.UTF8);
        //}
    }
}


/// 客户端登陆登陆服务器
//const BYTE USER_REQUEST_LOGIN_PARA = 2;
//struct stUserRequestLoginCmd : public stLogonUserCmd
//{
//  stUserRequestLoginCmd()
//  {
//    byParam = USER_REQUEST_LOGIN_PARA;
//    bzero(pstrPassword, sizeof(pstrPassword));
//  }
//  char pstrName[MAX_ACCNAMESIZE];    /**< 帐号 */
//  unsigned char pstrPassword[33];  /**< 用户密码 */
//  WORD game;              /**< 游戏类型编号，目前一律添0 */
//  WORD zone;              /**< 游戏区编号 */
//  char jpegPassport[7];        /**< 图形验证码 */
//  char mac_addr[13];
//  unsigned char uuid[25];
//  WORD wdNetType;
//  unsigned char passpodPwd[9];
//};