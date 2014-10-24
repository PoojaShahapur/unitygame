﻿using SDK.Common;
using System.Collections.Generic;

/**
 * @brief 心跳管理器
 */
namespace SDK.Lib
{
    class TickMgr : ITickMgr
    {
        protected List<ITick> m_TickLst;

        public TickMgr()
        {
            m_TickLst = new List<ITick>();
        }

        public void Advance(float delta)
        {
            foreach (ITick tk in m_TickLst)
            {
                tk.OnTick(delta);
            }
        }
    }
}
