using SDK.Common;
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
            m_resItem.increaseRef();

            if (m_resItem.HasLoaded())   // 如果已经加载
            {
                if (m_resItem.isSucceed)     // 如果已经成功
                {
                    onPakResLoaded(null);
                }
                else
                {
                    onPakResFailed(null);
                }
            }
            else
            {
                resItem.addEventListener(EventID.LOADED_EVENT, onPakResLoaded);
                resItem.addEventListener(EventID.FAILED_EVENT, onPakResFailed);
            }
        }

        override public void unload()
        {
            base.unload();

            if (m_resItem != null)
            {
                m_resItem.decreaseRef();
            }
        }

        virtual public void initByPakRes()
        {
            base.init(null);
        }

        protected void onPakResLoaded(IDispatchObject res)
        {
            initByPakRes();
        }

        protected void onPakResFailed(IDispatchObject res)
        {
            if (onFailed != null)
            {
                onFailed(this);
            }

            clearListener();
        }
    }
}