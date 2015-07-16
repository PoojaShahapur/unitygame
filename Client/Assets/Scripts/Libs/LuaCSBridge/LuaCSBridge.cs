using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 逻辑 Lua 和 CS 之间的交互，是以表为单位进行交互的，一定要注意表名正确
     */
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
            Ctx.m_instance.m_luaMgr.lua[m_tableName + ".gameObject"] = m_gameObject;
        }

        public void dispose()
        {
            Util.ClearMemory();
            Ctx.m_instance.m_logSys.log(string.Format("~ {0} was destroy!", m_tableName));
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

        // 直接从 Lua 脚本添加函数或者变量到表中，执行后不会有任何返回值，不知道为什么
        public object[] DoFile(string fileName)
        {
            return Ctx.m_instance.m_luaMgr.lua.DoFile(fileName);
        }

        /**
         * @brief 执行Lua方法
         * @brief funcName_ 函数名字
         */
        public object[] CallMethod(string funcName_, params object[] args)
        {
            string fullFuncName = "";               // 完全的有表的完全名字
            if (String.IsNullOrEmpty(m_tableName))  // 如果在 _G 表中
            {
                fullFuncName = funcName_;
            }
            else    // 在一个 _G 的一个表中
            {
                fullFuncName = m_tableName + "." + funcName_;
            }
            return Ctx.m_instance.m_luaMgr.CallLuaFunction(fullFuncName, args);
        }

        /**
         * @brief 获取 Lua 表中的数据
         * @param member_ 表中成员的名字
         */
        public object GetMember(string memberName_)
        {
            string fullMemberName = "";             // 有表前缀的成员的名字
            if (String.IsNullOrEmpty(m_tableName))  // 如果在 _G 表中
            {
                fullMemberName = memberName_;
            }
            else    // 在一个 _G 的一个表中
            {
                fullMemberName = m_tableName + "." + memberName_;
            }

            return Ctx.m_instance.m_luaMgr.lua[fullMemberName];
        }
    }
}