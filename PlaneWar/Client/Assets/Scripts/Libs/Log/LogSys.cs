using UnityEngine;

namespace SDK.Lib
{
    public class LogSys
    {
        protected LockList<string> mAsyncLogList;              // 这个是多线程访问的
        protected LockList<string> mAsyncWarnList;            // 这个是多线程访问的
        protected LockList<string> mAsyncErrorList;          // 这个是多线程访问的

        protected string mTmpStr;

        protected MList<LogDeviceBase> mLogDeviceList;
        protected MList<LogTypeId>[] mEnableLogTypeList;

        protected bool[] mEnableLog;    // 全局开关
        protected bool[] mIsOutStack;     // 是否显示堆栈信息
        protected bool[] mIsOutTimeStamp;   // 是否有时间戳

        // 构造函数仅仅是初始化变量，不涉及逻辑
        public LogSys()
        {
            this.mAsyncLogList = new LockList<string>("Logger_asyncLogList");
            this.mAsyncWarnList = new LockList<string>("Logger_asyncWarnList");
            this.mAsyncErrorList = new LockList<string>("Logger_asyncErrorList");
            this.mLogDeviceList = new MList<LogDeviceBase>();

#if UNITY_5
            Application.logMessageReceived += onDebugLogCallbackHandler;
            Application.logMessageReceivedThreaded += onDebugLogCallbackThreadHandler;
#elif UNITY_4_6 || UNITY_4_5
            Application.RegisterLogCallback(onDebugLogCallbackHandler);
            Application.RegisterLogCallbackThreaded(onDebugLogCallbackThreadHandler);
#endif

            this.mEnableLogTypeList = new MList<LogTypeId>[(int)LogColor.eLC_Count];

            this.mEnableLogTypeList[(int)LogColor.eLC_LOG] = new MList<LogTypeId>();
            //this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogCommon);
            //this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogResLoader);
            //this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogLocalFile);
            //this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogTestRL);
            //this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogAcceleration);
            //this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogUnityCB);

            //this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogSplitMergeEmit);
            //this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogSceneInterActive);
            //this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogKBE);
            //this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogScene);
            //this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogBeingMove);
            //this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogEventRemove);
            //this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogMusicBug);
            //this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogLoadBug);
            //this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogMergeBug);
            //this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogEatBug);
            //this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogSimHitBullet);
            this.mEnableLogTypeList[(int)LogColor.eLC_LOG].Add(LogTypeId.eLogTwoDTerrain);

            this.mEnableLogTypeList[(int)LogColor.eLC_WARN] = new MList<LogTypeId>();

            this.mEnableLogTypeList[(int)LogColor.eLC_ERROR] = new MList<LogTypeId>();
            //this.mEnableLogTypeList[(int)LogColor.eLC_ERROR].Add(LogTypeId.eLogLoadBug);

            this.mEnableLog = new bool[(int)LogColor.eLC_Count];
            this.mEnableLog[(int)LogColor.eLC_LOG] = true;
            this.mEnableLog[(int)LogColor.eLC_WARN] = false;
            this.mEnableLog[(int)LogColor.eLC_ERROR] = false;

            this.mIsOutStack = new bool[(int)LogColor.eLC_Count];
            this.mIsOutStack[(int)LogColor.eLC_LOG] = false;
            this.mIsOutStack[(int)LogColor.eLC_WARN] = false;
            this.mIsOutStack[(int)LogColor.eLC_ERROR] = false;

            this.mIsOutTimeStamp = new bool[(int)LogColor.eLC_Count];
            this.mIsOutTimeStamp[(int)LogColor.eLC_LOG] = false;
            this.mIsOutTimeStamp[(int)LogColor.eLC_WARN] = false;
            this.mIsOutTimeStamp[(int)LogColor.eLC_ERROR] = false;
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
            this.closeDevice();
        }

        public void setEnableLog(bool value)
        {
            this.mEnableLog[(int)LogColor.eLC_LOG] = value;
        }

        public void setEnableWarn(bool value)
        {
            this.mEnableLog[(int)LogColor.eLC_WARN] = value;
        }

        public void setEnableError(bool value)
        {
            this.mEnableLog[(int)LogColor.eLC_ERROR] = value;
        }

