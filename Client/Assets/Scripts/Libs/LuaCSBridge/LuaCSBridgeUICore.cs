using LuaInterface;
using System;
using System.Collections.Specialized;

namespace SDK.Lib
{
    /**
     * @brief 逻辑 Lua 和 CS 之间的交互，是以表为单位进行交互的，一定要注意表名正确，每一个表就是一个功能
     */
    public class LuaCSBridgeUICore : LuaCSBridge
    {
        protected UIAttrSystem m_uiAttrs;

        public LuaCSBridgeUICore(UIAttrSystem uiAttrs)
            : base("")
        {
            m_uiAttrs = uiAttrs;
        }

        public void loadLuaCfg()
        {
            // 首先读取 UIFOrmID 表
            LuaTable idTable = Ctx.m_instance.m_luaScriptMgr.GetLuaTable("UIFormID");
            ListDictionary idList = Ctx.m_instance.m_luaScriptMgr.lua.GetTableDict(idTable);
            LuaTable luaAttrsTable = Ctx.m_instance.m_luaScriptMgr.GetLuaTable("UIAttrs");
            LuaTable luaAttrsItemTable = null;
            int id = 0;
            UIAttrItem attrItem;
            foreach(double value in idList.Values)
            //foreach (string key in idList.Keys)
            {
                id = Convert.ToInt32(value);
                //id = Convert.ToInt32(idList[key]);
                attrItem = new UIAttrItem();
                m_uiAttrs.m_dicAttr[(UIFormID)id] = attrItem;
                luaAttrsItemTable = luaAttrsTable[id] as LuaTable;
                //luaAttrsItemTable = luaAttrsTable[key] as LuaTable;

                attrItem.m_bNeedLua = true;
                attrItem.m_widgetPath = luaAttrsItemTable["m_widgetPath"] as string;
                attrItem.m_luaScriptPath = luaAttrsItemTable["m_luaScriptPath"] as string;
                attrItem.m_luaScriptTableName = luaAttrsItemTable["m_luaScriptTableName"] as string;
            }
        }
    }
}