using UnityEngine;
using System;
using System.Collections.Generic;
using SDK.Common;
using System.Threading;

namespace SDK.Lib
{
    public class Logger : ILogger
    {
        public Mutex m_visitMutex = new Mutex();    // 主要是添加和获取数据互斥
        public List<string> m_strList = new List<string>();              // 这个是多线程访问的

        public void log(string message)
        {
            logout(message, LogColor.LOG);
        }

        // 多线程日志
        public void synclog(string message)
        {
            m_visitMutex.WaitOne();
            m_strList.Add(message);
            m_visitMutex.ReleaseMutex();
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

        public void updateLog()
        {
            m_visitMutex.WaitOne();
            foreach (string str in m_strList)
            {
                log(str);
            }
            m_strList.Clear();
            m_visitMutex.ReleaseMutex();
        }
    }
}