        protected void registerDevice()
        {
            LogDeviceBase logDevice = null;

            if (MacroDef.ENABLE_WINLOG)
            {
                logDevice = new WinLogDevice();
                logDevice.initDevice();
                this.mLogDeviceList.Add(logDevice);
            }

            if (MacroDef.ENABLE_NETLOG)
            {
                logDevice = new NetLogDevice();
                logDevice.initDevice();
                this.mLogDeviceList.Add(logDevice);
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
                this.mLogDeviceList.Add(logDevice);
            }
        }

        protected void unRegisterFileLogDevice()
        {
            foreach(var item in mLogDeviceList.list())
            {
                if(typeof(FileLogDevice) == item.GetType())
                {
                    item.closeDevice();
                    this.mLogDeviceList.Remove(item);
                    break;
                }
            }
        }

        // 需要一个参数的
        public void debugLog_1(LangItemID idx, string str)
        {
            string textStr = Ctx.mInstance.mLangMgr.getText(LangTypeId.eDebug5, idx);
            this.mTmpStr = string.Format(textStr, str);
            //Ctx.mInstance.mLogSys.log(mTmpStr);
        }

        public void formatLog(LangTypeId type, LangItemID item, params string[] param)
        {
            if (param.Length == 0)
            {
                this.mTmpStr = Ctx.mInstance.mLangMgr.getText(type, item);
            }
            else if (param.Length == 1)
            {
                this.mTmpStr = string.Format(Ctx.mInstance.mLangMgr.getText(type, item), param[0], param[1]);
            }
            //Ctx.mInstance.mLogSys.log(mTmpStr);
        }

        /**
         * @brief 所有的异常日志都走这个接口
         */
        public void catchLog(string message)
        {
            log("Out Catch Log");
            log(message);
        }

