using System.Collections;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 没有打包的系统，在没有打包之前使用这个加载系统，每一个 ResItem 只有一个资源，打包的资源也是每一个 item 只有一个资源包
     */
    public class ABUnPakComFileResItem : ABMemUnPakFileResItemBase
    {
        protected Object mObject;
        protected GameObject mRetGO;       // 方便调试的临时对象

        override public void init(LoadItem item)
        {
            base.init(item);
            initByBytes((item as ABUnPakLoadItem).mBytes, PRE_PATH);
        }

        override protected void initAsset()
        {
            base.initAsset();

            if (mBundle != null)
            {
                mRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                mRefCountResLoadResultNotify.resLoadState.setFailed();
            }
            mRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        override protected IEnumerator initAssetByCoroutine()
        {
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            AssetBundleCreateRequest createReq = AssetBundle.CreateFromMemory(mBytes);
#else
            AssetBundleCreateRequest createReq = AssetBundle.LoadFromMemoryAsync(mBytes);
#endif
            yield return createReq;

            mBundle = createReq.assetBundle;

            if (mBundle != null)
            {
                mRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                mRefCountResLoadResultNotify.resLoadState.setFailed();
            }

            mRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);

            clearInstanceListener();
        }

        protected GameObject loadBundle(string resName)
        {
            // 目前只能同步加载
            //if (mResNeedCoroutine)
            //{
            //    return loadBundleAsync(resName);
            //}
            //else
            //{
                return loadBundleSync(resName);
            //}
        }

        protected GameObject loadBundleSync(string resName)
        {
            mObject = mBundle.LoadAsset<Object>(m_bundlePath);
            return mObject as GameObject;
        }

        protected GameObject loadBundleAsync(string resName)
        {
            Ctx.mInstance.mCoroutineMgr.StartCoroutine(loadBundleByCoroutine());
            return null;
        }

        protected IEnumerator loadBundleByCoroutine()
        {
            AssetBundleRequest req = null;

            if (mBundle != null)
            {
#if UNITY_5
                // Unity5
                req = mBundle.LoadAssetAsync(m_bundlePath);
#elif UNITY_4_6 || UNITY_4_5
                // Unity4
                req = mBundle.LoadAsync(mPrefabName, typeof(GameObject));
#endif
                yield return req;
            }

            if (req != null && req.isDone)
            {
                mObject = req.asset;

                mRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                mRefCountResLoadResultNotify.resLoadState.setFailed();
            }

            mRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        override public GameObject InstantiateObject(string resName)
        {
            // 不能直接将 LoadAsync 加载出来的 GameObject 添加到场景中去
            m_bundlePath = Path.Combine(PRE_PATH, resName);
            loadBundle(m_bundlePath);
            mRetGO = null;

            if (mObject != null)
            {
                mRetGO = GameObject.Instantiate(mObject) as GameObject;
                if (null == mRetGO)
                {
                    
                }
            }

            return mRetGO;
        }

        override public UnityEngine.Object getObject(string resName)
        {
            if(mObject != null)
            {
                return mObject;
            }

            return null;
        }

        override public byte[] getBytes(string resName)            // 获取字节数据
        {
            if (mBytes != null)
            {
                return mBytes;
            }

            return null;
        }

        override public string getText(string resName)
        {
            if (mBytes != null)
            {
                return System.Text.Encoding.UTF8.GetString(mBytes);
            }

            return null;
        }

        override public void unload(bool unloadAllLoadedObjects = true)
        {
            //UtilApi.Destroy(mObject);      // LoadAssetAsync 加载出来的 GameObject 是不能 Destroy 的，只能有 Unload(true) 或者 Resources.UnloadUnusedAssets 卸载
            //mBytes = null;
            //mRetGO = null;
            //mBundle.Unload(true);
            //mBundle.Unload(false);

            mRetGO = null;

            base.unload(unloadAllLoadedObjects);
        }

        // 清理实例化事件监听器
        protected void clearInstanceListener()
        {

        }
    }
}