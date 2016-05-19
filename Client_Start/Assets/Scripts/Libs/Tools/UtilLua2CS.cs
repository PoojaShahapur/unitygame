using LuaInterface;
using System;

namespace SDK.Lib
{
    public class UtilLua2CS
    {
        static public int getTableAttrInt(LuaTable luaTable, string name)
        {
            return Convert.ToInt32(luaTable[name]);
        }

        static public string getTableAttrStr(LuaTable luaTable, string name)
        {
            return (string)(luaTable[name]);
        }
    }
}