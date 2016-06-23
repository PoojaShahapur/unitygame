using UnityEngine;

namespace SDK.Lib
{
    public class AuxAssetBundleManifestLoader : AuxLoaderBase
    {
        protected ResItem mResItem;
        protected AssetBundleManifest m_AssetBundleManifest;

        public AuxAssetBundleManifestLoader(string path = "")
            : base(path)
        {
            mResItem = null;
        }

        override public void dispose()
        {
            base.dispose();
        }

        public AssetBundleManifest getAssetBundleManifest()
        {
            return m_AssetBundleManifest;
        }

        // 同步加载
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
                param.setPath(path);
                param.m_loadEventHandle = onLoadEventHandle;

                param.m_loadNeedCoroutine = false;
                param.m_resNeedCoroutine = false;

                Ctx.m_instance.m_resLoadMgr.loadAsset(param, false);
                Ctx.m_instance.m_poolSys.deleteObj(param);
            }
        }

        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            mResItem = dispObj as ResItem;

            if (mResItem.hasSuccessLoaded())
            {
                mIsSuccess = true;

                // 从 AssetBundle 中获取名字 AssetBundleManifest
                m_AssetBundleManifest = mResItem.getObject("AssetBundleManifest") as AssetBundleManifest;
            }
            else if (mResItem.hasFailed())
            {
                mIsSuccess = false;

                Ctx.m_instance.m_logSys.log("AssetBundleManifest AssetBundles Can not Load", LogTypeId.eLogCommon);
            }

            // 卸载资源，AssetBundles 现在只有不使用的时候一次性全部卸载掉，如果在不使用 AssetBundleManifest 之前就卸载对应的 AssetBundles ，这个 AssetBundleManifest 就会变成 null 的
            //Ctx.m_instance.m_resLoadMgr.unload(res.getResUniqueId(), onLoadEventHandle);

            if (mEvtHandle != null)
            {
                mEvtHandle.dispatchEvent(this);
            }
        }

        override public void unload()
        {
            if (mResItem != null)
            {
                Ctx.m_instance.m_resLoadMgr.unload(mResItem.getResUniqueId(), onLoadEventHandle);
                mResItem = null;
            }

            base.unload();
        }
    }
}