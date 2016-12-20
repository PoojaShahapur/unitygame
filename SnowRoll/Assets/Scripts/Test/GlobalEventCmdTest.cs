using LuaInterface;
using msg;
using SDK.Lib;

namespace UnitTest
{
    public class GlobalEventCmdTest
    {
        static public void onTestProtoBuf(LuaTable cmd)
        {
            int requid = UtilLua2CS.getTableAttrInt(cmd, "requid");
            int reqguid = UtilLua2CS.getTableAttrInt(cmd, "reqguid");
            string reqaccount = UtilLua2CS.getTableAttrStr(cmd, "reqaccount");
        }

        //static public void onTestProtoBufBuffer(ushort commandID, LuaStringBuffer buffer)
        static public void onTestProtoBufBuffer(ushort commandID, LuaInterface.LuaByteBuffer buffer)
        {
            MSG_ReqTest msg = ProtobufUtil.DeSerializeFBytes<MSG_ReqTest>(buffer.buffer);
        }
    }
}