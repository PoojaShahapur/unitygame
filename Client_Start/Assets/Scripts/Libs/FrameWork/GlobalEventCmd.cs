using LuaInterface;

namespace SDK.Lib
{
    public class GlobalEventCmd
    {
        static public void onTestProtoBuf(LuaTable cmd)
        {
            int requid = UtilLua2CS.getTableAttrInt(cmd, "requid");
            int reqguid = UtilLua2CS.getTableAttrInt(cmd, "reqguid");
            string reqaccount = UtilLua2CS.getTableAttrStr(cmd, "reqaccount");
        }
    }
}