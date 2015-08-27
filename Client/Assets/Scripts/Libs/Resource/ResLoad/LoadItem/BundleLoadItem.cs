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
            if (ResLoadType.eLoadDisc == m_resLoadType)
            {
                loadFromAssetBundle();
            }
            else if (ResLoadType.eLoadDicWeb == m_resLoadType || ResLoadType.eLoadWeb == m_resLoadType)
            {
                Ctx.m_instance.m_coroutineMgr.StartCoroutine(downloadAsset());
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
        // AssetBundle.CreateFromFile 这个函数仅支持未压缩的资源。这是加载资产包的最快方式。自己被这个函数坑了好几次，一定是非压缩的资源，如果压缩式不能加载的，加载后，内容也是空的
        protected void loadFromAssetBundle()
        {
            string path;
            path = Application.dataPath + "/" + m_path;
            m_assetBundle = AssetBundle.CreateFromFile(path);

            if (m_assetBundle != null)
            {
                nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                nonRefCountResLoadResultNotify.resLoadState.setFailed();
            }

            nonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }
    }
}