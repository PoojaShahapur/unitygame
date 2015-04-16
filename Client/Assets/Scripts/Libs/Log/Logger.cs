using UnityEngine;
using System;
using System.Collections.Generic;
using SDK.Common;

namespace SDK.Lib
{
    public class Logger
    {
        public MMutex m_visitMutex = new MMutex(false, "LoggerMutex");    // 主要是添加和获取数据互斥
        public List<string> m_strList = new List<string>();              // 这个是多线程访问的
        public string m_tmpStr;
        public bool m_bOutLog = true;          // 是否输出日志

        protected List<LogDeviceBase> m_logDeviceList = new List<LogDeviceBase>();

        public Logger()
        {
#if UNITY_5
            Application.logMessageReceived += onDebugLogCallbackHandler;
            Application.logMessageReceivedThreaded += onDebugLogCallbackThreadHandler;
#elif UNITY_4_6
            Application.RegisterLogCallback(onDebugLogCallbackHandler);
            Application.RegisterLogCallbackThreaded(onDebugLogCallbackThreadHandler);
#endif

            registerDevice();
        }

        protected void registerDevice()
        {
            LogDeviceBase logDevice = null;

#if ENABLE_WINLOG
            logDevice = new WinLogDevice();
            logDevice.initDevice();
            m_logDeviceList.Add(logDevice);
#endif

#if ENABLE_NETLOG
            logDevice = new NetLogDevice();
            logDevice.initDevice();
            m_logDeviceList.Add(logDevice);
#endif

            //logDevice = new FileLogDevice();
            //logDevice.initDevice();
            //m_logDeviceList.Add(logDevice);
        }

        // 需要一个参数的
        public void debugLog_1(LangItemID idx, string str)
        {
            Ctx.m_instance.m_langMgr.getText(LangTypeId.eDebug5, (int)idx);
            m_tmpStr = string.Format(Ctx.m_instance.m_shareData.m_retLangStr, str);
            Ctx.m_instance.m_log.log(m_tmpStr);
        }

        public void formatLog(LangTypeId type, int item, params string[] param)
        {
            Ctx.m_instance.m_langMgr.getText(type, item);
            if (param.Length == 0)
            {
                m_tmpStr = Ctx.m_instance.m_shareData.m_retLangStr;
            }
            else if (param.Length == 1)
            {
                m_tmpStr = string.Format(Ctx.m_instance.m_shareData.m_retLangStr, param[0], param[1]);
            }
            Ctx.m_instance.m_log.log(m_tmpStr);
        }

        public void log(string message)
        {
            logout(message, LogColor.LOG);
        }

        // 多线程日志
        public void asynclog(string message)
        {
            using (MLock mlock = new MLock(m_visitMutex))
            {
                m_strList.Add(message);
            }
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
        #if THREAD_CALLCHECK
            MThread.needMainThread();
        #endif

            if (m_bOutLog)
            {
                foreach (LogDeviceBase logDevice in m_logDeviceList)
                {
                    logDevice.logout(message, type);
                }
            }
        }

        public void updateLog()
        {
        #if THREAD_CALLCHECK
            MThread.needMainThread();
        #endif

            using (MLock mlock = new MLock(m_visitMutex))
            {
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
            }
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

            Ctx.m_instance.m_log.asynclog("onDebugLogCallbackThreadHandler ---- Error");
            Ctx.m_instance.m_log.asynclog(name);
            Ctx.m_instance.m_log.asynclog(stack);
        }
    }
}