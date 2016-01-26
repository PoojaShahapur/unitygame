using LuaInterface;
using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief Form 交换信息
     */
    public class LuaCSBridgeForm : LuaCSBridge
    {
        public const string LUA_DISPATCH_TABLE_NAME = "UIManager";          // 接收全局事件的 Lua 表的名字
        public const string LUA_DISPATCH_FUNC_NAME = "OnUiMsg";             // 处理全局事件的函数名字
        public const string LUA_DISPATCH_FULL_FUNC_NAME = "UIManager.OnUiMsg";  // 包括表名字的函数

        public const string ON_INIT = "onInit";             // 代码构造完成调用
        public const string ON_READY = "onReady";           // 资源加载完成调用脚本，只调用一次
        public const string ON_SHOW = "onShow";             // 每一次显示都调用一次
        public const string ON_HIDE = "onHide";             // 每一次隐藏都调用一次
        public const string ON_EXIT = "onExit";             // 退出调用一次
        public const string ON_BTN_CLK = "onBtnClk";        // 按钮点击调用

        // 表的名字说明
        public const string BTN_CLICK_TABLE = "BtnClickTable";      // BtnClickTable 按钮点击事件表

        protected GameObject m_gameObject;  // 测试绑定 UnitEngine 对象
        protected UIAttrItem m_attrItem;    // Form 基本配置
        protected Form m_form;              // CS 中的 Form
        protected LuaTable m_luaInsTable;      // Lua 中的 Form 实例表

        public LuaCSBridgeForm(UIAttrItem attrItem, Form form_)
            : base(attrItem.m_luaScriptPath, attrItem.m_luaScriptTableName)
        {
            m_attrItem = attrItem;
            m_form = form_;
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

        // 加载表并且实例化表
        override public void init()
        {
            base.init();
            // 现在 new 移动到一个另外一个文件中了
            //m_luaInsTable = base.CallClassMethod("new")[0] as LuaTable;
            m_luaInsTable = Ctx.m_instance.m_luaSystem.m_luaCSBridgeMalloc.malloc(m_luaTable);
        }

        // 资源加载完成初始化
        public void postInit()
        {
            Ctx.m_instance.m_luaSystem.lua[m_tableName + ".gameObject"] = m_gameObject;
            Ctx.m_instance.m_luaSystem.lua[m_tableName + ".transform"] = m_gameObject.transform;
            Ctx.m_instance.m_luaSystem.lua[m_tableName + ".form"] = m_form;
        }

        // 根据表注册 UI 事件， LuaTable 的格式如下 luaTable {name="Panel_Name", BtnClickTable={"ui/click", "ui/tab"} ImageClickTable={"ui/click", "ui/tab"}}
        public void registerWidgetEventByTable(LuaTable luaTable)
        {
            m_form.formName = luaTable["name"] as string;
            LuaTable btnClickTable = luaTable["BtnClickTable"] as LuaTable;

            string[] btnArray = btnClickTable.ToArray<string>();
            m_form.registerBtnClickEventByList(btnArray);

            // LuaTable imageClickTable = luaTable["ImageClickTable"] as LuaTable;
            // string[] imageArary = imageClickTable.ToArray<string>();
            // m_form.registerImageClickEventByList(imageArary);
        }

        public void handleUIEvent(string eventName, string formName, string path)
        {
            // Ctx.m_instance.m_luaSystem.DoFile("script/panelscript/UIMgr.lua");
            LuaTable luaTable = Ctx.m_instance.m_luaSystem.GetLuaTable("UIManager");
            CallGlobalMethod(LuaCSBridgeForm.LUA_DISPATCH_FULL_FUNC_NAME, luaTable, eventName, formName, path);  // 这个地方把 luaTable 传递进去，是因为 Lua 中是这么写的 UIManager:OnBtnClick ，而不是 UIManager.OnBtnClick 写的
            // CallGlobalMethod("UISysTest.OnUiMsg", "bbb", "ffff");
        }

        override public object[] CallClassMethod(string funcName_, params object[] args)
        {
            string fullFuncName = "";               // 完全的有表的完全名字
            if (!String.IsNullOrEmpty(m_tableName))  // 如果在 _G 表中
            {
                fullFuncName = m_tableName + "." + funcName_;
                return Ctx.m_instance.m_luaSystem.CallLuaFunction(fullFuncName, m_luaInsTable, args);
            }

            return null;
        }
    }
}