using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Reflection;
using System.IO;

public class Util {
    /// <summary>
    /// 取得Lua路径
    /// </summary>
    public static string LuaPath(string name) {
        string path = Application.dataPath + "/";
        string lowerName = name.ToLower();
        if (lowerName.EndsWith(".lua")) {
			if (File.Exists(path + "Prefabs/Resources/" + name))
			{
				return path + "Prefabs/Resources/" + name;
			}
			else if (File.Exists(path + "Prefabs/Resources/LuaScript/LuaLib/" + name))
			{
				return path + "Prefabs/Resources/LuaScript/LuaLib/" + name;
			}
			else
			{
				return path + "lua/" + name;
			}
        }
        
		if (File.Exists(path + "Prefabs/Resources/" + name + ".lua"))
		{
			return path + "Prefabs/Resources/" + name + ".lua";
		}
		else if (File.Exists(path + "Prefabs/Resources/LuaScript/LuaLib/" + name + ".lua"))
		{
			return path + "Prefabs/Resources/LuaScript/LuaLib/" + name + ".lua";
		}
		else
		{
			return path + "lua/" + name + ".lua";
		}
    }

    public static void Log(string str) {
        Debug.Log(str);
    }

    public static void LogWarning(string str) {
        Debug.LogWarning(str);
    }

    public static void LogError(string str) {
        Debug.LogError(str); 
    }
}