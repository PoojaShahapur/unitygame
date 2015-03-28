﻿using SDK.Common;
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

        public override void serialize(ByteBuffer ba)
        {
            base.serialize(ba);

            ba.writeMultiByte(pstrName, GkEncode.UTF8, CVMsg.MAX_ACCNAMESIZE);
            ba.writeMultiByte(pstrPassword, GkEncode.UTF8, 33);
            ba.writeUnsignedInt16(game);
            ba.writeUnsignedInt16(zone);
            ba.writeMultiByte(jpegPassport, GkEncode.UTF8, 7);
            ba.writeMultiByte(mac_addr, GkEncode.UTF8, 13);
            ba.writeMultiByte(uuid, GkEncode.UTF8, 25);
            ba.writeUnsignedInt16(wdNetType);
            ba.writeMultiByte(passpodPwd, GkEncode.UTF8, 9);
        }

        //public override void derialize(ByteBuffer ba)
        //{
        //    base.derialize(ba);

        //    pstrName = ba.readMultiByte(CVMsg.MAX_ACCNAMESIZE, GkEncode.UTF8);
        //    pstrPassword = ba.readMultiByte(33, GkEncode.UTF8);
        //    game = ba.readUnsignedShort();
        //    zone = ba.readUnsignedShort();
        //    jpegPassport = ba.readMultiByte(7, GkEncode.UTF8);
        //    mac_addr = ba.readMultiByte(13, GkEncode.UTF8);
        //    uuid = ba.readMultiByte(25, GkEncode.UTF8);
        //    wdNetType = ba.readUnsignedShort();
        //    passpodPwd = ba.readMultiByte(9, GkEncode.UTF8);
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