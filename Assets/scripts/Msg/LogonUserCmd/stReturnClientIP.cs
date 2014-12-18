using SDK.Common;

namespace Game.Msg
{
    public class stReturnClientIP : stLogonUserCmd
    {
        public string pstrIP;

        public stReturnClientIP()
        {
            byParam = RETURN_CLIENT_IP_PARA;
        }

        public virtual void derialize(IByteArray ba)
        {
            base.derialize(ba);
            pstrIP = ba.readMultiByte((int)CVMsg.MAX_IP_LENGTH, GkEncode.UTF8);
        }
    }
}

//const BYTE RETURN_CLIENT_IP_PARA = 16;
//struct stReturnClientIP : public stLogonUserCmd
//{
//    stReturnClientIP()
//    {
//    byParam = RETURN_CLIENT_IP_PARA;
//    bzero(pstrIP, sizeof(pstrIP));
//    }
//    unsigned char pstrIP[MAX_IP_LENGTH];
//};