        protected bool isInFilter(LogTypeId logTypeId, LogColor logColor)
        {
            if (this.mEnableLog[(int)logColor])
            {
                if (this.mEnableLogTypeList[(int)logColor].Contains(logTypeId))
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
            if (isInFilter(logTypeId, LogColor.eLC_LOG))
            {
                if(this.mIsOutTimeStamp[(int)LogColor.eLC_LOG])
                {
                    message = string.Format("{0}: {1}", UtilApi.getFormatTime(), message);
                }

                if (this.mIsOutStack[(int)LogColor.eLC_LOG])
                {
                    System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(true);
                    string traceStr = stackTrace.ToString();
                    message = string.Format("{0}\n{1}", message, traceStr);
                }

                if (MThread.isMainThread())
                {
                    this.logout(message, LogColor.eLC_LOG);
                }
                else
                {
                    this.asyncLog(message);
                }
            }
        }

        public void lua_warn(string message, int logTypeId = 0)
        {
            this.warn(message, (LogTypeId)logTypeId);
        }

        public void warn(string message, LogTypeId logTypeId = LogTypeId.eLogCommon)
        {
            if (isInFilter(logTypeId, LogColor.eLC_WARN))
            {
                if (this.mIsOutTimeStamp[(int)LogColor.eLC_WARN])
                {
                    message = string.Format("{0}: {1}", UtilApi.getFormatTime(), message);
                }

                if (this.mIsOutStack[(int)LogColor.eLC_WARN])
                {
                    System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(true);
                    string traceStr = stackTrace.ToString();
                    message = string.Format("{0}\n{1}", message, traceStr);
                }

                if (MThread.isMainThread())
                {
                    this.logout(message, LogColor.eLC_WARN);
                }
                else
                {
                    this.asyncWarn(message);
                }
            }
        }

        public void lua_error(string message, int logTypeId = 0)
        {
            this.error(message, (LogTypeId)logTypeId);
        }

        public void error(string message, LogTypeId logTypeId = LogTypeId.eLogCommon)
        {
            if (isInFilter(logTypeId, LogColor.eLC_ERROR))
            {
                if (this.mIsOutTimeStamp[(int)LogColor.eLC_ERROR])
                {
                    message = string.Format("{0}: {1}", UtilApi.getFormatTime(), message);
                }

                if (this.mIsOutStack[(int)LogColor.eLC_ERROR])
                {
                    System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(true);
                    string traceStr = stackTrace.ToString();
                    message = string.Format("{0}\n{1}", message, traceStr);
                }

                if (MThread.isMainThread())
                {
                    this.logout(message, LogColor.eLC_ERROR);
                }
                else
                {
                    this.asyncError(message);
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
            //StackTrace stackTrace = new StackTrace(true);        // 这个在 new 的地方生成当时堆栈数据，需要的时候再 new ，否则是旧的堆栈数据
            //string traceStr = stackTrace.ToString();
            //message = string.Format("{0}\n{1}", message, traceStr);

            this.mAsyncWarnList.Add(message);

            //ThreadLogMR threadLog = new ThreadLogMR();
            //threadLog.mLogSys = message;
            //Ctx.mInstance.mSysMsgRoute.push(threadLog);
        }

        // 多线程日志
        protected void asyncError(string message)
        {
            //StackTrace stackTrace = new StackTrace(true);        // 这个在 new 的地方生成当时堆栈数据，需要的时候再 new ，否则是旧的堆栈数据
            //string traceStr = stackTrace.ToString();
            //message = string.Format("{0}\n{1}", message, traceStr);

            this.mAsyncErrorList.Add(message);

            //ThreadLogMR threadLog = new ThreadLogMR();
            //threadLog.mLogSys = message;
            //Ctx.mInstance.mSysMsgRoute.push(threadLog);
        }

        public void logout(string message, LogColor type = LogColor.eLC_LOG)
        {
            if (MacroDef.THREAD_CALLCHECK)
            {
                MThread.needMainThread();
            }

            //foreach (LogDeviceBase logDevice in mLogDeviceList.list())
            int idx = 0;
            int len = this.mLogDeviceList.Count();
            LogDeviceBase logDevice = null;

            while (idx < len)
            {
                logDevice = this.mLogDeviceList[idx];
                logDevice.logout(message, type);

                ++idx;
            }
        }

        public void updateLog()
        {
            if (MacroDef.THREAD_CALLCHECK)
            {
                MThread.needMainThread();
            }

            while ((this.mTmpStr = mAsyncLogList.RemoveAt(0)) != default(string))
            {
                this.logout(mTmpStr, LogColor.eLC_LOG);
            }

            while ((this.mTmpStr = mAsyncWarnList.RemoveAt(0)) != default(string))
            {
                this.logout(mTmpStr, LogColor.eLC_WARN);
            }

            while ((this.mTmpStr = mAsyncErrorList.RemoveAt(0)) != default(string))
            {
                this.logout(mTmpStr, LogColor.eLC_ERROR);
            }
        }

        static private void onDebugLogCallbackHandler(string name, string stack, LogType type) 
        { 
            // LogType.Log 日志直接自己输出
            if (LogType.Error == type || LogType.Exception == type)
            {
                Ctx.mInstance.mLogSys.error("onDebugLogCallbackHandler ---- Error", LogTypeId.eLogUnityCB);
                Ctx.mInstance.mLogSys.error(name, LogTypeId.eLogUnityCB);
                Ctx.mInstance.mLogSys.error(stack, LogTypeId.eLogUnityCB);
            }
            else if(LogType.Assert == type || LogType.Warning == type)
            {
                Ctx.mInstance.mLogSys.warn("onDebugLogCallbackHandler ---- Warning", LogTypeId.eLogUnityCB);
                Ctx.mInstance.mLogSys.warn(name, LogTypeId.eLogUnityCB);
                Ctx.mInstance.mLogSys.warn(stack, LogTypeId.eLogUnityCB);
            }
        }

        static private void onDebugLogCallbackThreadHandler(string name, string stack, LogType type)
        {
            if (LogType.Error == type || LogType.Exception == type)
            {
                //Ctx.mInstance.mLogSys.asyncError("onDebugLogCallbackThreadHandler ---- Error");
                //Ctx.mInstance.mLogSys.asyncError(name);
                //Ctx.mInstance.mLogSys.asyncError(stack);
                Ctx.mInstance.mLogSys.error("onDebugLogCallbackThreadHandler ---- Error", LogTypeId.eLogUnityCB);
                Ctx.mInstance.mLogSys.error(name, LogTypeId.eLogUnityCB);
                Ctx.mInstance.mLogSys.error(stack, LogTypeId.eLogUnityCB);
            }
            else if (LogType.Assert == type || LogType.Warning == type)
            {
                //Ctx.mInstance.mLogSys.asyncWarn("onDebugLogCallbackThreadHandler ---- Warning");
                //Ctx.mInstance.mLogSys.asyncWarn(name);
                //Ctx.mInstance.mLogSys.asyncWarn(stack);
                Ctx.mInstance.mLogSys.warn("onDebugLogCallbackThreadHandler ---- Warning", LogTypeId.eLogUnityCB);
                Ctx.mInstance.mLogSys.warn(name, LogTypeId.eLogUnityCB);
                Ctx.mInstance.mLogSys.warn(stack, LogTypeId.eLogUnityCB);
            }
        }

        protected void closeDevice()
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