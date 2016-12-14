using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

namespace SDK.Lib
{
    public class LogSys
    {
        protected LockList<string> mAsyncLogList = new LockList<string>("Logger_asyncLogList");              // 这个是多线程访问的
        protected LockList<string> mAsyncWarnList = new LockList<string>("Logger_asyncWarnList");            // 这个是多线程访问的
        protected LockList<string> mAsyncErrorList = new LockList<string>("Logger_asyncErrorList");          // 这个是多线程访问的

        public string mTmpStr;
        public bool mIsOutLog = true;          // 是否输出日志

        protected MList<LogDeviceBase> mLogDeviceList = new MList<LogDeviceBase>();
        protected MList<LogDeviceBase> mFightLogDeviceList = new MList<LogDeviceBase>();

        protected MList<LogTypeId> mEnableLogTypeList;
        protected bool mEnableLog;      // 全局开关

        // 构造函数仅仅是初始化变量，不涉及逻辑
        public LogSys()
        {
#if UNITY_5
            Application.logMessageReceived += onDebugLogCallbackHandler;
            Application.logMessageReceivedThreaded += onDebugLogCallbackThreadHandler;
#elif UNITY_4_6 || UNITY_4_5
            Application.RegisterLogCallback(onDebugLogCallbackHandler);
            Application.RegisterLogCallbackThreaded(onDebugLogCallbackThreadHandler);
#endif
            mEnableLogTypeList = new MList<LogTypeId>();
            mEnableLogTypeList.Add(LogTypeId.eLogCommon);
            mEnableLogTypeList.Add(LogTypeId.eLogResLoader);
            mEnableLogTypeList.Add(LogTypeId.eLogLocalFile);
            mEnableLogTypeList.Add(LogTypeId.eLogTestRL);
            mEnableLogTypeList.Add(LogTypeId.eLogAcceleration);

            mEnableLog = true;
        }

        // 初始化逻辑处理
        public void init()
        {
            this.registerDevice();
            this.registerFileLogDevice();
        }

        // 析构
        public void dispose()
        {

        }

        public void setEnableLog(bool value)
        {
            mEnableLog = value;
        }

        protected void registerDevice()
        {
            LogDeviceBase logDevice = null;

            if (MacroDef.ENABLE_WINLOG)
            {
                logDevice = new WinLogDevice();
                logDevice.initDevice();
                mLogDeviceList.Add(logDevice);
                mFightLogDeviceList.Add(logDevice);
            }

            if (MacroDef.ENABLE_NETLOG)
            {
                logDevice = new NetLogDevice();
                logDevice.initDevice();
                mLogDeviceList.Add(logDevice);
                mFightLogDeviceList.Add(logDevice);
            }
        }

        // 注册文件日志，因为需要账号，因此需要等待输入账号后才能注册，可能多次注册
        public void registerFileLogDevice()
        {
            Ctx.mInstance.mDataPlayer.m_accountData.m_account = "A1000";
            if (MacroDef.ENABLE_FILELOG)
            {
                unRegisterFileLogDevice();

                LogDeviceBase logDevice = null;
                logDevice = new FileLogDevice();
                (logDevice as FileLogDevice).fileSuffix = Ctx.mInstance.mDataPlayer.m_accountData.m_account;
                logDevice.initDevice();
                mLogDeviceList.Add(logDevice);

                logDevice = new FileLogDevice();
                (logDevice as FileLogDevice).fileSuffix = Ctx.mInstance.mDataPlayer.m_accountData.m_account;
                (logDevice as FileLogDevice).filePrefix = "FightLog";   // 战斗日志
                logDevice.initDevice();
                mFightLogDeviceList.Add(logDevice);
            }
        }

        protected void unRegisterFileLogDevice()
        {
            foreach(var item in mLogDeviceList.list())
            {
                if(typeof(FileLogDevice) == item.GetType())
                {
                    item.closeDevice();
                    mLogDeviceList.Remove(item);
                    break;
                }
            }
        }

        // 需要一个参数的
        public void debugLog_1(LangItemID idx, string str)
        {
            string textStr = Ctx.mInstance.mLangMgr.getText(LangTypeId.eDebug5, idx);
            mTmpStr = string.Format(textStr, str);
            Ctx.mInstance.mLogSys.log(mTmpStr);
        }

        public void formatLog(LangTypeId type, LangItemID item, params string[] param)
        {
            if (param.Length == 0)
            {
                mTmpStr = Ctx.mInstance.mLangMgr.getText(type, item);
            }
            else if (param.Length == 1)
            {
                mTmpStr = string.Format(Ctx.mInstance.mLangMgr.getText(type, item), param[0], param[1]);
            }
            Ctx.mInstance.mLogSys.log(mTmpStr);
        }

        /**
         * @brief 所有的异常日志都走这个接口
         */
        public void catchLog(string message)
        {
            log("Out Catch Log");
            log(message);
        }

        // 战斗日志，都是主线程中发送
        public void fightLog(string message)
        {
            if (MThread.isMainThread())
            {
                foreach (LogDeviceBase logDevice in mFightLogDeviceList.list())
                {
                    logDevice.logout(message, LogColor.LOG);
                }
            }
        }

