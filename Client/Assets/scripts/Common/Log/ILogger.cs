using System;
using System.Collections.Generic;

namespace SDK.Common
{
    public interface ILogger
    {
        void log(string message);
        void asynclog(string message);
        void warn(string message);
        void error(string message);
        void updateLog();
    }
}