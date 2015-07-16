using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using SDK.Common;

namespace SDK.Lib
{
    public class LuaCSBridge
    {
        protected string m_tableName;   // 表的名字
        protected GameObject m_gameObject;  // 测试绑定 UnitEngine 对象

        /**
         * @brief 表的名字
         */
        public LuaCSBridge(string tableName)
        {
            m_tableName = tableName;
        }

        protected void Start()
        {
            LuaScriptMgr _sluaManager = Ctx.m_instance.m_luaMgr;
            LuaState _luaState = _sluaManager.lua;
            _luaState[m_tableName + ".gameObject"] = m_gameObject;
        }

        protected void OnClick()
        {
            CallMethod("OnClick");
        }

        protected void OnClickEvent(GameObject go)
        {
            CallMethod("OnClick", go);
        }

        /// <summary>
        /// 添加单击事件
        /// </summary>
        public void AddClick(GameObject go, LuaFunction luafunc)
        {
            if (go == null) return;
            //buttons.Add(luafunc);
            go.GetComponent<Button>().onClick.AddListener(
                delegate()
                {
                    luafunc.Call(go);
                }
            );
        }

        /// <summary>
        /// 执行Lua方法
        /// </summary>
        protected object[] CallMethod(string func, params object[] args)
        {
            return Util.CallMethod(m_tableName, func, args);
        }

        //-----------------------------------------------------------------
        public void dispose()
        {
            Util.ClearMemory();
            Ctx.m_instance.m_logSys.log(string.Format("~ {0} was destroy!", m_tableName));
        }
    }
}