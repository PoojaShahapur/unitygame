using SDK.Common;
using System.Collections;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 没有打包的系统，在没有打包之前使用这个加载系统
     */
    public class ABUnPakLevelFileResItem : ABMemUnPakFileResItemBase
    {
        protected string m_levelName;

        public string levelName
        {
            set
            {
                m_levelName = value;
            }
        }

        override public void init(LoadItem item)
        {
            initByBytes((item as ABUnPakLoadItem).m_bytes, SCENE_PRE_PATH);
        }

        override protected void initAsset()
        {
            base.initAsset();

            if (m_bundle != null)
            {
                m_bundle.LoadAsset<GameObject>(m_bundlePath);
                Application.LoadLevel(m_levelName);
                refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                refCountResLoadResultNotify.resLoadState.setFailed();
            }

            refCountResLoadResultNotify.loadEventDispatch.dispatchEvent(this);

            clearListener();
        }

        override protected IEnumerator initAssetByCoroutine()
        {
            AssetBundleCreateRequest createReq = AssetBundle.CreateFromMemory(m_bytes);
            yield return createReq;

            m_bundle = createReq.assetBundle;

            AssetBundleRequest req = null;

            if (m_bundle != null)
            {
#if UNITY_5
                // Unity5
                req = m_bundle.LoadAssetAsync(m_bundlePath);
#elif UNITY_4_6
                // Unity4
                req = m_bundle.LoadAsync(m_prefabName, typeof(GameObject));
#endif
                yield return req;
            }

            AsyncOperation asyncOpt = Application.LoadLevelAsync(m_levelName);
            yield return asyncOpt;

            if (null != asyncOpt && asyncOpt.isDone)
            {
                refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                refCountResLoadResultNotify.resLoadState.setFailed();
            }
            refCountResLoadResultNotify.loadEventDispatch.dispatchEvent(this);

            clearListener();

            yield return null;
        }
    }
}