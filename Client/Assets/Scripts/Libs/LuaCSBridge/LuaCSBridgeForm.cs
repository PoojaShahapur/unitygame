using LuaInterface;
using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief Form 交换信息
     */
    public class LuaCSBridgeForm : LuaCSBridge
    {
        protected GameObject m_gameObject;  // 测试绑定 UnitEngine 对象
        protected Form m_form;

        public LuaCSBridgeForm(string tableName, Form form_)
            : base(tableName)
        {

        }

        public GameObject gameObject
        {
            get
            {
                return m_gameObject;
            }
            set
            {
                m_gameObject = value;
            }
        }

        override protected void init()
        {
            base.init();
            Ctx.m_instance.m_luaMgr.lua[m_tableName + ".gameObject"] = m_gameObject;
            Ctx.m_instance.m_luaMgr.lua[m_tableName + ".transform"] = m_gameObject.transform;
            Ctx.m_instance.m_luaMgr.lua[m_tableName + ".form"] = m_form;
        }

        public void registerWidgetEventByTable(LuaTable luaTable)
        {
            m_form.formName = luaTable["name"] as string;
            LuaTable btnClickTable = luaTable["BtnClickTable"] as LuaTable;

            string[] btnArray = btnClickTable.ToArray<string>();
            m_form.registerBtnClickEventByList(btnArray);
        }

        public void handleUIEvent(string eventName, string formName, string path)
        {
            LuaTable luaTable = Ctx.m_instance.m_luaMgr.GetLuaTable("UIManager");
            CallGlobalMethod("UIManager:OnBtnClick", luaTable, eventName, formName, path);  // 这个地方把 luaTable 传递进去，是因为 Lua 中是这么写的 UIManager:OnBtnClick ，而不是 UIManager.OnBtnClick 写的
        }
    }
}