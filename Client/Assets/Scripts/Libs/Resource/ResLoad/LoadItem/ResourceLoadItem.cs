using SDK.Common;
using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 仅仅支持从 Resources 默认 Bundle 包中加载
     */
    public class ResourceLoadItem : LoadItem
    {
        protected UnityEngine.Object m_prefabObj;

        public UnityEngine.Object prefabObj
        {
            get
            {
                return m_prefabObj;
            }
            set
            {
                m_prefabObj = value;
            }
        }

        override public void load()
        {
            base.load();
            if (m_loadNeedCoroutine)
            {
                loadFromDefaultAssetBundleByCoroutine();
            }
            else
            {
                loadFromDefaultAssetBundle();
            }
        }

        // Resources.Load就是从一个缺省打进程序包里的AssetBundle里加载资源，而一般AssetBundle文件需要你自己创建，运行时 动态加载，可以指定路径和来源的。
        protected void loadFromDefaultAssetBundle()
        {
            //string path = Application.dataPath + "/" + m_path;
            m_prefabObj = Resources.Load(m_pathNoExt);
            // Resources.LoadAsync unity5.0 中暂时不支持
            //ResourceRequest req = Resources.LoadAsync<GameObject>(path);
            
            if (onLoaded != null)
            {
                onLoaded(this);
            }
        }

        protected IEnumerator loadFromDefaultAssetBundleByCoroutine()
        {
            ResourceRequest req = Resources.LoadAsync<GameObject>(m_pathNoExt);
            yield return req;

            if (req.asset != null && req.isDone)
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
        }
    }
}