using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 支持从本地和 web 服务器加载自己手工打包的 Bundle 类型，但是不包括场景资源打包成 Bundle
     */
    public class BundleLoadItem : LoadItem
    {
        override public void load()
        {
            base.load();

            if (ResLoadType.eLoadStreamingAssets == mResLoadType ||
                ResLoadType.eLoadLocalPersistentData == mResLoadType)
            {
                if (mLoadNeedCoroutine)
                {
                    // 如果有协程的直接这么调用，编辑器会卡死
                    //loadFromAssetBundleByCoroutine()
                    Ctx.mInstance.mCoroutineMgr.StartCoroutine(loadFromAssetBundleByCoroutine());
                }
                else
                {
                    loadFromAssetBundle();
                }
            }
            else if (ResLoadType.eLoadWeb == mResLoadType)
            {
                Ctx.mInstance.mCoroutineMgr.StartCoroutine(downloadAsset());
            }
        }

        // 这个是卸载，因为有时候资源加载进来可能已经不用了，需要直接卸载掉
        override public void unload()
        {
            if (m_assetBundle != null)
            {
                m_assetBundle.Unload(true);
                m_assetBundle = null;
            }
            base.unload();
        }

        // CreateFromFile(注意这种方法只能用于standalone程序）这是最快的加载方法
        // AssetBundle.CreateFromFile 这个函数仅支持未压缩的资源。这是加载资产包的最快方式。自己被这个函数坑了好几次，一定是非压缩的资源，如果压缩式不能加载的，加载后，内容也是空的。目前这个接口各个平台都支持了，包括 Android 和 Mac、Iphone
        protected IEnumerator loadFromAssetBundleByCoroutine()
        {
            string path = "";
            // UNITY_5_2 没有
            //AssetBundleCreateRequest req = null;

#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            //byte[] bytes = Ctx.mInstance.m_fileSys.LoadFileByte(path);
            //req = AssetBundle.CreateFromMemory(bytes);
            //yield return req;

            // UNITY_5_2 没有异步从文件加载的 LoadFromFileAsync 接口，只有从内存异步加载的 CreateFromMemory 接口，因此直接使用 WWW 读取，就不先从文件系统将二进制读取进来，然后再调用 CreateFromMemory 了，不知道 WWW 和 从文件系统读取二进制再 CreateFromMemory 哪个更快。
            WWW www = null;
            path = ResPathResolve.msFileLoadRootPathList[(int)mResLoadType] + "/" + mLoadPath;
            www = new WWW(path);
            yield return www;
            m_assetBundle = www.assetBundle;

            www.Dispose();
            www = null;
#else
            AssetBundleCreateRequest req = null;

            path = ResPathResolve.msABLoadRootPathList[(int)mResLoadType] + "/" + mLoadPath;
                        
            req = AssetBundle.LoadFromFileAsync(path);
            yield return req;

            m_assetBundle = req.assetBundle;
#endif

            assetBundleLoaded();
        }

        protected void loadFromAssetBundle()
        {
            string path;
            path = ResPathResolve.msABLoadRootPathList[(int)mResLoadType] + "/" + mLoadPath;
            // UNITY_5_2 没有
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            m_assetBundle = AssetBundle.CreateFromFile(path);
#else
            m_assetBundle = AssetBundle.LoadFromFile(path);
#endif
            assetBundleLoaded();
        }

        protected void assetBundleLoaded()
        {
            if (m_assetBundle != null)
            {
                m_nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                m_nonRefCountResLoadResultNotify.resLoadState.setFailed();
            }

            m_nonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }
    }
}