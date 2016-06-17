namespace SDK.Lib
{
    /**
     * @brief 关卡加载
     */
    public class AuxLevelLoader : AuxLoaderBase
    {
        protected LevelResItem mLevelResItem;

        public AuxLevelLoader()
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

                LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                param.setPath(string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathScene], mPath));
                param.m_loadEventHandle = onLevelLoaded;
                param.m_resNeedCoroutine = false;
                param.m_loadNeedCoroutine = false;
                Ctx.m_instance.m_resLoadMgr.loadAsset(param);
                Ctx.m_instance.m_poolSys.deleteObj(param);
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

                LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                param.setPath(string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathScene], mPath));
                param.m_loadEventHandle = onLevelLoaded;
                param.m_resNeedCoroutine = true;
                param.m_loadNeedCoroutine = true;
                Ctx.m_instance.m_resLoadMgr.loadAsset(param);
                Ctx.m_instance.m_poolSys.deleteObj(param);
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
                Ctx.m_instance.m_resLoadMgr.unload(mLevelResItem.getResUniqueId(), null);
                base.unload();
            }
        }
    }
}