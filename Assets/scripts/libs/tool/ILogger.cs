using System;
using System.Collections.Generic;

namespace San.Guo
{
    public interface ILogger
    {
        void log(string message);
        void warn(string message);
        void error(string message);
        void logout(string message, LogColor type = LogColor.LOG);
    }
}
