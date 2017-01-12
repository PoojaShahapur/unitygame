using UnityEngine;

namespace SDK.Lib
{
    public class AuxAssetBundleManifestLoader : AuxLoaderBase
    {
        protected ResItem mResItem;
        protected AssetBundleManifest mAssetBundleManifest;

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
            return this.mAssetBundleManifest;
        }

        // 同步加载
        override public void syncLoad(string path, MAction<IDispatchObject> evtHandle = null)
        {
            base.syncLoad(path, evtHandle);

            if (this.isInvalid())
            {
                LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
                param.setPath(path);
                param.mLoadEventHandle = this.onLoadEventHandle;

                param.mLoadNeedCoroutine = false;
                param.mResNeedCoroutine = false;

                Ctx.mInstance.mResLoadMgr.loadAsset(param, false);
                Ctx.mInstance.mPoolSys.deleteObj(param);
            }
            else if (this.hasLoadEnd())
            {
                this.onLoadEventHandle(this.mResItem);
            }
        }

        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            if (null != dispObj)
            {
                this.mResItem = dispObj as ResItem;

                if (this.mResItem.hasSuccessLoaded())
                {
                    this.mResLoadState.setSuccessLoaded();

                    // 从 AssetBundle 中获取名字 AssetBundleManifest
                    this.mAssetBundleManifest = mResItem.getObject("AssetBundleManifest") as AssetBundleManifest;
                }
                else if (this.mResItem.hasFailed())
                {
                    this.mResLoadState.setFailed();
                }
            }

            // 卸载资源，AssetBundles 现在只有不使用的时候一次性全部卸载掉，如果在不使用 AssetBundleManifest 之前就卸载对应的 AssetBundles ，这个 AssetBundleManifest 就会变成 null 的
            //Ctx.mInstance.mResLoadMgr.unload(res.getResUniqueId(), onLoadEventHandle);

            if (this.mEvtHandle != null)
            {
                this.mEvtHandle.dispatchEvent(this);
            }
        }

        override public void unload()
        {
            if (this.mResItem != null)
            {
                Ctx.mInstance.mResLoadMgr.unload(mResItem.getResUniqueId(), onLoadEventHandle);
                this.mResItem = null;
            }

            base.unload();
        }
    }
}