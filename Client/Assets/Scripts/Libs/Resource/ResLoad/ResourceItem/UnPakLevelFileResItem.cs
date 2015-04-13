using SDK.Common;
using System.Collections;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 没有打包的系统，在没有打包之前使用这个加载系统
     */
    public class UnPakLevelFileResItem : FileResItem
    {
        public const string SCENE_PRE_PATH = "Assets/Scenes";
        public byte[] m_bytes;
        protected AssetBundle m_bundle;
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
            m_bytes = (item as UnPakLoadItem).m_bytes;

            m_bundlePath = Path.Combine(SCENE_PRE_PATH, m_path);
            m_bundlePath = string.Format("{0}.{1}", m_bundlePath, m_extName);

            // 检查是否资源打包成 unity3d 
            if (Ctx.m_instance.m_cfg.m_pakExtNameList.IndexOf(m_extName) != -1)
            {
                if (m_resNeedCoroutine)
                {
                    Ctx.m_instance.m_coroutineMgr.StartCoroutine(initAssetByCoroutine());
                }
                else
                {
                    initAsset();
                }
            }
            else
            {
                if (onLoaded != null)
                {
                    onLoaded(this);
                }

                clearListener();
            }
        }

        protected void initAsset()
        {
            m_bundle = AssetBundle.CreateFromMemoryImmediate(m_bytes);

            if (m_bundle != null)
            {
                m_bundle.LoadAsset<GameObject>(m_bundlePath);
                Application.LoadLevel(m_levelName);
                if (onLoaded != null)
                {
                    onLoaded(this);
                }
            }
            else
            {
                if (onFailed != null)
                {
                    onFailed(this);
                }
            }

            clearListener();
        }

        protected IEnumerator initAssetByCoroutine()
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
                if (onLoaded != null)
                {
                    onLoaded(this);
                }
            }
            else
            {
                if (onFailed != null)
                {
                    onFailed(this);
                }
            }

            clearListener();

            yield return null;
        }

        override public void unload()
        {
            m_bytes = null;
            m_bundle.Unload(false);
            m_bundle = null;
        }
    }
}