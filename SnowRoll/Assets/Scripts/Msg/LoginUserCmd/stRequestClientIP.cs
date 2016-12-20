using Game.Msg;

namespace SDK.Lib
{
    public class stRequestClientIP : stLogonUserCmd
    {
        public stRequestClientIP()
        {
            byParam = REQUEST_CLIENT_IP_PARA;
        }
    }
}


//const BYTE REQUEST_CLIENT_IP_PARA = 15;
//struct stRequestClientIP : public stLogonUserCmd
//{
//    stRequestClientIP()
//    {
//    byParam = REQUEST_CLIENT_IP_PARA;
//    }
//};