        protected bool isInFilter(LogTypeId logTypeId)
        {
            if (mEnableLog)
            {
                if (mEnableLogTypeList.IndexOf(logTypeId) != -1)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        // Lua 调用 Log 这个函数的时候， LogTypeId 类型转换会报错，不能使用枚举类型
        public void lua_log(string message, int logTypeId = 0)
        {
            this.log(message, (LogTypeId)logTypeId);
        }

        public void log(string message, LogTypeId logTypeId = LogTypeId.eLogCommon)
        {
            //StackTrace stackTrace = new StackTrace(true);
            //string traceStr = stackTrace.ToString();
            //message = string.Format("{0}\n{1}", message, traceStr);
            if (isInFilter(logTypeId))
            {
                if (MThread.isMainThread())
                {
                    logout(message, LogColor.LOG);
                }
                else
                {
                    asyncLog(message);
                }
            }
        }

        public void lua_warn(string message, int logTypeId = 0)
        {
            this.warn(message, (LogTypeId)logTypeId);
        }

        public void warn(string message, LogTypeId logTypeId = LogTypeId.eLogCommon)
        {
            if (isInFilter(logTypeId))
            {
                StackTrace stackTrace = new StackTrace(true);
                string traceStr = stackTrace.ToString();
                message = string.Format("{0}\n{1}", message, traceStr);

                if (MThread.isMainThread())
                {
                    logout(message, LogColor.WARN);
                }
                else
                {
                    asyncWarn(message);
                }
            }
        }

        public void lua_error(string message, int logTypeId = 0)
        {
            this.error(message, (LogTypeId)logTypeId);
        }

        public void error(string message, LogTypeId logTypeId = LogTypeId.eLogCommon)
        {
            if (isInFilter(logTypeId))
            {
                StackTrace stackTrace = new StackTrace(true);
                string traceStr = stackTrace.ToString();
                message = string.Format("{0}\n{1}", message, traceStr);

                if (MThread.isMainThread())
                {
                    logout(message, LogColor.ERROR);
                }
                else
                {
                    asyncError(message);
                }
            }
        }

        // 多线程日志
        protected void asyncLog(string message)
        {
            mAsyncLogList.Add(message);

            //ThreadLogMR threadLog = new ThreadLogMR();
            //threadLog.mLogSys = message;
            //Ctx.mInstance.mSysMsgRoute.push(threadLog);
        }

        // 多线程日志
        protected void asyncWarn(string message)
        {
            StackTrace stackTrace = new StackTrace(true);        // 这个在 new 的地方生成当时堆栈数据，需要的时候再 new ，否则是旧的堆栈数据
            string traceStr = stackTrace.ToString();
            message = string.Format("{0}\n{1}", message, traceStr);

            mAsyncWarnList.Add(message);

            //ThreadLogMR threadLog = new ThreadLogMR();
            //threadLog.mLogSys = message;
            //Ctx.mInstance.mSysMsgRoute.push(threadLog);
        }

        // 多线程日志
        protected void asyncError(string message)
        {
            StackTrace stackTrace = new StackTrace(true);        // 这个在 new 的地方生成当时堆栈数据，需要的时候再 new ，否则是旧的堆栈数据
            string traceStr = stackTrace.ToString();
            message = string.Format("{0}\n{1}", message, traceStr);

            mAsyncErrorList.Add(message);

            //ThreadLogMR threadLog = new ThreadLogMR();
            //threadLog.mLogSys = message;
            //Ctx.mInstance.mSysMsgRoute.push(threadLog);
        }

        public void logout(string message, LogColor type = LogColor.LOG)
        {
            if (MacroDef.THREAD_CALLCHECK)
            {
                MThread.needMainThread();
            }

            if (mIsOutLog)
            {
                foreach (LogDeviceBase logDevice in mLogDeviceList.list())
                {
                    logDevice.logout(message, type);
                }
            }
        }

        public void updateLog()
        {
            if (MacroDef.THREAD_CALLCHECK)
            {
                MThread.needMainThread();
            }

            while ((mTmpStr = mAsyncLogList.RemoveAt(0)) != default(string))
            {
                logout(mTmpStr, LogColor.LOG);
            }

            while ((mTmpStr = mAsyncWarnList.RemoveAt(0)) != default(string))
            {
                logout(mTmpStr, LogColor.LOG);
            }

            while ((mTmpStr = mAsyncErrorList.RemoveAt(0)) != default(string))
            {
                logout(mTmpStr, LogColor.LOG);
            }
        }

        static private void onDebugLogCallbackHandler(string name, string stack, LogType type) 
        { 
            // LogType.Log 日志直接自己输出
            if (LogType.Error == type || LogType.Exception == type)
            {
                Ctx.mInstance.mLogSys.error("onDebugLogCallbackHandler ---- Error");
                Ctx.mInstance.mLogSys.error(name);
                Ctx.mInstance.mLogSys.error(stack);
            }
            else if(LogType.Assert == type || LogType.Warning == type)
            {
                Ctx.mInstance.mLogSys.warn("onDebugLogCallbackHandler ---- Warning");
                Ctx.mInstance.mLogSys.warn(name);
                Ctx.mInstance.mLogSys.warn(stack);
            }
        }

        static private void onDebugLogCallbackThreadHandler(string name, string stack, LogType type)
        {
            if (LogType.Error == type || LogType.Exception == type)
            {
                Ctx.mInstance.mLogSys.asyncError("onDebugLogCallbackThreadHandler ---- Error");
                Ctx.mInstance.mLogSys.asyncError(name);
                Ctx.mInstance.mLogSys.asyncError(stack);
            }
            else if (LogType.Assert == type || LogType.Warning == type)
            {
                Ctx.mInstance.mLogSys.asyncWarn("onDebugLogCallbackThreadHandler ---- Warning");
                Ctx.mInstance.mLogSys.asyncWarn(name);
                Ctx.mInstance.mLogSys.asyncWarn(stack);
            }
        }

        public void closeDevice()
        {
            foreach (LogDeviceBase logDevice in mLogDeviceList.list())
            {
                logDevice.closeDevice();
            }
        }

        public void logLoad(InsResBase res)
        {
            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                log(string.Format("{0} Loaded", res.getLoadPath()));
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                log(string.Format("{0} Failed", res.getLoadPath()));
            }
        }
    }
}