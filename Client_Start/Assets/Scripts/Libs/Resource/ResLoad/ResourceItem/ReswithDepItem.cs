namespace SDK.Lib
{
    public class ReswithDepItem : ResItem
    {
        protected ResAndDepItem mResAndDepItem;
        protected bool mLoadNeedCoroutine;
        protected bool mIsCheckDep;

        public ReswithDepItem()
        {
            mIsCheckDep = false;
        }

        override public void init(LoadItem item)
        {
            base.init(item);

            if (!this.mIsCheckDep || !hasDep())  // 如果不检查依赖，或者没有依赖，直接进入加载完成
            {
                onResLoaded();
            }
            else
            {
                loadDep();
            }
        }

        override public void reset()
        {
            base.reset();

            mResAndDepItem = null;
            mLoadNeedCoroutine = false;
            mIsCheckDep = false;
        }

        protected bool hasDep()
        {
            return Ctx.mInstance.mDepResMgr.hasDep(this.mLoadPath);
        }

        // 如果有依赖返回 true，没有就返回 false
        protected void loadDep()
        {
            // 这个地方只有当资源从来没有加载过的时候，才会加载一次，如果已经加载不会再次进来，ResItem 的引用计数可能是多个，但是 ResItem 的 mResAndDepItem 中资源的引用计数就是 1，只有当 ResItem 卸载的时候，才会卸载 mResAndDepItem
            if (mResAndDepItem == null)
            {
                mResAndDepItem = new ResAndDepItem();
            }

            mResAndDepItem.addEventHandle(onDepResLoaded);
            mResAndDepItem.mLoadPath = this.mLoadPath;
            mResAndDepItem.mLoadNeedCoroutine = this.mLoadNeedCoroutine;
            mResAndDepItem.mResNeedCoroutine = this.mResNeedCoroutine;
            mResAndDepItem.loadDep();
        }

        virtual protected void onResLoaded()
        {

        }

        // 依赖的资源加载完成
        protected void onDepResLoaded(IDispatchObject dispObj)
        {
            onResLoaded();
        }

        override public void unload(bool unloadAllLoadedObjects = true)
        {
            if (mResAndDepItem != null)
            {
                mResAndDepItem.unloadDep();
                mResAndDepItem = null;
            }

            base.unload(unloadAllLoadedObjects);
        }

        override public void setLoadParam(LoadParam param)
        {
            base.setLoadParam(param);

            this.mLoadNeedCoroutine = param.mLoadNeedCoroutine;
            this.mIsCheckDep = param.mIsCheckDep;
        }
    }
}