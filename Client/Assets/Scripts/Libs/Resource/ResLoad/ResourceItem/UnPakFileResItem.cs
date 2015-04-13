using SDK.Common;
using System.Collections;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 没有打包的系统，在没有打包之前使用这个加载系统
     */
    public class UnPakFileResItem : FileResItem
    {
        public byte[] m_bytes;
        protected AssetBundle m_bundle;
        protected Object m_object;
        protected GameObject m_retGO;       // 方便调试的临时对象

        override public void init(LoadItem item)
        {
            m_bytes = (item as UnPakLoadItem).m_bytes;
            m_bundlePath = Path.Combine(PRE_PATH, m_path);

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

            if (m_bundle != null)
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

        protected GameObject loadBundle(string resname)
        {
            // 目前只能同步加载
            //if (m_resNeedCoroutine)
            //{
                return loadBundleAsync(resname);
            //}
            //else
            //{
            //    loadBundleSync(resname);
            //}
        }

        protected GameObject loadBundleSync(string resname)
        {
            m_object = m_bundle.LoadAsset<Object>(m_bundlePath);
            return m_object as GameObject;
        }

        protected GameObject loadBundleAsync(string resname)
        {
            Ctx.m_instance.m_coroutineMgr.StartCoroutine(initAssetByCoroutine());
            return null;
        }

        protected IEnumerator loadBundleByCoroutine()
        {
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

                //m_bundle.Unload(false);     // 卸载自身资源
                // m_bundle.Unload(true);      // 真正卸载的时候全部卸载
            }

            if (req != null && req.asset != null)
            {
                m_object = req.asset;

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
        }

        override public GameObject InstantiateObject(string resname)
        {
            // 一定不能直接返回 bundle 中直接加载的对象，否则虽然能挂在到场景中，但是会丢失上面所有的脚本
            //if((m_object as GameObject) != null)
            //{
            //    return m_object as GameObject;
            //}
            m_bundlePath = Path.Combine(PRE_PATH, resname);
            loadBundle(m_bundlePath);
            m_retGO = null;

            if (m_object != null)
            {
                m_retGO = GameObject.Instantiate(m_object) as GameObject;
                if (null == m_retGO)
                {
                    Ctx.m_instance.m_log.log("不能实例化数据");
                }
            }

            return m_retGO;
        }

        override public UnityEngine.Object getObject(string resname)
        {
            if(m_object != null)
            {
                return m_object;
            }

            return null;
        }

        override public byte[] getBytes(string resname)            // 获取字节数据
        {
            if (m_bytes != null)
            {
                return m_bytes;
            }

            return null;
        }

        override public string getText(string resname)
        {
            if (m_bytes != null)
            {
                return System.Text.Encoding.UTF8.GetString(m_bytes);
            }

            return null;
        }
    }
}