namespace SDK.Lib
{
    /**
     * @brief 关卡加载
     */
    public class AuxLevelLoader : AuxLoaderBase
    {
        protected LevelResItem mLevelResItem;

        public AuxLevelLoader(string path = "")
            : base(path)
        {
            
        }

        override public void dispose()
        {
            base.dispose();
        }

        override public void syncLoad(string path, MAction<IDispatchObject> evtHandle = null)
        {
            if (needUnload(path))
            {
                unload();
            }

            this.setPath(path);

            if (this.isInvalid())
            {
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(null, evtHandle);

                LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
                param.setPath(string.Format("{0}{1}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathScene], mPath));
                param.mLoadEventHandle = onLevelLoaded;
                param.mResNeedCoroutine = false;
                param.mLoadNeedCoroutine = false;
                Ctx.mInstance.mResLoadMgr.loadAsset(param);
                Ctx.mInstance.mPoolSys.deleteObj(param);

                this.mLevelResItem = Ctx.mInstance.mResLoadMgr.getResource(param.mResUniqueId) as LevelResItem;
                this.onLevelLoaded(this.mLevelResItem);
            }
        }

        // 异步加载对象
        override public void asyncLoad(string path, MAction<IDispatchObject> evtHandle)
        {
            if (needUnload(path))
            {
                unload();
            }

            this.setPath(path);

            if (this.isInvalid())
            {
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(null, evtHandle);

                LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
                param.setPath(string.Format("{0}{1}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathScene], mPath));
                param.mLoadEventHandle = onLevelLoaded;
                param.mResNeedCoroutine = true;
                param.mLoadNeedCoroutine = true;
                Ctx.mInstance.mResLoadMgr.loadAsset(param);
                Ctx.mInstance.mPoolSys.deleteObj(param);
            }
        }

        public void onLevelLoaded(IDispatchObject dispObj)
        {
            mLevelResItem = dispObj as LevelResItem;
            if (mLevelResItem.hasSuccessLoaded())
            {
                mIsSuccess = true;
            }
            else if (mLevelResItem.hasFailed())
            {
                mIsSuccess = false;
            }

            if (mEvtHandle != null)
            {
                mEvtHandle.dispatchEvent(this);
            }
        }

        override public void unload()
        {
            if (mLevelResItem != null)
            {
                Ctx.mInstance.mResLoadMgr.unload(mLevelResItem.getResUniqueId(), null);
                base.unload();
            }
        }
    }
}