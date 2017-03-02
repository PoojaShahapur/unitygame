using UnityEngine;
using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KBEngine
{
	public enum DEBUGLEVEL : int
	{
		DEBUG = 0,
		INFO,
		WARNING,
		ERROR,

		NOLOG,  // 放在最后面，使用这个时表示不输出任何日志（!!!慎用!!!）
	}

	public class Dbg 
	{
		static public DEBUGLEVEL debugLevel = DEBUGLEVEL.DEBUG;

#if UNITY_EDITOR
		static Dictionary<string, Profile> _profiles = new Dictionary<string, Profile>();
#endif

		public static string getHead()
		{
			return "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + "] ";
		}

		public static void INFO_MSG(object s)
		{
            if (DEBUGLEVEL.INFO >= debugLevel)
            {
                if (SDK.Lib.Ctx.mInstance.mLogSys == null)
                {
                    Debug.Log(getHead() + s);
                }
                else
                {
                    if (SDK.Lib.MacroDef.ENABLE_LOG)
                    {
                        SDK.Lib.Ctx.mInstance.mLogSys.log(string.Format("{0}{1}", getHead(), s), SDK.Lib.LogTypeId.eLogKBE);
                    }
                }
            }
		}

		public static void DEBUG_MSG(object s)
		{
            if (DEBUGLEVEL.DEBUG >= debugLevel)
            {
                if (SDK.Lib.Ctx.mInstance.mLogSys == null)
                {
                    Debug.Log(getHead() + s);
                }
                else
                {
                    if (SDK.Lib.MacroDef.ENABLE_LOG)
                    {
                        SDK.Lib.Ctx.mInstance.mLogSys.log(string.Format("{0}{1}", getHead(), s), SDK.Lib.LogTypeId.eLogKBE);
                    }
                }
            }
		}

		public static void WARNING_MSG(object s)
		{
            if (DEBUGLEVEL.WARNING >= debugLevel)
            {
                if (SDK.Lib.Ctx.mInstance.mLogSys == null)
                {
                    Debug.LogWarning(getHead() + s);
                }
                else
                {
                    if (SDK.Lib.MacroDef.ENABLE_WARN)
                    {
                        SDK.Lib.Ctx.mInstance.mLogSys.warn(string.Format("{0}{1}", getHead(), s), SDK.Lib.LogTypeId.eLogKBE);
                    }
                }
            }
		}

		public static void ERROR_MSG(object s)
		{
            if (DEBUGLEVEL.ERROR >= debugLevel)
            {
                if (SDK.Lib.Ctx.mInstance.mLogSys == null)
                {
                    Debug.LogError(getHead() + s);
                }
                else
                {
                    if (SDK.Lib.MacroDef.ENABLE_ERROR)
                    {
                        SDK.Lib.Ctx.mInstance.mLogSys.error(string.Format("{0}{1}", getHead(), s), SDK.Lib.LogTypeId.eLogKBE);
                    }
                }
            }
		}

		public static void profileStart(string name)
		{
#if UNITY_EDITOR
			Profile p = null;
			if(!_profiles.TryGetValue(name, out p))
			{
				p = new Profile(name);
				_profiles.Add(name, p);
			}

			p.start();
#endif
		}

		public static void profileEnd(string name)
		{
#if UNITY_EDITOR
			_profiles[name].end();
#endif
		}
		
	}
}
