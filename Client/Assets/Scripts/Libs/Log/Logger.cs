using UnityEngine;
using System;
using System.Collections.Generic;
using SDK.Common;
using System.Diagnostics;

namespace SDK.Lib
{
    public class Logger
    {
        public LockList<string> m_asyncLogList = new LockList<string>("Logger_asyncLogList", 4);              // 这个是多线程访问的
        public LockList<string> m_asyncWarnList = new LockList<string>("Logger_asyncWarnList", 4);            // 这个是多线程访问的
        public LockList<string> m_asyncErrorList = new LockList<string>("Logger_asyncErrorList", 4);          // 这个是多线程访问的

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

#if ENABLE_FILELOG
            logDevice = new FileLogDevice();
            logDevice.initDevice();
            m_logDeviceList.Add(logDevice);
#endif
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
            //StackTrace stackTrace = new StackTrace(true);
            //string traceStr = stackTrace.ToString();
            //message = string.Format("{0}\n{1}", message, traceStr);

            logout(message, LogColor.LOG);
        }

        public void warn(string message)
        {
            StackTrace stackTrace = new StackTrace(true);
            string traceStr = stackTrace.ToString();
            message = string.Format("{0}\n{1}", message, traceStr);

			logout(message, LogColor.WARN);
        }
        
        public void error(string message)
        {
            StackTrace stackTrace = new StackTrace(true);
            string traceStr = stackTrace.ToString();
            message = string.Format("{0}\n{1}", message, traceStr);

			logout(message, LogColor.ERROR);
        }

        // 多线程日志
        public void asyncLog(string message)
        {
            m_asyncLogList.Add(message);

            //ThreadLogMR threadLog = new ThreadLogMR();
            //threadLog.m_log = message;
            //Ctx.m_instance.m_sysMsgRoute.push(threadLog);
        }

        // 多线程日志
        public void asyncWarn(string message)
        {
            StackTrace stackTrace = new StackTrace(true);        // 这个在 new 的地方生成当时堆栈数据，需要的时候再 new ，否则是旧的堆栈数据
            string traceStr = stackTrace.ToString();
            message = string.Format("{0}\n{1}", message, traceStr);

            m_asyncWarnList.Add(message);

            //ThreadLogMR threadLog = new ThreadLogMR();
            //threadLog.m_log = message;
            //Ctx.m_instance.m_sysMsgRoute.push(threadLog);
        }

        // 多线程日志
        public void asyncError(string message)
        {
            StackTrace stackTrace = new StackTrace(true);        // 这个在 new 的地方生成当时堆栈数据，需要的时候再 new ，否则是旧的堆栈数据
            string traceStr = stackTrace.ToString();
            message = string.Format("{0}\n{1}", message, traceStr);

            m_asyncErrorList.Add(message);

            //ThreadLogMR threadLog = new ThreadLogMR();
            //threadLog.m_log = message;
            //Ctx.m_instance.m_sysMsgRoute.push(threadLog);
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

            while ((m_tmpStr = m_asyncLogList.RemoveAt(0)) != default(string))
            {
                logout(m_tmpStr, LogColor.LOG);
            }

            while ((m_tmpStr = m_asyncWarnList.RemoveAt(0)) != default(string))
            {
                logout(m_tmpStr, LogColor.LOG);
            }

            while ((m_tmpStr = m_asyncErrorList.RemoveAt(0)) != default(string))
            {
                logout(m_tmpStr, LogColor.LOG);
            }
        }

        static private void onDebugLogCallbackHandler(string name, string stack, LogType type) 
        { 
            // LogType.Log 日志直接自己输出
            if (LogType.Error == type || LogType.Exception == type)
            {
                Ctx.m_instance.m_log.error("onDebugLogCallbackHandler ---- Error");
                Ctx.m_instance.m_log.error(name);
                Ctx.m_instance.m_log.error(stack);
            }
            else if(LogType.Assert == type || LogType.Warning == type)
            {
                Ctx.m_instance.m_log.warn("onDebugLogCallbackHandler ---- Warning");
                Ctx.m_instance.m_log.warn(name);
                Ctx.m_instance.m_log.warn(stack);
            }
        }

        static private void onDebugLogCallbackThreadHandler(string name, string stack, LogType type)
        {
            if (LogType.Error == type || LogType.Exception == type)
            {
                Ctx.m_instance.m_log.asyncError("onDebugLogCallbackThreadHandler ---- Error");
                Ctx.m_instance.m_log.asyncError(name);
                Ctx.m_instance.m_log.asyncError(stack);
            }
            else if (LogType.Assert == type || LogType.Warning == type)
            {
                Ctx.m_instance.m_log.asyncWarn("onDebugLogCallbackThreadHandler ---- Warning");
                Ctx.m_instance.m_log.asyncWarn(name);
                Ctx.m_instance.m_log.asyncWarn(stack);
            }
        }

        public void closeDevice()
        {
            foreach (LogDeviceBase logDevice in m_logDeviceList)
            {
                logDevice.closeDevice();
            }
        }
    }
}