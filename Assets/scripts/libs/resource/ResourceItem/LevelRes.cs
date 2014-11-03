using System;
using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    public class LevelRes : Res
    {
        public LevelRes()
        {
            
        }

        override public void init(LoadItem item)
        {
            initAsset();
        }

        override public void initAsset()
        {
            if (onLoadedCB != null)
            {
                Ctx.m_instance.m_shareMgr.m_evt.m_param = this;
                onLoadedCB(Ctx.m_instance.m_shareMgr.m_evt);
            }
        }

        override public void reset()
        {
            base.reset();
        }
    }
}