using System;
using System.Collections.Generic;

namespace San.Guo
{
    public interface INetworkMgr
    {
        bool openSocket(string ip, int port);
        ByteArray getMsg();
    }
}
