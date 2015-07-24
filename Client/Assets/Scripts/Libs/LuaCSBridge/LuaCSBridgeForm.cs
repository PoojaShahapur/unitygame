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

        override protected void init()
        {
            base.init();
            Ctx.m_instance.m_luaMgr.lua[m_tableName + ".gameObject"] = m_gameObject;
        }
    }
}