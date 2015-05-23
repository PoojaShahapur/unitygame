﻿using SDK.Common;
using System.Collections;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 打包的资源系统 base
     */
    public class ABMemUnPakFileResItemBase : ABUnPakFileResItemBase
    {
        protected ABPakFileResItemBase m_resItem = null;

        public void setPakRes(ABPakFileResItemBase resItem)
        {
            m_resItem = resItem;
            m_resItem.refCount.incRef();

            if (m_resItem.hasLoaded())   // 如果已经加载
            {
                onPakResLoadEventHandle(null);
            }
            else
            {
                resItem.loadEventDispatch.addEventHandle(onPakResLoadEventHandle);
            }
        }

        override public void unload()
        {
            base.unload();

            if (m_resItem != null)
            {
                m_resItem.refCount.decRef();
            }
        }

        virtual public void initByPakRes()
        {
            base.init(null);
        }

        protected void onPakResLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            if (res.hasSuccessLoaded())
            {
                initByPakRes();
            }
            else if(res.hasFailed())
            {
                m_loadEventDispatch.dispatchEvent(this);
                clearListener();
            }
        }
    }
}