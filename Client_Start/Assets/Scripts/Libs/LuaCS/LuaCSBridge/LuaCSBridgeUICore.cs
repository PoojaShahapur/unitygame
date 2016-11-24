using LuaInterface;
using System;
using System.Collections;
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
            : base("", "")
        {
            m_uiAttrs = uiAttrs;
        }

        public void loadLuaCfg_bak()
        {
            //// 首先读取 UIFormID 表
            //LuaTable idTable = Ctx.mInstance.mLuaSystem.GetLuaTable("GlobalNS.UIFormID");
            ////ListDictionary idList = Ctx.mInstance.mLuaSystem.lua.GetTableDict(idTable);
            //ListDictionary idList = Ctx.mInstance.mLuaSystem.lua.GetTableDict(idTable);
            //LuaTable luaAttrsTable = Ctx.mInstance.mLuaSystem.GetLuaTable("GlobalNS.UIAttrSystem");
            //LuaTable luaAttrsItemTable = null;
            //int id = 0;
            //UIAttrItem attrItem;
            //foreach(double value in idList.Values)
            ////foreach (string key in idList.Keys)
            //{
            //    id = Convert.ToInt32(value);
            //    //id = Convert.ToInt32(idList[key]);
            //    attrItem = new UIAttrItem();
            //    m_uiAttrs.mId2AttrDic[(UIFormID)id] = attrItem;
            //    luaAttrsItemTable = luaAttrsTable[id] as LuaTable;
            //    //luaAttrsItemTable = luaAttrsTable[key] as LuaTable;

            //    attrItem.m_bNeedLua = true;
            //    attrItem.m_widgetPath = luaAttrsItemTable["m_widgetPath"] as string;
            //    attrItem.m_luaScriptPath = luaAttrsItemTable["m_luaScriptPath"] as string;
            //    attrItem.m_luaScriptTableName = luaAttrsItemTable["m_luaScriptTableName"] as string;
            //}

            // 新版本
            // 首先读取 UIFormID 表
            LuaTable idTable = Ctx.mInstance.mLuaSystem.getLuaTable("GlobalNS.UIFormID");
            System.Collections.Generic.IEnumerator<DictionaryEntry> idList = idTable.ToDictTable().GetEnumerator();
            LuaTable luaAttrsTable = Ctx.mInstance.mLuaSystem.getLuaTable("GlobalNS.UIAttrSystem");
            LuaTable luaAttrsItemTable = null;
            int id = 0;
            UIAttrItem attrItem = null;
            while (idList.MoveNext())
            {
                id = Convert.ToInt32(idList.Current.Value);
                attrItem = new UIAttrItem();
                m_uiAttrs.mId2AttrDic[(UIFormID)id] = attrItem;
                luaAttrsItemTable = luaAttrsTable[id] as LuaTable;

                attrItem.m_bNeedLua = true;
                attrItem.m_widgetPath = luaAttrsItemTable["m_widgetPath"] as string;
                attrItem.m_luaScriptPath = luaAttrsItemTable["m_luaScriptPath"] as string;
                attrItem.m_luaScriptTableName = luaAttrsItemTable["m_luaScriptTableName"] as string;
            }

            idList.Dispose();
        }

        public void loadLuaCfg()
        {
            // 首先读取 UIFormID 表
            LuaTable idTable = Ctx.mInstance.mLuaSystem.getLuaTable("GlobalNS.UIFormID");
            //IDictionaryEnumerator idTableEnum =  idTable.GetEnumerator();
            System.Collections.Generic.IEnumerator<DictionaryEntry> idTableEnum = idTable.ToDictTable().GetEnumerator();
            idTableEnum.Reset();

            LuaTable luaAttrsTable = Ctx.mInstance.mLuaSystem.getLuaTable("GlobalNS.UIAttrSystem");
            LuaTable luaAttrsItemTable = null;
            int id = 0;
            UIAttrItem attrItem;
            while(idTableEnum.MoveNext())
            {
                if(Ctx.mInstance.mLuaSystem.isSystemAttr((string)idTableEnum.Current.Key))
                {
                    continue;
                }
                id = Convert.ToInt32(idTableEnum.Current.Value);
                luaAttrsItemTable = luaAttrsTable[id] as LuaTable;

                // 有个 eUICount 是个数，这个是没有 FormId 的
                if (luaAttrsItemTable != null)
                {
                    attrItem = new UIAttrItem();
                    m_uiAttrs.mId2AttrDic[(UIFormID)id] = attrItem;

                    attrItem.m_bNeedLua = true;
                    attrItem.m_widgetPath = luaAttrsItemTable["m_widgetPath"] as string;
                    attrItem.m_luaScriptPath = luaAttrsItemTable["m_luaScriptPath"] as string;
                    attrItem.m_luaScriptTableName = luaAttrsItemTable["m_luaScriptTableName"] as string;
                }
            }
        }
    }
}