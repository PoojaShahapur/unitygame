using UnityEngine;
using System;
using System.Collections.Generic;
using SDK.Common;

namespace SDK.Lib
{
    public class Logger : ILogger
    {
        public void log(string message)
        {
            logout(message, LogColor.LOG);
        }

        public void warn(string message)
        {
			logout(message, LogColor.WARN);
        }
        
        public void error(string message)
        {
			logout(message, LogColor.ERROR);
        }

        public void logout(string message, LogColor type = LogColor.LOG)
        {
            if (type == LogColor.LOG)
            {
                Debug.Log(message);
            }
            else if (type == LogColor.WARN)
            {
                Debug.LogWarning(message);
            }
            else if (type == LogColor.ERROR)
            {
                Debug.LogError(message);
            }
        }
    }
}