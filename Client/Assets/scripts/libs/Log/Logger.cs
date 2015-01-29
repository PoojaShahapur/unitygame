using UnityEngine;
using System;
using System.Collections.Generic;
using SDK.Common;

namespace SDK.Lib
{
    public class Logger : ILogger
    {
        public MutexWrap m_visitMutex = new MutexWrap(false, "LoggerMutex");    // 主要是添加和获取数据互斥
        public List<string> m_strList = new List<string>();              // 这个是多线程访问的
        public string m_tmpStr;

        public Logger()
        {
            Application.RegisterLogCallback(onDebugLogCallbackHandler);
            Application.RegisterLogCallbackThreaded(onDebugLogCallbackThreadHandler);
        }

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
            m_tmpStr = "";
            foreach (string str in m_strList)
            {
                m_tmpStr += str;
            }
            if (m_tmpStr != "")
            {
                log(m_tmpStr);
            }
            m_strList.Clear();
            m_visitMutex.ReleaseMutex();
        }

        static private void onDebugLogCallbackHandler(string name, string stack, LogType type) 
        { 
            if (LogType.Error != type && LogType.Exception != type)
            { 
                return; 
            }

            Ctx.m_instance.m_log.log("onDebugLogCallbackHandler ---- Error");
            Ctx.m_instance.m_log.log(name);
            Ctx.m_instance.m_log.log(stack);
        }

        static private void onDebugLogCallbackThreadHandler(string name, string stack, LogType type)
        {
            if (LogType.Error != type && LogType.Exception != type)
            {
                return;
            }

            Ctx.m_instance.m_log.synclog("onDebugLogCallbackThreadHandler ---- Error");
            Ctx.m_instance.m_log.synclog(name);
            Ctx.m_instance.m_log.synclog(stack);
        }
    }
}