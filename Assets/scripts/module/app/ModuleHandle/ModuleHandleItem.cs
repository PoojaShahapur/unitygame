using SDK.Common;
using System;

namespace Game.App
{
    /**
     * @brief 单一模块处理 Item 
     */
    public class ModuleHandleItem
    {
        public Action<EventDisp> m_loadedcb;
        public string m_key;
    }
}