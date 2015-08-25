using SDK.Lib;
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
        protected Object m_object;
        protected GameObject m_retGO;       // 方便调试的临时对象

        override public void init(LoadItem item)
        {
            base.init(item);
            initByBytes((item as ABUnPakLoadItem).m_bytes, PRE_PATH);
        }

        override protected void initAsset()
        {
            base.initAsset();

            if (m_bundle != null)
            {
                refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                refCountResLoadResultNotify.resLoadState.setFailed();
            }
            refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        override protected IEnumerator initAssetByCoroutine()
        {
            AssetBundleCreateRequest createReq = AssetBundle.CreateFromMemory(m_bytes);
            yield return createReq;

            m_bundle = createReq.assetBundle;

            if (m_bundle != null)
            {
                refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                refCountResLoadResultNotify.resLoadState.setFailed();
            }

            refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);

            clearInstanceListener();
        }

        protected GameObject loadBundle(string resName)
        {
            // 目前只能同步加载
            //if (m_resNeedCoroutine)
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
            m_object = m_bundle.LoadAsset<Object>(m_bundlePath);
            return m_object as GameObject;
        }

        protected GameObject loadBundleAsync(string resName)
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
#elif UNITY_4_6 || UNITY_4_5
                // Unity4
                req = m_bundle.LoadAsync(m_prefabName, typeof(GameObject));
#endif
                yield return req;
            }

            if (req != null && req.isDone)
            {
                m_object = req.asset;

                refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                refCountResLoadResultNotify.resLoadState.setFailed();
            }

            refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        override public GameObject InstantiateObject(string resName)
        {
            // 不能直接将 LoadAsync 加载出来的 GameObject 添加到场景中去
            m_bundlePath = Path.Combine(PRE_PATH, resName);
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

        override public UnityEngine.Object getObject(string resName)
        {
            if(m_object != null)
            {
                return m_object;
            }

            return null;
        }

        override public byte[] getBytes(string resName)            // 获取字节数据
        {
            if (m_bytes != null)
            {
                return m_bytes;
            }

            return null;
        }

        override public string getText(string resName)
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