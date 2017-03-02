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
            m_resItem.refCountResLoadResultNotify.refCount.incRef();

            if (m_resItem.refCountResLoadResultNotify.resLoadState.hasLoaded())   // 如果已经加载
            {
                onPakResLoadEventHandle(null);
            }
            else
            {
                resItem.refCountResLoadResultNotify.loadResEventDispatch.addEventHandle(null, onPakResLoadEventHandle);
            }
        }

        override public void unload(bool unloadAllLoadedObjects = true)
        {
            if (m_resItem != null)
            {
                m_resItem.refCountResLoadResultNotify.refCount.decRef();
                m_resItem = null;
            }

            base.unload(unloadAllLoadedObjects);
        }

        virtual public void initByPakRes()
        {
            base.init(null);
        }

        protected void onPakResLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                initByPakRes();
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                mRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
            }
        }
    }
}