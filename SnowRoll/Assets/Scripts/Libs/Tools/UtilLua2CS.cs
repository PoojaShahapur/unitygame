using LuaInterface;
using System;

namespace SDK.Lib
{
    public class UtilLua2CS
    {
        static public int getTableAttrInt(LuaTable luaTable, string name)
        {
            int ret = 0;
            object obj = luaTable[name];
            if (obj != null)
            {
                ret = Convert.ToInt32(obj);
            }

            return ret;
        }

        static public uint getTableAttrUInt(LuaTable luaTable, string name)
        {
            uint ret = 0;

            object obj = luaTable[name];
            if (obj != null)
            {
                ret = Convert.ToUInt32(obj);
            }

            return ret;
        }

        static public string getTableAttrStr(LuaTable luaTable, string name)
        {
            string ret = "";
            object obj = luaTable[name];
            if (obj != null)
            {
                ret = (string)(luaTable[name]);
            }

            return ret;
        }
    }
}