using LuaInterface;

namespace SDK.Lib
{
    /**
     * @brief Lua 调用 CS 接口
     */
    public class LuaToCS
    {
        static public void onTestProtoBuf(LuaTable cmd)
        {
            int requid = UtilLua2CS.getTableAttrInt(cmd, "requid");
            int reqguid = UtilLua2CS.getTableAttrInt(cmd, "reqguid");
            string reqaccount = UtilLua2CS.getTableAttrStr(cmd, "reqaccount");
        }
    }
}