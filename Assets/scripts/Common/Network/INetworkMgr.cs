using System;
using System.Collections.Generic;

namespace SDK.Common
{
    public interface INetworkMgr
    {
        bool openSocket(string ip, int port);
        IByteArray getMsg();
    }
}