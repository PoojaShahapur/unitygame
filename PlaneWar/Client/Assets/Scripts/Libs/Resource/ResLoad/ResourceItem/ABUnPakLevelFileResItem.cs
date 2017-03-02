using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 没有打包的系统，在没有打包之前使用这个加载系统
     */
    public class ABUnPakLevelFileResItem : ABMemUnPakFileResItemBase
    {
        protected string mLevelName;

        public string levelName
        {
            set
            {
                mLevelName = value;
            }
        }

        override public void init(LoadItem item)
        {
            initByBytes((item as ABUnPakLoadItem).mBytes, SCENE_PRE_PATH);
        }

        override protected void initAsset()
        {
            base.initAsset();
            if (mBundle != null)
            {
                mBundle.LoadAsset<GameObject>(m_bundlePath);
                Application.LoadLevel(mLevelName);
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

            AsyncOperation asyncOpt = Application.LoadLevelAsync(mLevelName);
            yield return asyncOpt;

            if (null != asyncOpt && asyncOpt.isDone)
            {
                mRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                mRefCountResLoadResultNotify.resLoadState.setFailed();
            }
            mRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
            //yield return null;
        }
    }
}