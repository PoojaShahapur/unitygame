﻿using UnityEngine;
using LuaInterface;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using SDK.Common;

namespace SDK.Lib
{
    public class Util
    {
        public static int Int(object o)
        {
            return Convert.ToInt32(o);
        }

        public static float Float(object o)
        {
            return (float)Math.Round(Convert.ToSingle(o), 2);
        }

        public static long Long(object o)
        {
            return Convert.ToInt64(o);
        }

        public static int Random(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static float Random(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static string Uid(string uid)
        {
            int position = uid.LastIndexOf('_');
            return uid.Remove(0, position + 1);
        }

        public static long GetTime()
        {
            TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            return (long)ts.TotalMilliseconds;
        }

        /// <summary>
        /// 搜索子物体组件-GameObject版
        /// </summary>
        public static T Get<T>(GameObject go, string subnode) where T : Component
        {
            if (go != null)
            {
                Transform sub = go.transform.FindChild(subnode);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Transform版
        /// </summary>
        public static T Get<T>(Transform go, string subnode) where T : Component
        {
            if (go != null)
            {
                Transform sub = go.FindChild(subnode);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Component版
        /// </summary>
        public static T Get<T>(Component go, string subnode) where T : Component
        {
            return go.transform.FindChild(subnode).GetComponent<T>();
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        public static T Add<T>(GameObject go) where T : Component
        {
            if (go != null)
            {
                T[] ts = go.GetComponents<T>();
                for (int i = 0; i < ts.Length; i++)
                {
                    if (ts[i] != null) UnityEngine.Object.Destroy(ts[i]);
                }
                return go.gameObject.AddComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        public static T Add<T>(Transform go) where T : Component
        {
            return Add<T>(go.gameObject);
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        public static GameObject Child(GameObject go, string subnode)
        {
            return Child(go.transform, subnode);
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        public static GameObject Child(Transform go, string subnode)
        {
            Transform tran = go.FindChild(subnode);
            if (tran == null) return null;
            return tran.gameObject;
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        public static GameObject Peer(GameObject go, string subnode)
        {
            return Peer(go.transform, subnode);
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        public static GameObject Peer(Transform go, string subnode)
        {
            Transform tran = go.parent.FindChild(subnode);
            if (tran == null) return null;
            return tran.gameObject;
        }

        /// <summary>
        /// 手机震动
        /// </summary>
        public static void Vibrate()
        {
            //int canVibrate = PlayerPrefs.GetInt(Const.AppPrefix + "Vibrate", 1);
            //if (canVibrate == 1) iPhoneUtils.Vibrate();
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        public static string Encode(string message)
        {
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(message);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        public static string Decode(string message)
        {
            byte[] bytes = Convert.FromBase64String(message);
            return Encoding.GetEncoding("utf-8").GetString(bytes);
        }

        /// <summary>
        /// 判断数字
        /// </summary>
        public static bool IsNumeric(string str)
        {
            if (str == null || str.Length == 0) return false;
            for (int i = 0; i < str.Length; i++)
            {
                if (!Char.IsNumber(str[i])) { return false; }
            }
            return true;
        }

        /// <summary>
        /// 清除所有子节点
        /// </summary>
        public static void ClearChild(Transform go)
        {
            if (go == null) return;
            for (int i = go.childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(go.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 生成一个Key名
        /// </summary>
        public static string GetKey(string key)
        {
            return "";
        }

        /// <summary>
        /// 取得整型
        /// </summary>
        public static int GetInt(string key)
        {
            string name = GetKey(key);
            return PlayerPrefs.GetInt(name);
        }

        /// <summary>
        /// 有没有值
        /// </summary>
        public static bool HasKey(string key)
        {
            string name = GetKey(key);
            return PlayerPrefs.HasKey(name);
        }

        /// <summary>
        /// 保存整型
        /// </summary>
        public static void SetInt(string key, int value)
        {
            string name = GetKey(key);
            PlayerPrefs.DeleteKey(name);
            PlayerPrefs.SetInt(name, value);
        }

        /// <summary>
        /// 取得数据
        /// </summary>
        public static string GetString(string key)
        {
            string name = GetKey(key);
            return PlayerPrefs.GetString(name);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public static void SetString(string key, string value)
        {
            string name = GetKey(key);
            PlayerPrefs.DeleteKey(name);
            PlayerPrefs.SetString(name, value);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        public static void RemoveData(string key)
        {
            string name = GetKey(key);
            PlayerPrefs.DeleteKey(name);
        }

        /// <summary>
        /// 清理内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect(); Resources.UnloadUnusedAssets();
            LuaScriptMgr mgr = Ctx.m_instance.m_luaMgr;
            if (mgr == null) mgr.LuaGC();
        }

        /// <summary>
        /// 是否为数字
        /// </summary>
        public static bool IsNumber(string strNumber)
        {
            Regex regex = new Regex("[^0-9]");
            return !regex.IsMatch(strNumber);
        }

        /// <summary>
        /// 取得数据存放目录
        /// </summary>
        public static string DataPath
        {
            get
            {
                string game = AppConst.AppName.ToLower();
                if (Application.isMobilePlatform)
                {
                    return Application.persistentDataPath + "/" + game + "/";
                }
                if (AppConst.DebugMode)
                {
                    return Application.dataPath + "/" + AppConst.AssetDirname + "/";
                }
                return "c:/" + game + "/";
            }
        }

        public static string GetRelativePath()
        {
            if (Application.isEditor)
                return "file://" + System.Environment.CurrentDirectory.Replace("\\", "/") + "/Assets/" + AppConst.AssetDirname + "/";
            else if (Application.isMobilePlatform || Application.isConsolePlatform)
                return "file:///" + DataPath;
            else // For standalone player.
                return "file://" + Application.streamingAssetsPath + "/";
        }

        /// <summary>
        /// 取得行文本
        /// </summary>
        public static string GetFileText(string path)
        {
            return File.ReadAllText(path);
        }

        /// <summary>
        /// 网络可用
        /// </summary>
        public static bool NetAvailable
        {
            get
            {
                return Application.internetReachability != NetworkReachability.NotReachable;
            }
        }

        /// <summary>
        /// 是否是无线
        /// </summary>
        public static bool IsWifi
        {
            get
            {
                return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
            }
        }

        /// <summary>
        /// 应用程序内容路径
        /// </summary>
        public static string AppContentPath()
        {
            string path = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    path = "jar:file://" + Application.dataPath + "!/assets/";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    path = Application.dataPath + "/Raw/";
                    break;
                default:
                    path = Application.dataPath + "/" + AppConst.AssetDirname + "/";
                    break;
            }
            return path;
        }

        /// <summary>
        /// 是否是登录场景
        /// </summary>
        public static bool isLogin
        {
            get { return Application.loadedLevelName.CompareTo("login") == 0; }
        }

        /// <summary>
        /// 是否是城镇场景
        /// </summary>
        public static bool isMain
        {
            get { return Application.loadedLevelName.CompareTo("main") == 0; }
        }

        /// <summary>
        /// 判断是否是战斗场景
        /// </summary>
        public static bool isFight
        {
            get { return Application.loadedLevelName.CompareTo("fight") == 0; }
        }

        public static string LuaPath()
        {
            if (AppConst.DebugMode)
            {
                return Application.dataPath + "/lua/";
            }
            return DataPath + "lua/";
        }

        /// <summary>
        /// 取得Lua路径
        /// </summary>
        public static string LuaPath(string name)
        {
            string path = AppConst.DebugMode ? Application.dataPath + "/" : DataPath;
            string lowerName = name.ToLower();
            if (lowerName.EndsWith(".lua"))
            {
                int index = name.LastIndexOf('.');
                name = name.Substring(0, index);
            }
            name = name.Replace('.', '/');
            return path + "lua/" + name + ".lua";
        }

        public static void Log(string str)
        {
            Debug.Log(str);
        }

        public static void LogWarning(string str)
        {
            Debug.LogWarning(str);
        }

        public static void LogError(string str)
        {
            Debug.LogError(str);
        }

        /// <summary>
        /// 防止初学者不按步骤来操作
        /// </summary>
        /// <returns></returns>
        public static int CheckRuntimeFile()
        {
            if (!Application.isEditor) return 0;
            string streamDir = Application.dataPath + "/StreamingAssets/";
            if (!Directory.Exists(streamDir))
            {
                return -1;
            }
            else
            {
                string[] files = Directory.GetFiles(streamDir);
                if (files.Length == 0) return -1;

                if (!File.Exists(streamDir + "files.txt"))
                {
                    return -1;
                }
            }
            string sourceDir = Application.dataPath + "/Source/LuaWrap/";
            if (!Directory.Exists(sourceDir))
            {
                return -2;
            }
            else
            {
                string[] files = Directory.GetFiles(sourceDir);
                if (files.Length == 0) return -2;
            }
            return 0;
        }

        /// <summary>
        /// 执行Lua方法
        /// </summary>
        public static object[] CallMethod(string module, string func, params object[] args)
        {
            LuaScriptMgr luaMgr = Ctx.m_instance.m_luaMgr;
            if (luaMgr == null) return null;
            string funcName = "";
            if(String.IsNullOrEmpty(module))    // 如果在 _G 表中
            {
                funcName = func;
            }
            else    // 在一个 _G 的一个表中
            {
                funcName = module + "." + func;
            }
            // funcName = funcName.Replace("(Clone)", "");
            return luaMgr.CallLuaFunction(funcName, args);
        }
    }
}