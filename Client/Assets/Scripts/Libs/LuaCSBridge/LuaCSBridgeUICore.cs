using LuaInterface;
using SDK.Lib;
using System.Collections.Specialized;

namespace SDK.Lib
{
    /**
     * @brief 逻辑 Lua 和 CS 之间的交互，是以表为单位进行交互的，一定要注意表名正确，每一个表就是一个功能
     */
    public class LuaCSBridgeUICore : LuaCSBridge
    {
        protected UIAttrs m_uiAttrs;

        public LuaCSBridgeUICore(UIAttrs uiAttrs)
            : base("")
        {
            m_uiAttrs = uiAttrs;
        }

        public void loadLuaCfg()
        {
            // 首先读取 UIFOrmID 表
            LuaTable idTable = Ctx.m_instance.m_luaMgr.GetLuaTable("UIFormID");
            ListDictionary idList = Ctx.m_instance.m_luaMgr.lua.GetTableDict(idTable);
            LuaTable luaAttrsTable = Ctx.m_instance.m_luaMgr.GetLuaTable("UIAttrs");
            LuaTable luaAttrsItemTable = null;
            int id = 0;
            UIAttrItem attrItem;
            foreach(double value in idList.Values)
            {
                id = (int)value;
                attrItem = new UIAttrItem();
                m_uiAttrs.m_dicAttr[(UIFormID)id] = attrItem;
                luaAttrsItemTable = luaAttrsItemTable[id] as LuaTable;

                attrItem.m_bNeedLua = true;
                attrItem.m_widgetPath = luaAttrsItemTable["m_widgetPath"] as string;
                attrItem.m_luaScriptPath = luaAttrsItemTable["m_luaScriptPath"] as string;
                attrItem.m_luaScriptTableName = luaAttrsItemTable["m_luaScriptTableName"] as string;
            }
        }
    }
}