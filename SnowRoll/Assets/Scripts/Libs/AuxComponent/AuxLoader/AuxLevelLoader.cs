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
            base.syncLoad(path, evtHandle);

            if (this.isInvalid())
            {
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
            else if (this.hasLoadEnd())
            {
                this.onLevelLoaded(this.mLevelResItem);
            }
        }

        override public void syncLoad(string path, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction)
        {
            base.syncLoad(path, luaTable, luaFunction);

            this.updatePath(path);

            if (this.isInvalid())
            {
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
            else if (this.hasLoadEnd())
            {
                this.onLevelLoaded(this.mLevelResItem);
            }
        }

        // 异步加载对象
        override public void asyncLoad(string path, MAction<IDispatchObject> evtHandle)
        {
            base.asyncLoad(path, evtHandle);

            if (this.isInvalid())
            {
                LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
                param.setPath(string.Format("{0}{1}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathScene], mPath));
                param.mLoadEventHandle = this.onLevelLoaded;
                param.mResNeedCoroutine = true;
                param.mLoadNeedCoroutine = true;
                Ctx.mInstance.mResLoadMgr.loadAsset(param);
                Ctx.mInstance.mPoolSys.deleteObj(param);
            }
            else if (this.hasLoadEnd())
            {
                this.onLevelLoaded(this.mLevelResItem);
            }
        }

        override public void asyncLoad(string path, LuaInterface.LuaTable luaTable, LuaInterface.LuaFunction luaFunction)
        {
            base.asyncLoad(path, luaTable, luaFunction);

            this.updatePath(path);

            if (this.isInvalid())
            {
                LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
                param.setPath(string.Format("{0}{1}", Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathScene], mPath));
                param.mLoadEventHandle = this.onLevelLoaded;
                param.mResNeedCoroutine = true;
                param.mLoadNeedCoroutine = true;
                Ctx.mInstance.mResLoadMgr.loadAsset(param);
                Ctx.mInstance.mPoolSys.deleteObj(param);
            }
            else if (this.hasLoadEnd())
            {
                this.onLevelLoaded(this.mLevelResItem);
            }
        }

        public void onLevelLoaded(IDispatchObject dispObj)
        {
            if (null != dispObj)
            {
                this.mLevelResItem = dispObj as LevelResItem;

                if (this.mLevelResItem.hasSuccessLoaded())
                {
                    this.mResLoadState.setSuccessLoaded();
                }
                else if (this.mLevelResItem.hasFailed())
                {
                    this.mResLoadState.setFailed();
                }
            }

            if (this.mEvtHandle != null)
            {
                this.mEvtHandle.dispatchEvent(this);
            }
        }

        override public void unload()
        {
            if (this.mLevelResItem != null)
            {
                Ctx.mInstance.mResLoadMgr.unload(mLevelResItem.getResUniqueId(), null);
                base.unload();
            }
        }
    }
}