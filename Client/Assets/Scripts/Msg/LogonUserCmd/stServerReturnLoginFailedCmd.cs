using SDK.Common;

namespace Game.Msg
{
    public class stServerReturnLoginFailedCmd : stLogonUserCmd
    {
        public byte byReturnCode;

        public stServerReturnLoginFailedCmd()
        {
            byParam = SERVER_RETURN_LOGIN_FAILED;
        }

        public override void derialize(ByteBuffer ba)
        {
            base.derialize(ba);
            byReturnCode = ba.readUnsignedInt8();
        }
    }
}


/// 登陆失败后返回的信息
//const BYTE SERVER_RETURN_LOGIN_FAILED = 3;
//struct stServerReturnLoginFailedCmd : public stLogonUserCmd
//{
//  stServerReturnLoginFailedCmd()
//  {
//    byParam = SERVER_RETURN_LOGIN_FAILED;
//  }
//  BYTE byReturnCode;      /**< 返回的子参数 */
//} ;