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

        // 异步加载对象
        override public void asyncLoad(string path, MAction<IDispatchObject> dispObj)
        {
            this.setPath(path);

            if (this.isInvalid())
            {
                unload();
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(null, dispObj);

                LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                param.setPath(string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathScene], mPath));
                param.m_loadEventHandle = onLevelLoaded;
                param.m_resNeedCoroutine = true;
                param.m_loadNeedCoroutine = true;
                Ctx.m_instance.m_resLoadMgr.loadLevel(param);
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
            Ctx.m_instance.m_resLoadMgr.unload(mLevelResItem.getResUniqueId(), null);
            UtilApi.UnloadUnusedAssets();           // 卸载共享资源
            base.unload();
        }
    }
}