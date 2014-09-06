using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace San.Guo
{
    public interface INetworkMgr
    {
        bool openSocket(string ip, int port);
        ByteArray getMsg();
    }
}
