using SDK.Common;
using System.Collections;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 没有打包的系统，在没有打包之前使用这个加载系统，每一个 ResItem 只有一个资源，打包的资源也是每一个 item 只有一个资源包
     */
    public class ABUnPakComFileResItem : ABUnPakFileResItemBase
    {
        protected Object m_object;
        protected GameObject m_retGO;       // 方便调试的临时对象

        override public void init(LoadItem item)
        {
            m_bytes = (item as ABUnPakLoadItem).m_bytes;
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

            clearInstanceListener();
        }

        protected GameObject loadBundle(string resname)
        {
            // 目前只能同步加载
            //if (m_resNeedCoroutine)
            //{
            //    return loadBundleAsync(resname);
            //}
            //else
            //{
                return loadBundleSync(resname);
            //}
        }

        protected GameObject loadBundleSync(string resname)
        {
            m_object = m_bundle.LoadAsset<Object>(m_bundlePath);
            return m_object as GameObject;
        }

        protected GameObject loadBundleAsync(string resname)
        {
            Ctx.m_instance.m_coroutineMgr.StartCoroutine(loadBundleByCoroutine());
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
            }

            if (req != null && req.isDone)
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
            // 不能直接将 LoadAsync 加载出来的 GameObject 添加到场景中去
            m_bundlePath = Path.Combine(PRE_PATH, resname);
            loadBundle(m_bundlePath);
            m_retGO = null;

            if (m_object != null)
            {
                m_retGO = GameObject.Instantiate(m_object) as GameObject;
                if (null == m_retGO)
                {
                    Ctx.m_instance.m_logSys.log("不能实例化数据");
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

        override public void unload()
        {
            //UtilApi.Destroy(m_object);      // LoadAssetAsync 加载出来的 GameObject 是不能 Destroy 的，只能有 Unload(true) 或者 Resources.UnloadUnusedAssets 卸载
            //m_bytes = null;
            //m_retGO = null;
            //m_bundle.Unload(true);
            //m_bundle.Unload(false);

            base.unload();
            m_retGO = null;
        }

        // 清理实例化事件监听器
        protected void clearInstanceListener()
        {

        }
    }
}