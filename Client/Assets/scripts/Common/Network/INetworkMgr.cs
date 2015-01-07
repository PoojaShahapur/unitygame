using System;
using System.Collections.Generic;

namespace SDK.Common
{
    public interface INetworkMgr
    {
        bool openSocket(string ip, int port);
        void closeSocket(string ip, int port);
        IByteArray getMsg();
        IByteArray getSendBA();
        void send();
        void quipApp();
    }
}